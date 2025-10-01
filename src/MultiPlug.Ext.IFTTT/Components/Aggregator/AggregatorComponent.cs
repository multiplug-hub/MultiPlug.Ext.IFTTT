using System;
using System.Linq;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Models.Components.Aggregator;
using MultiPlug.Ext.IFTTT.Models.Exchange;

namespace MultiPlug.Ext.IFTTT.Components.Aggregator
{
    public class AggregatorComponent : AggregatorProperties
    {
        private static object m_Lock = new object();

        public event Action EventsUpdated;
        public event Action SubscriptionsUpdated;

        public AggregatorComponent(string theGuid)
        {
            AggregatedSubscriptions = new AggregatorSubscription[0];
            ResetSubscriptions = new Subscription[0];
            Event = new Event { Guid = theGuid, Id = "IFTTT.Aggregator." + System.Guid.NewGuid().ToString().Substring(9, 4), Description = "Aggregator", Group = "Aggregators" };
        }

        internal void Remove(Subscription subscription, bool v)
        {
            throw new NotImplementedException();
        }

        internal void UpdateProperties(AggregatorProperties theNewProperties)
        {
            bool SuUpdated = false;
            bool EvUpdated = false;

            if (theNewProperties.Event.Guid != Event.Guid)
                return;

            if (theNewProperties.Event.Description != Event.Description)
            {
                Event.Description = theNewProperties.Event.Description;
            }
            if (theNewProperties.Event.Id != Event.Id)
            {
                Event.Id = theNewProperties.Event.Id;
                EvUpdated = true;
            }

            if (theNewProperties.AggregatedSubscriptions != null)
            {
                var EDeleted = AggregatedSubscriptions.Where(e => theNewProperties.AggregatedSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (EDeleted.Count() > 0) { SuUpdated = true; }
                AggregatedSubscriptions = AggregatedSubscriptions.Except(EDeleted).ToArray();

                var ENew = theNewProperties.AggregatedSubscriptions.Where(e => AggregatedSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (ENew.Count() > 0) { SuUpdated = true; }
                foreach (var item in ENew) { item.Guid = System.Guid.NewGuid().ToString(); item.Event += item.OnEvent; item.PayloadStored += OnPayloadStored; }
                AggregatedSubscriptions = AggregatedSubscriptions.Concat(ENew).ToArray();
            }

            if (theNewProperties.ResetSubscriptions != null)
            {
                var EDeleted = ResetSubscriptions.Where(e => theNewProperties.ResetSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (EDeleted.Count() > 0) { SuUpdated = true; }
                ResetSubscriptions = ResetSubscriptions.Except(EDeleted).ToArray();

                var ENew = theNewProperties.ResetSubscriptions.Where(e => ResetSubscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (ENew.Count() > 0) { SuUpdated = true; }
                foreach (var item in ENew) { item.Guid = System.Guid.NewGuid().ToString(); item.Event += OnReset; }
                ResetSubscriptions = ResetSubscriptions.Concat(ENew).ToArray();
            }

            if (EvUpdated && EventsUpdated != null)
            {
                EventsUpdated();
            }

            if (SuUpdated && SubscriptionsUpdated != null)
            {
                SubscriptionsUpdated();
            }
        }

        private void OnPayloadStored()
        {
            lock (m_Lock)
            {
                if (AggregatedSubscriptions.All(s => s.Stored))
                {
                    PayloadSubject[] Subjects = AggregatedSubscriptions.SelectMany(s => s.PayloadSubjects.Subjects).ToArray();

                    Array.ForEach(AggregatedSubscriptions, s => s.Reset());

                    Event.Invoke(new Payload
                    (
                        Event.Id,
                        Subjects
                    ));
                }
            }
        }

        private void OnReset(SubscriptionEvent obj)
        {
            lock (m_Lock)
            {
                Array.ForEach(AggregatedSubscriptions, s => s.Reset());
            }
        }
    }
}
