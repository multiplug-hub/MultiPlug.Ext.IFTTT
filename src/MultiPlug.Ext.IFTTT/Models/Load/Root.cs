using System.Runtime.Serialization;
using MultiPlug.Ext.IFTTT.Models.Components;
using MultiPlug.Ext.IFTTT.Models.Components.Aggregator;
using MultiPlug.Ext.IFTTT.Models.Components.Injector;

namespace MultiPlug.Ext.IFTTT.Models.Load
{
    public class Root
    {
        [DataMember]
        public CacheProperties[] Caches { get; set; }
        [DataMember]
        public CounterProperties[] Counters { get; set; }
        [DataMember]
        public TimerProperties[] Timers { get; set; }
        [DataMember]
        public AggregatorProperties[] Aggregators { get; set; }
        [DataMember]
        public InjectorProperties[] Injectors { get; set; }
    }
}
