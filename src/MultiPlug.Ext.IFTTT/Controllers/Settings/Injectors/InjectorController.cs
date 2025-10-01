using System;
using System.Linq;
using System.Collections.Generic;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.IFTTT.Models.Settings;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Components.Injector;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Injectors
{
    [Route("injectors/injector")]
    public class InjectorController : SettingsApp
    {
        public Response Get(string id)
        {
            InjectorComponent Injector = null;

            if (!string.IsNullOrEmpty(id))
            {
                Injector = Core.Instance.Injectors.FirstOrDefault(t => t.Guid == id);
            }

            if (Injector == null)
            {
                Injector = new InjectorComponent(Guid.NewGuid().ToString());
            }

            return new Response
            {
                Model = new InjectorModel
                {
                    Guid = id,
                    EventId = Injector.Event.Id,
                    EventDescription = Injector.Event.Description,
                    SubscriptionGuid = Injector.Subscriptions.Select(s => s.Guid).ToArray(),
                    SubscriptionId = Injector.Subscriptions.Select(s => s.Id).ToArray(),
                    SubscriptionConnected = Injector.Subscriptions.Select(s => s.Connected).ToArray(),
                    Subject = Injector.Subjects.Select(p => p.Subject).ToArray(),
                    Value = Injector.Subjects.Select(p => p.Value).ToArray()
                },
                Template = "GetInjectorViewContents"
            };
        }

        public Response Post(InjectorModel theModel)
        {
            if (string.IsNullOrEmpty(theModel.Guid))
            {
                theModel.Guid = System.Guid.NewGuid().ToString();
            }

            if (theModel != null &&
                theModel.Guid != null &&
                theModel.EventId != null &&
                theModel.EventDescription != null
                )
            {
                var Subscriptions = new List<Subscription>();

                if (theModel.SubscriptionGuid != null &&
                    theModel.SubscriptionId != null &&
                    theModel.SubscriptionGuid.Length == theModel.SubscriptionId.Length)
                {
                    for (int i = 0; i < theModel.SubscriptionGuid.Length; i++)
                    {
                        if (string.IsNullOrEmpty(theModel.SubscriptionId[i]))
                        {
                            continue;
                        }

                        Subscriptions.Add(new Subscription
                        {
                            Guid = (string.IsNullOrEmpty(theModel.SubscriptionGuid[i])) ? Guid.NewGuid().ToString() : theModel.SubscriptionGuid[i],
                            Id = theModel.SubscriptionId[i],
                        });
                    }
                }

                var Pairs = new List<PayloadSubject>();

                if( theModel.Subject != null &&
                    theModel.Value != null &&
                    theModel.Subject.Length == theModel.Value.Length)
                {
                    for (int i = 0; i < theModel.Subject.Length; i++)
                    {
                        if (string.IsNullOrEmpty(theModel.Subject[i]))
                        {
                            continue;
                        }

                        Pairs.Add(new PayloadSubject(theModel.Subject[i], theModel.Value[i]));
                    }
                }

                Core.Instance.Update(new Models.Components.Injector.InjectorProperties
                {
                    Event = new Event { Guid = theModel.Guid, Id = theModel.EventId, Description = theModel.EventDescription },
                    Subjects = Pairs.ToArray(),
                    Subscriptions = Subscriptions.ToArray()
                });
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = new Uri(Context.Referrer, "?id=" + theModel.Guid)
            };
        }
    }
}
