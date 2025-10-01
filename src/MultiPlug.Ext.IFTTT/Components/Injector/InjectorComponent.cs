using System;
using System.Collections.Generic;
using System.Linq;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Models.Components.Injector;

namespace MultiPlug.Ext.IFTTT.Components.Injector
{
    public class InjectorComponent : InjectorProperties
    {
        public event Action EventsUpdated;
        public event Action SubscriptionsUpdated;

        public InjectorComponent(string theGuid)
        {
            Subscriptions = new Subscription[0];
            Event = new Event { Guid = theGuid, Id = "IFTTT.Injector." + System.Guid.NewGuid().ToString().Substring(9, 4), Description = "Data Injector", Group = "Injectors" };
            Subjects = new PayloadSubject[0];
        }

        internal void UpdateProperties(InjectorProperties theNewProperties)
        {
            bool SuUpdated = false;
            bool EvUpdated = false;

            if (theNewProperties.Event.Guid != null && theNewProperties.Event.Guid != Event.Guid)
                return;

            if (theNewProperties.Event != null)
            {
                if (Event.Merge(Event, theNewProperties.Event, false))
                {
                    EvUpdated = true;
                }
            }

            if(theNewProperties.Subjects != null)
            {
                Subjects = theNewProperties.Subjects;
            }

            if (theNewProperties.Subscriptions != null)
            {
                var SubsDeleted = Subscriptions.Where(e => theNewProperties.Subscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (SubsDeleted.Count() > 0) { SuUpdated = true; }
                Subscriptions = Subscriptions.Except(SubsDeleted).ToArray();

                var SubsNew = theNewProperties.Subscriptions.Where(e => Subscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (SubsNew.Count() > 0) { SuUpdated = true; }
                foreach (var item in SubsNew) { item.Guid = System.Guid.NewGuid().ToString(); item.Event += OnEvent; }
                Subscriptions = Subscriptions.Concat(SubsNew).ToArray();
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

        internal void Remove(Subscription theSubscription)
        {
            var Search = Subscriptions.FirstOrDefault(Subscription => Subscription.Guid == theSubscription.Guid);

            if (Search != null)
            {
                var SubscriptionsList = Subscriptions.ToList();
                SubscriptionsList.Remove(Search);
                Subscriptions = SubscriptionsList.ToArray();
                SubscriptionsUpdated();
            }
        }

        private void OnEvent(SubscriptionEvent e)
        {
            var Pairs = new List<PayloadSubject>();
            Pairs.AddRange( e.PayloadSubjects.Select(Pair => new PayloadSubject(string.Copy(Pair.Subject), string.Copy(Pair.Value) ) ) );
            Pairs.AddRange(Subjects);
            Event.Invoke(new Payload( Event.Id, Pairs.ToArray() ) );
        }
    }
}
