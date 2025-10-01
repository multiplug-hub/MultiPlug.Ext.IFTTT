namespace MultiPlug.Ext.IFTTT.Models.Settings
{
    public class Timer
    {
        public bool AutoReset { get; internal set; }
        public bool Enabled { get; internal set; }
        public string EventDescription { get; internal set; }
        public string EventId { get; internal set; }
        public string EventSubject { get; internal set; }
        public bool EventSubjectInterval { get; internal set; }
        public bool EventSubjectTime { get; internal set; }
        public string EventSubjectValue { get; internal set; }
        public string Guid { get; internal set; }
        public double Interval { get; internal set; }
        public bool StartOnSystemStart { get; internal set; }
        public bool[] StartSubscriptionConnected { get; internal set; }
        public string[] StartSubscriptionGuid { get; set; }
        public string[] StartSubscriptionId { get; set; }
        public bool[] StopSubscriptionConnected { get; internal set; }
        public string[] StopSubscriptionGuid { get; set; }
        public string[] StopSubscriptionId { get; set; }
    }
}
