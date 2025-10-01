using System;
using System.Linq;
using System.Collections.Generic;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Components.Timer;
using MultiPlug.Ext.IFTTT.Models.Components;
using MultiPlug.Ext.IFTTT.Models.Exchange;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Timers
{
    [Route("timers/timer")]
    public class TimerController : SettingsApp
    {
        public Response Get(string id)
        {
            TimerComponent Timer = null;

            if ( ! string.IsNullOrEmpty(id))
            {
                Timer = Core.Instance.Timers.FirstOrDefault(t => t.ElapsedEvent.Guid == id);
            }

            if( Timer == null)
            {
                Timer = new TimerComponent(Guid.NewGuid().ToString());
            }

            return new Response
            {
                Model = new Models.Settings.Timer
                {
                    Guid = id,
                    EventId = Timer.ElapsedEvent.Id,
                    EventDescription = Timer.ElapsedEvent.Description,
                    EventSubject = Timer.ElapsedEvent.Subjects[0],
                    EventSubjectValue = Timer.ElapsedEvent.ElapsedValue,
                    EventSubjectTime = Timer.ElapsedEvent.AddDataTimeSubject,
                    EventSubjectInterval = Timer.ElapsedEvent.AddIntervalSubject,
                    AutoReset = Timer.AutoReset.Value,
                    Interval = Timer.Interval.Value,
                    Enabled = Timer.Enabled.Value,
                    StartOnSystemStart = Timer.StartOnSystemStart.Value,
                    StartSubscriptionGuid = Timer.StartSubscriptions.Select( s => s.Guid).ToArray(),
                    StartSubscriptionId = Timer.StartSubscriptions.Select(s => s.Id).ToArray(),
                    StartSubscriptionConnected = Timer.StartSubscriptions.Select(s => s.Connected).ToArray(),
                    StopSubscriptionGuid = Timer.StopSubscriptions.Select(s => s.Guid).ToArray(),
                    StopSubscriptionId = Timer.StopSubscriptions.Select(s => s.Id).ToArray(),
                    StopSubscriptionConnected = Timer.StopSubscriptions.Select(s => s.Connected).ToArray()
                },
                Template = "GetTimerViewContents"
            };
        }

        public Response Post(Models.Settings.Timer theModel)
        {
            if(string.IsNullOrEmpty(theModel.Guid))
            {
                theModel.Guid = System.Guid.NewGuid().ToString();
            }

            var StartSubscriptions = new List<Subscription>();

            if (theModel.StartSubscriptionGuid != null &&
                theModel.StartSubscriptionId != null &&
                theModel.StartSubscriptionGuid.Length == theModel.StartSubscriptionId.Length)
            {
                for (int i = 0; i < theModel.StartSubscriptionGuid.Length; i++)
                {
                    if (string.IsNullOrEmpty(theModel.StartSubscriptionId[i]))
                    {
                        continue;
                    }

                    StartSubscriptions.Add(new Subscription
                    {
                        Guid = theModel.StartSubscriptionGuid[i],
                        Id = theModel.StartSubscriptionId[i]
                    });
                }
            }

            var StopSubscriptions = new List<Subscription>();

            if (theModel.StopSubscriptionGuid != null &&
                theModel.StopSubscriptionId != null &&
                theModel.StopSubscriptionGuid.Length == theModel.StopSubscriptionId.Length)
            {
                for (int i = 0; i < theModel.StopSubscriptionGuid.Length; i++)
                {
                    if (string.IsNullOrEmpty(theModel.StopSubscriptionId[i]))
                    {
                        continue;
                    }

                    StopSubscriptions.Add(new Subscription
                    {
                        Guid = theModel.StopSubscriptionGuid[i],
                        Id = theModel.StopSubscriptionId[i]
                    });
                }
            }

            Core.Instance.Update(new TimerProperties
            {
                ElapsedEvent = new TimerEvent
                {
                    Guid = theModel.Guid,
                    Id = theModel.EventId,
                    Description = theModel.EventDescription,
                    Subjects = new string[] { theModel.EventSubject },
                    Group = "Timer",
                    ElapsedValue = theModel.EventSubjectValue,
                    AddDataTimeSubject = theModel.EventSubjectTime,
                    AddIntervalSubject = theModel.EventSubjectInterval
                },
                Interval = theModel.Interval,
                AutoReset = theModel.AutoReset,
                Enabled = theModel.Enabled,
                StartOnSystemStart = theModel.StartOnSystemStart,
                StartSubscriptions = StartSubscriptions.ToArray(),
                StopSubscriptions = StopSubscriptions.ToArray()
            });

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Moved,
                Location = new Uri(Context.Referrer, "?id=" + theModel.Guid)
            };
        }
    }
}
