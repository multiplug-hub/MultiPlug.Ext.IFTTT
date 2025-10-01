using System.Runtime.Serialization;
using MultiPlug.Base;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Models.Exchange;

namespace MultiPlug.Ext.IFTTT.Models.Components.Aggregator
{
    public class AggregatorProperties : MultiPlugBase
    {
        public string Guid { get { return Event.Guid; } }
        [DataMember]
        public Event Event { get; set; }
        [DataMember]
        public AggregatorSubscription[] AggregatedSubscriptions { get; set; }
        [DataMember]
        public Subscription[] ResetSubscriptions { get; set; }
    }
}
