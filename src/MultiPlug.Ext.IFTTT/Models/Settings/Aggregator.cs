using MultiPlug.Base;
using MultiPlug.Ext.IFTTT.Components.Aggregator;

namespace MultiPlug.Ext.IFTTT.Models.Settings
{
    public class Aggregator
    {
        public string Guid { get; set; }
        public string EventId { get; set; }
        public string EventDescription { get; set; }
        public string[] AggregateSubscriptionGuid { get; set; }
        public string[] AggregateSubscriptionId { get; set; }
        public bool[] AggregateSubscriptionConnected { get; internal set; }
        public string[] ResetSubscriptionGuid { get; set; }
        public string[] ResetSubscriptionId { get; set; }
        public bool[] ResetSubscriptionConnected { get; internal set; }
    }
}
