using System.Runtime.Serialization;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Models.Exchange
{
    public class TimerEvent : Event
    {
        [DataMember]
        public string ElapsedValue { get; set; }
        [DataMember]
        public bool AddIntervalSubject { get; set; }
        [DataMember]
        public bool AddDataTimeSubject { get; set; }
    }
}
