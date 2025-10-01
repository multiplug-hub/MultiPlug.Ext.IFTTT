using System.Collections.Generic;
using System.Runtime.Serialization;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Models.Components.Injector
{
    public class InjectorProperties : Base.MultiPlugBase
    {
        public string Guid { get { return Event.Guid; } }
        [DataMember]
        public Event Event { get; set; }
        [DataMember]
        public Subscription[] Subscriptions { get; set; }

        [DataMember]
        public PayloadSubject[] Subjects { get; set; }
    }
}
