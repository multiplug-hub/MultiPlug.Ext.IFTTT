using System;
using System.Linq;
using System.Collections.Generic;
using MultiPlug.Base.Http;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Models.Settings;
using MultiPlug.Ext.IFTTT.Components.Aggregator;
using MultiPlug.Ext.IFTTT.Models.Exchange;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Aggregators
{
    [Route("aggregators/aggregator")]
    public class AggregatorController : SettingsApp
    {
        public Response Get( string id )
        {
            AggregatorComponent Aggregator = null;

            if (!string.IsNullOrEmpty(id))
            {
                Aggregator = Core.Instance.Aggregators.FirstOrDefault(t => t.Guid == id);
            }

            if( Aggregator == null)
            {
                Aggregator = new AggregatorComponent(Guid.NewGuid().ToString());
            }

            return new Response
            {
                Model = new Aggregator
                {
                    Guid = id,
                    EventId = Aggregator.Event.Id,
                    EventDescription = Aggregator.Event.Description,
                    AggregateSubscriptionGuid = Aggregator.AggregatedSubscriptions.Select(s => s.Guid).ToArray(),
                    AggregateSubscriptionId = Aggregator.AggregatedSubscriptions.Select(s => s.Id).ToArray(),
                    AggregateSubscriptionConnected = Aggregator.AggregatedSubscriptions.Select(s => s.Connected).ToArray(),
                    ResetSubscriptionGuid = Aggregator.ResetSubscriptions.Select(s => s.Guid).ToArray(),
                    ResetSubscriptionId = Aggregator.ResetSubscriptions.Select(s => s.Id).ToArray(),
                    ResetSubscriptionConnected = Aggregator.ResetSubscriptions.Select(s => s.Connected).ToArray(),
                },
                Template = "GetAggregatorViewContents"
            };
        }

        public Response Post(Aggregator theModel)
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
                var AggregateSubscriptions = new List<AggregatorSubscription>();

                if (theModel.AggregateSubscriptionGuid != null &&
                    theModel.AggregateSubscriptionId != null && 
                    theModel.AggregateSubscriptionGuid.Length == theModel.AggregateSubscriptionId.Length)
                {
                    for (int i = 0; i < theModel.AggregateSubscriptionGuid.Length; i++)
                    {
                        if (string.IsNullOrEmpty(theModel.AggregateSubscriptionId[i]))
                        {
                            continue;
                        }

                        AggregateSubscriptions.Add(new AggregatorSubscription
                        {
                            Guid = (string.IsNullOrEmpty(theModel.AggregateSubscriptionGuid[i])) ? Guid.NewGuid().ToString() : theModel.AggregateSubscriptionGuid[i],
                            Id = theModel.AggregateSubscriptionId[i],
                        });
                    }
                }

                var ResetSubscriptions = new List<Subscription>();

                if (theModel.ResetSubscriptionGuid != null &&
                    theModel.ResetSubscriptionId != null && 
                    theModel.ResetSubscriptionGuid.Length == theModel.ResetSubscriptionId.Length)
                {
                    for (int i = 0; i < theModel.ResetSubscriptionGuid.Length; i++)
                    {
                        if (string.IsNullOrEmpty(theModel.ResetSubscriptionId[i]))
                        {
                            continue;
                        }

                        ResetSubscriptions.Add(new Subscription
                        {
                            Guid = (string.IsNullOrEmpty(theModel.ResetSubscriptionGuid[i])) ? Guid.NewGuid().ToString() : theModel.ResetSubscriptionGuid[i],
                            Id = theModel.ResetSubscriptionId[i],
                        });
                    }
                }

                Core.Instance.Update(new Models.Components.Aggregator.AggregatorProperties {
                    Event = new Event
                    {
                        Guid = theModel.Guid,
                        Id = theModel.EventId,
                        Description = theModel.EventDescription
                    },
                    AggregatedSubscriptions = AggregateSubscriptions.ToArray(),
                    ResetSubscriptions = ResetSubscriptions.ToArray()
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
