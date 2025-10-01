using System;
using System.Linq;
using System.Collections.Generic;

using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Models.Components;
using MultiPlug.Ext.IFTTT.Components.Cache;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Caches
{
    [Route("caches/cache")]
    public class CacheController : SettingsApp
    {
        public Response Get( string id )
        {
            CacheComponent Cache = null;

            if (!string.IsNullOrEmpty(id))
            {
                Cache = Core.Instance.Caches.FirstOrDefault(t => t.Guid == id);
            }

            if (Cache == null)
            {
                Cache = new CacheComponent(Guid.NewGuid().ToString());
            }


            return new Response
            {
                Model = new Models.Settings.Cache
                {
                    Guid = id,
                    EventId = Cache.ResponseEvent.Id,
                    EventDescription = Cache.ResponseEvent.Description,
                    EventSubjects = Cache.ResponseEvent.Subjects,
                    SubscriptionGuid = Cache.Subscriptions.Select(s => s.Guid).ToArray(),
                    SubscriptionId = Cache.Subscriptions.Select(s => s.Id).ToArray(),
                    SubscriptionConnected = Cache.Subscriptions.Select(s => s.Connected).ToArray(),

                    RequestSubscriptionGuid = Cache.RequestSubscriptions.Select(s => s.Guid).ToArray(),
                    RequestSubscriptionId = Cache.RequestSubscriptions.Select(s => s.Id).ToArray(),
                    RequestSubscriptionConnected = Cache.RequestSubscriptions.Select(s => s.Connected).ToArray(),
                    InvokeEventOnSubjectValue = Cache.InvokeEventOnSubjectValue,
                    CurrentValues = Cache.Cache.Subjects == null ? new string[0] : Cache.Cache.Subjects.Select( s => s.Value).ToArray()
                },
                Template = "GetCacheViewContents"
            };
        }

        public Response Post(Models.Settings.Cache theModel)
        {
            if (string.IsNullOrEmpty(theModel.Guid))
            {
                theModel.Guid = System.Guid.NewGuid().ToString();
            }

            var Subscriptions = new List<Subscription>();

            if (theModel.SubscriptionGuid != null &&
                theModel.SubscriptionId != null &&
                theModel.SubscriptionGuid.Length == theModel.SubscriptionId.Length)
            {
                for (int i = 0; i < theModel.SubscriptionGuid.Length; i++)
                {
                    if (string.IsNullOrEmpty(theModel.RequestSubscriptionId[i]))
                    {
                        continue;
                    }

                    Subscriptions.Add(new Subscription
                    {
                        Guid = theModel.SubscriptionGuid[i],
                        Id = theModel.SubscriptionId[i]
                    });
                }
            }

            var RequestSubscriptions = new List<Subscription>();

            if (theModel.RequestSubscriptionGuid != null &&
                theModel.RequestSubscriptionId != null &&
                theModel.RequestSubscriptionGuid.Length == theModel.RequestSubscriptionId.Length)
            {
                for (int i = 0; i < theModel.RequestSubscriptionGuid.Length; i++)
                {
                    if (string.IsNullOrEmpty(theModel.RequestSubscriptionId[i]))
                    {
                        continue;
                    }

                    RequestSubscriptions.Add(new Subscription
                    {
                        Guid = theModel.RequestSubscriptionGuid[i],
                        Id = theModel.RequestSubscriptionId[i]
                    });
                }
            }

            Core.Instance.Update(new CacheProperties
            {
                ResponseEvent = new Event(theModel.Guid, theModel.EventId, theModel.EventDescription, theModel.EventSubjects == null ? new string[0]: theModel.EventSubjects),
                Subscriptions = Subscriptions.ToArray(),
                RequestSubscriptions = RequestSubscriptions.ToArray(),
                InvokeEventOnSubjectValue = theModel.InvokeEventOnSubjectValue
            });

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = new Uri(Context.Referrer, "?id=" + theModel.Guid)
            };
        }
    }
}