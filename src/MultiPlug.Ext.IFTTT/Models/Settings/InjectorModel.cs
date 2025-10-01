using MultiPlug.Base;

namespace MultiPlug.Ext.IFTTT.Models.Settings
{
    public class InjectorModel
    {
        public string Guid { get; internal set; }
        public string EventId { get; internal set; }
        public string EventDescription { get; internal set; }
        public string[] SubscriptionGuid { get; internal set; }
        public string[] SubscriptionId { get; internal set; }
        public string[] Subject { get; internal set; }
        public string[] Value { get; internal set; }
        public bool[] SubscriptionConnected { get; internal set; }
    }
}
