namespace MultiPlug.Ext.IFTTT.Models.Settings
{
    public class Cache
    {
        public string[] CurrentValues { get; internal set; }
        public string EventDescription { get; internal set; }
        public string EventId { get; internal set; }
        public string[] EventSubjects { get; internal set; }
        public string Guid { get; internal set; }
        public string InvokeEventOnSubjectValue { get; internal set; }
        public bool[] RequestSubscriptionConnected { get; internal set; }
        public string[] RequestSubscriptionGuid { get; internal set; }
        public string[] RequestSubscriptionId { get; internal set; }
        public bool[] SubscriptionConnected { get; internal set; }
        public string[] SubscriptionGuid { get; internal set; }
        public string[] SubscriptionId { get; internal set; }
    }
}
