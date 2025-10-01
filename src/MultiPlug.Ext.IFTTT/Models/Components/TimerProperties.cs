using System.Runtime.Serialization;
using MultiPlug.Base;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Models.Exchange;

namespace MultiPlug.Ext.IFTTT.Models.Components
{
    public class TimerProperties : MultiPlugBase
    {
        public string Guid { get { return ElapsedEvent.Guid; } }
        [DataMember]
        public bool? Enabled { get; set; }
        [DataMember]
        public bool? AutoReset { get; set; }
        [DataMember]
        public bool? StartOnSystemStart { get; set; }
        [DataMember]
        public double? Interval { get; set; }
        [DataMember]
        public TimerEvent ElapsedEvent { get; set; }
        [DataMember]
        public Subscription[] StartSubscriptions { get; set; }
        [DataMember]
        public Subscription[] StopSubscriptions { get; set; }
    }
}
