using System;
using System.Linq;
using System.Timers;
using System.Collections.Generic;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Models.Exchange;
using MultiPlug.Ext.IFTTT.Models.Components;

namespace MultiPlug.Ext.IFTTT.Components.Timer
{
    public class TimerComponent : TimerProperties
    {
        public event Action EventsUpdated;
        public event Action SubscriptionsUpdated;

        private System.Timers.Timer m_Timer;

        private Payload m_Cache = new Payload(string.Empty, new PayloadSubject[0]);

        public bool Running { get { return m_Timer.Enabled; } }
        public bool StartedBySystemStartFlag { get; set; }

        public TimerComponent(string theGuid)
        {
            StartSubscriptions = new Subscription[0];
            StopSubscriptions = new Subscription[0];
            Interval = 1000;
            Enabled = true;
            AutoReset = false;
            ElapsedEvent = new TimerEvent
            {
                Guid = theGuid,
                Id = "IFTTT.Timer." + System.Guid.NewGuid().ToString().Substring(9, 4),
                Description = "Timer",
                Group = "Timers",
                Subjects = new string[] { "value", "interval", "time" },
                CachedPayload = new Func<Payload>(CachedValue),
                ElapsedValue = "1",
                AddIntervalSubject = true,
                AddDataTimeSubject = true
            };

            StartOnSystemStart = true;

            m_Timer = new System.Timers.Timer();
            m_Timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            m_Timer.Interval = Interval.Value;
            m_Timer.AutoReset = AutoReset.Value;
        }

        internal void UpdateProperties(Models.Components.TimerProperties theNewProperties)
        {
            bool SuUpdated = false;
            bool EvUpdated = false;

            if (theNewProperties.ElapsedEvent.Guid != ElapsedEvent.Guid)
                return;

            if (theNewProperties.ElapsedEvent != null)
            {
                if (Event.Merge(ElapsedEvent, theNewProperties.ElapsedEvent, false))
                {
                    EvUpdated = true;
                }

                ElapsedEvent.Subjects[0] = theNewProperties.ElapsedEvent.Subjects[0];
                ElapsedEvent.ElapsedValue = theNewProperties.ElapsedEvent.ElapsedValue;
                ElapsedEvent.AddIntervalSubject = theNewProperties.ElapsedEvent.AddIntervalSubject;
                ElapsedEvent.AddDataTimeSubject = theNewProperties.ElapsedEvent.AddDataTimeSubject;
            }

            if (theNewProperties.Enabled != null && Enabled != theNewProperties.Enabled)
            {
                Enabled = theNewProperties.Enabled;

                if( ! Enabled.Value)
                {
                    m_Timer.Stop();
                }
            }

            if (theNewProperties.AutoReset != null && AutoReset != theNewProperties.AutoReset)
            {
                AutoReset = theNewProperties.AutoReset;
                m_Timer.AutoReset = AutoReset.Value;
            }

            if(theNewProperties.Interval != null && theNewProperties.Interval != Interval && theNewProperties.Interval > 0)
            {
                Interval = theNewProperties.Interval;
                m_Timer.Interval = Interval.Value;
            }

            if (theNewProperties.StartOnSystemStart != null && theNewProperties.StartOnSystemStart != StartOnSystemStart)
            {
                StartOnSystemStart = theNewProperties.StartOnSystemStart;
            }

            if(theNewProperties.StartSubscriptions != null)
            {
                var StEDeleted = StartSubscriptions.Where( e => theNewProperties.StartSubscriptions.FirstOrDefault( ne => ne.Guid == e.Guid) == null);
                if(StEDeleted.Count() > 0){ SuUpdated = true; }
                StartSubscriptions = StartSubscriptions.Except(StEDeleted).ToArray();

                var StENew = theNewProperties.StartSubscriptions.Where(e => StartSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (StENew.Count() > 0) { SuUpdated = true; }
                foreach( var item in StENew ){ item.Guid = System.Guid.NewGuid().ToString(); item.Event += OnStart; }
                StartSubscriptions = StartSubscriptions.Concat(StENew).ToArray();
            }

            if(theNewProperties.StopSubscriptions != null)
            {
                var SpEDeleted = StopSubscriptions.Where(e => theNewProperties.StopSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (SpEDeleted.Count() > 0) { SuUpdated = true; }
                StopSubscriptions = StopSubscriptions.Except(SpEDeleted).ToArray();

                var SpENew = theNewProperties.StopSubscriptions.Where(e => StopSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (SpENew.Count() > 0) { SuUpdated = true; }
                foreach (var item in SpENew){ item.Guid = System.Guid.NewGuid().ToString(); item.Event += OnStop; }
                StopSubscriptions = StopSubscriptions.Concat(SpENew).ToArray();
            }

           // OnSystemStarted();

            if(EvUpdated && EventsUpdated != null)
            {
                EventsUpdated();
            }
            
            if(SuUpdated && SubscriptionsUpdated != null)
            {
                SubscriptionsUpdated();
            }
        }

        private void OnStop(SubscriptionEvent obj)
        {
            Stop();
        }

        private void OnStart(SubscriptionEvent obj)
        {
            Start();
        }

        internal void Start()
        {
            m_Timer.Start();
        }

        public void Stop()
        {
            m_Timer.Stop();
        }

        internal void OnSystemStarted()
        {
            if (Enabled.Value && StartOnSystemStart.Value && ( !m_Timer.Enabled ) && ( !StartedBySystemStartFlag ) )
            {
                StartedBySystemStartFlag = true;
                m_Timer.Start();
            }
        }

        internal void Remove(Subscription theSubscription, bool isStopSubscription)
        {
            Subscription[] Subscriptions;

            if (isStopSubscription)
            {
                Subscriptions = StopSubscriptions;
            }
            else
            {
                Subscriptions = StartSubscriptions;
            }

            var Search = Subscriptions.FirstOrDefault(Subscription => Subscription.Guid == theSubscription.Guid);

            if(Search != null)
            {
                var SubscriptionsList = Subscriptions.ToList();
                SubscriptionsList.Remove(Search);

                if (isStopSubscription)
                {
                    StopSubscriptions = SubscriptionsList.ToArray();
                }
                else
                {
                    StartSubscriptions = SubscriptionsList.ToArray();
                }

                SubscriptionsUpdated();
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!Enabled.Value)
                return;

            List<PayloadSubject> Subjects = new List<PayloadSubject>();

            Subjects.Add( new PayloadSubject( ElapsedEvent.Subjects[0], ElapsedEvent.ElapsedValue) );
            Subjects.Add( new PayloadSubject( ElapsedEvent.Subjects[1], (ElapsedEvent.AddIntervalSubject) ? Interval.ToString() : string.Empty ) );
            Subjects.Add( new PayloadSubject( ElapsedEvent.Subjects[2], (ElapsedEvent.AddDataTimeSubject) ? DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") : string.Empty ) );

            m_Cache = new Payload(ElapsedEvent.Id, Subjects.ToArray());

            ElapsedEvent.Invoke(m_Cache);
        }

        private Payload CachedValue()
        {
            return m_Cache;
        }
    }
}
