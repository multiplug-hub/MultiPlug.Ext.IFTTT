using System;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Models.Exchange
{
    public class AggregatorSubscription : Subscription
    {
        private static object m_Lock = new object();

        internal event Action PayloadStored;

        internal Payload PayloadSubjects { get; private set; } = new Payload(string.Empty, new PayloadSubject[0]);

        internal bool Stored { get; private set; }

        internal void OnEvent(SubscriptionEvent obj)
        {
            PayloadStored?.Invoke();
        }

        internal void Reset()
        {
            lock (m_Lock)
            {
                PayloadSubjects = new Payload(string.Empty, new PayloadSubject[0]);
                Stored = false;
            }
        }
    }
}
