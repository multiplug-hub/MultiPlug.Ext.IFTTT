using System;
using System.Linq;
using System.Collections.Generic;
using MultiPlug.Ext.IFTTT.Models.Components;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Components.Cache
{
    public class CacheComponent : CacheProperties
    {
        public event Action SubscriptionsUpdated;
        public event Action EventUpdated;

        private Payload m_Cache = new Payload();

        public CacheComponent(string theGuid)
        {
            ResponseEvent = new Event(theGuid, "IFTTT.Cache." + System.Guid.NewGuid().ToString().Substring(9, 4), "Cache", "Caches");
            Subscriptions = new Subscription[0];
            RequestSubscriptions = new Subscription[0];
            InvokeEventOnSubjectValue = string.Empty;
        }

        internal bool Remove(Subscription theSubscription, bool isRequestSubscription)
        {
            Subscription[] SubscriptionsArray;

            if (isRequestSubscription)
            {
                SubscriptionsArray = RequestSubscriptions;
            }
            else
            {
                SubscriptionsArray = Subscriptions;
            }

            var Search = SubscriptionsArray.FirstOrDefault(Subscription => Subscription.Guid == theSubscription.Guid);

            if (Search != null)
            {
                var SubscriptionsList = SubscriptionsArray.ToList();
                SubscriptionsList.Remove(Search);

                if (isRequestSubscription)
                {
                    RequestSubscriptions = SubscriptionsList.ToArray();
                }
                else
                {
                    Subscriptions = SubscriptionsList.ToArray();
                }

                SubscriptionsUpdated();

                return true;
            }

            return false;
        }

        internal void UpdateProperties(CacheProperties theNewProperties)
        {
            if( theNewProperties.Guid != Guid)
            {
                return;
            }

            bool SuUpdated = false;
            bool EventUpdate = false;

            if(theNewProperties.InvokeEventOnSubjectValue != null && theNewProperties.InvokeEventOnSubjectValue != InvokeEventOnSubjectValue)
            {
                InvokeEventOnSubjectValue = theNewProperties.InvokeEventOnSubjectValue;
            }

            if (theNewProperties.ResponseEvent != null)
            {
                if( Event.Merge(ResponseEvent, theNewProperties.ResponseEvent) )
                {
                    EventUpdate = true;
                }
            }

            if(theNewProperties.Subscriptions != null)
            {
                var New = theNewProperties.Subscriptions.Where(e => Subscriptions.FirstOrDefault(ne => ne.Guid == e.Guid) == null);
                if (New.Count() > 0) { SuUpdated = true; }
                foreach (var item in New) { item.Guid = System.Guid.NewGuid().ToString(); item.Event += OnEvent; }
                Subscriptions = Subscriptions.Concat(New).ToArray();
            }

            if(theNewProperties.RequestSubscriptions != null)
            {
                var list = RequestSubscriptions.ToList();

                foreach( var item in theNewProperties.RequestSubscriptions )
                {
                    if( string.IsNullOrEmpty(item.Guid))
                    {
                        item.Guid = System.Guid.NewGuid().ToString();
                        item.Event += OnRequest;
                        list.Add(item);
                        SuUpdated = true;
                    }
                    else
                    {
                        var Search = RequestSubscriptions.FirstOrDefault(s => s.Guid == item.Guid);

                        if(Search == null)
                        {
                            item.Event += OnRequest;
                            list.Add(item);
                            SuUpdated = true;
                        }
                        else
                        {
                            if( Subscription.Merge(Search, item))
                            {
                                SuUpdated = true;
                            }
                        }
                    }
                }

                RequestSubscriptions = list.ToArray();
            }

            if (SuUpdated)
            {
                SubscriptionsUpdated?.Invoke();
            }

            if(EventUpdate)
            {
                EventUpdated?.Invoke();
            }
        }

        private void OnRequest(SubscriptionEvent obj)
        {
            if(string.IsNullOrEmpty(InvokeEventOnSubjectValue))
            {
                ResponseEvent.Invoke(Cache);
            }
            else
            {
                foreach (var item in obj.PayloadSubjects)
                {
                    if(item.Value == InvokeEventOnSubjectValue)
                    {
                        ResponseEvent.Invoke(Cache);
                        break;
                    }
                }
            }
        }

        public Payload Cache { get { return m_Cache; } }

        public void OnEvent(SubscriptionEvent theEvents)
        {
            List<PayloadSubject> Subjects = new List<PayloadSubject>();

            for(int i = 0; i < ResponseEvent.Subjects.Length; i++)
            {
                if( i < theEvents.PayloadSubjects.Length)
                {
                    Subjects.Add(new PayloadSubject(string.Copy(ResponseEvent.Subjects[i]), string.Copy(theEvents.PayloadSubjects[i].Value)));
                }
            }

            m_Cache = new Payload( string.Empty, Subjects.ToArray() );
        }
    }
}
