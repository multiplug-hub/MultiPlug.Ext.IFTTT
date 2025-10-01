using System.Runtime.Serialization;
using MultiPlug.Base;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Models.Components
{
    public class CacheProperties : MultiPlugBase
    {
        public string Guid
        {
            get
            {
                return ResponseEvent == null ? null : ResponseEvent.Guid; // Used for Legacy Versions.
            }
        }
        [DataMember]
        public Subscription[] Subscriptions { get; set; }
        [DataMember]
        public Event ResponseEvent { get; set; }
        [DataMember]
        public Subscription[] RequestSubscriptions { get; set; }
        [DataMember]
        public string InvokeEventOnSubjectValue { get; set; }
    }
}
