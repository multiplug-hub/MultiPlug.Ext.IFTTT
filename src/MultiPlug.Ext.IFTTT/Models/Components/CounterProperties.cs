using System.Runtime.Serialization;
using MultiPlug.Base;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Models.Components
{
    public class CounterProperties : MultiPlugBase
    {
        public string Guid { get { return GoalEvent.Guid; } }
        [DataMember]
        public bool? Enabled { get; set; }
        [DataMember]
        public bool? AutoReset { get; set; }
        [DataMember]
        public Event GoalEvent { get; set; }
        [DataMember]
        public int? Goal { get; set; }
        [DataMember]
        public Subscription[] IncrementSubscriptions { get; set; }
        [DataMember]
        public Subscription[] ResetSubscriptions { get; set; }
    }
}
