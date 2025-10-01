using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using MultiPlug.Base.Http;
using MultiPlug.Base.Attribute;
using MultiPlug.Ext.IFTTT.Models.Components;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Components.Counter;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Counter
{
    [Route("counters/counter")]
    public class CounterController : SettingsApp
    {
        public Response Get(string id)
        {
            CounterComponent Counter = null;

            if (!string.IsNullOrEmpty(id))
            {
                Counter = Core.Instance.Counters.FirstOrDefault(t => t.GoalEvent.Guid == id);
            }

            if (Counter == null)
            {
                Counter = new CounterComponent(Guid.NewGuid().ToString());
            }

            return new Response
            {
                Model = new Models.Settings.Counter
                {
                    Guid = id,
                    Enabled = Counter.Enabled.Value,
                    GoalEventId = Counter.GoalEvent.Id,
                    GoalEventDescription = Counter.GoalEvent.Description,
                    Goal = Counter.Goal.Value,
                    AutoReset = Counter.AutoReset.Value,
                    IncrementSubscriptionGuid = Counter.IncrementSubscriptions.Select(s => s.Guid).ToArray(),
                    IncrementSubscriptionId = Counter.IncrementSubscriptions.Select(s => s.Id).ToArray(),
                    IncrementSubscriptionConnected = Counter.IncrementSubscriptions.Select(s => s.Connected).ToArray(),
                    ResetSubscriptionGuid = Counter.ResetSubscriptions.Select(s => s.Guid).ToArray(),
                    ResetSubscriptionId = Counter.ResetSubscriptions.Select(s => s.Id).ToArray(),
                    ResetSubscriptionConnected = Counter.ResetSubscriptions.Select(s => s.Connected).ToArray()
                },
                Template = "GetCounterViewContents"
            };
        }

        public Response Post(Models.Settings.Counter theModel)
        {
            if (string.IsNullOrEmpty(theModel.Guid))
            {
                theModel.Guid = System.Guid.NewGuid().ToString();
            }

            var IncrementSubscriptions = new List<Subscription>();

            if (theModel.IncrementSubscriptionGuid != null &&
                theModel.IncrementSubscriptionId != null &&
                theModel.IncrementSubscriptionGuid.Length == theModel.IncrementSubscriptionId.Length)
            {
                for (int i = 0; i < theModel.IncrementSubscriptionGuid.Length; i++)
                {
                    if (string.IsNullOrEmpty(theModel.IncrementSubscriptionId[i]))
                    {
                        continue;
                    }

                    IncrementSubscriptions.Add(new Subscription
                    {
                        Guid = theModel.IncrementSubscriptionGuid[i],
                        Id = theModel.IncrementSubscriptionId[i]
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
                        Guid = theModel.ResetSubscriptionGuid[i],
                        Id = theModel.ResetSubscriptionId[i]
                    });
                }
            }

            Core.Instance.Update(new CounterProperties
            {
                GoalEvent = new Event
                {
                    Guid = theModel.Guid,
                    Id = theModel.GoalEventId,
                    Description = theModel.GoalEventDescription,
                    //Subjects = new string[] { theModel.EventSubject },
                    Group = "Counters",
                    //ElapsedValue = theModel.EventSubjectValue,
                    //AddDataTimeSubject = theModel.EventSubjectTime,
                    //AddIntervalSubject = theModel.EventSubjectInterval
                },
                Enabled = theModel.Enabled,
                Goal = theModel.Goal,
                AutoReset = theModel.AutoReset,
                IncrementSubscriptions = IncrementSubscriptions.ToArray(),
                ResetSubscriptions = ResetSubscriptions.ToArray()
            });

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = new Uri(Context.Referrer, "?id=" + theModel.Guid)
            };
        }
    }
}
