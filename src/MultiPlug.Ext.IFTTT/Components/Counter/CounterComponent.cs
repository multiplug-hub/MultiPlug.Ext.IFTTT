using System;
using System.Linq;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Models.Components;

namespace MultiPlug.Ext.IFTTT.Components.Counter
{
    public class CounterComponent : CounterProperties
    {
        internal event Action EventsUpdated;
        internal event Action SubscriptionsUpdated;

        private Payload m_Count = new Payload();

        public object CountValue { get { return m_Count; } }

        internal int Current { get; set; }

        public CounterComponent(string theGuid)
        {
            IncrementSubscriptions = new Subscription[0];
            ResetSubscriptions = new Subscription[0];
            Enabled = true;
            AutoReset = true;
            Current = 0;
            Goal = 1;
            GoalEvent = new Event { Guid = theGuid, Id = "IFTTT.Counter." + System.Guid.NewGuid().ToString().Substring(9, 4), Description = "Counter", CachedPayload = new Func<Payload>(CachedValue), Group = "Counters" };

            UpdateCount();
        }

        internal void Remove(Subscription theSubscription, bool isResetSubscription)
        {
            Subscription[] Subscriptions;

            if (isResetSubscription)
            {
                Subscriptions = ResetSubscriptions;
            }
            else
            {
                Subscriptions = IncrementSubscriptions;
            }

            var Search = Subscriptions.FirstOrDefault(Subscription => Subscription.Guid == theSubscription.Guid);

            if (Search != null)
            {
                var SubscriptionsList = Subscriptions.ToList();
                SubscriptionsList.Remove(Search);

                if (isResetSubscription)
                {
                    ResetSubscriptions = SubscriptionsList.ToArray();
                }
                else
                {
                    IncrementSubscriptions = SubscriptionsList.ToArray();
                }

                SubscriptionsUpdated();
            }
        }

        internal void UpdateProperties(CounterProperties theNewProperties)
        {
            bool SuUpdated = false;
            bool EvUpdated = false;

            if (theNewProperties.Guid != null && theNewProperties.Guid != GoalEvent.Guid)
                return;

            if (theNewProperties.GoalEvent != null)
            {
                if (Event.Merge(GoalEvent, theNewProperties.GoalEvent))
                {
                    EvUpdated = true;
                    UpdateCount();
                }
            }

            if (theNewProperties.Enabled != null && Enabled != theNewProperties.Enabled)
            {
                Enabled = theNewProperties.Enabled;
            }

            if (theNewProperties.AutoReset != null && AutoReset != theNewProperties.AutoReset)
            {
                AutoReset = theNewProperties.AutoReset;
            }

            if (theNewProperties.Goal != null && Goal != theNewProperties.Goal)
            {
                Goal = theNewProperties.Goal;

                if(Goal >= Current)
                {
                    Current = 0;
                }

                UpdateCount();
            }

            if (theNewProperties.IncrementSubscriptions != null)
            {
                var IncEDeleted = IncrementSubscriptions.Where( e => theNewProperties.IncrementSubscriptions.FirstOrDefault( ne => ne.Guid == e.Guid) == null);
                if(IncEDeleted.Count() > 0){ SuUpdated = true; }
                IncrementSubscriptions = IncrementSubscriptions.Except(IncEDeleted).ToArray();

                var StENew = theNewProperties.IncrementSubscriptions.Where(e => IncrementSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (StENew.Count() > 0) { SuUpdated = true; }
                foreach( var item in StENew ){ item.Guid = System.Guid.NewGuid().ToString(); item.Event += OnIncrementEvent; }
                IncrementSubscriptions = IncrementSubscriptions.Concat(StENew).ToArray();
            }

            if ( theNewProperties.ResetSubscriptions != null)
            {
                var ReEDeleted = ResetSubscriptions.Where(e => theNewProperties.ResetSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (ReEDeleted.Count() > 0) { SuUpdated = true; }
                ResetSubscriptions = ResetSubscriptions.Except(ReEDeleted).ToArray();

                var SpENew = theNewProperties.ResetSubscriptions.Where(e => ResetSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (SpENew.Count() > 0) { SuUpdated = true; }
                foreach (var item in SpENew){ item.Guid = System.Guid.NewGuid().ToString(); item.Event += OnResetEvent; }
                ResetSubscriptions = ResetSubscriptions.Concat(SpENew).ToArray();
            }


            if(EvUpdated && EventsUpdated != null)
            {
                EventsUpdated();
            }
            
            if(SuUpdated && SubscriptionsUpdated != null)
            {
                SubscriptionsUpdated();
            }
        }

        private void OnResetEvent(SubscriptionEvent obj)
        {
            if (!Enabled.Value)
                return;

            Current = 0;

            UpdateCount();
        }

        private void OnIncrementEvent(SubscriptionEvent obj)
        {
            if (!Enabled.Value)
                return;

            Current++;

            UpdateCount();

            if (Current == Goal)
            {
                if (AutoReset.Value)
                {
                    Current = 0;
                }
                else
                {
                    Enabled = false;
                }


                GoalEvent.Invoke(m_Count);
            }
        }

        private void UpdateCount()
        {
            m_Count = new Payload
            (
                GoalEvent.Id,
                new PayloadSubject[]
                {
                    new PayloadSubject( "count", Current.ToString() ),
                    new PayloadSubject( "goal", Goal.ToString() )
                }
            );
        }

        public Payload CachedValue()
        {
            return m_Count;
        }
    }
}
