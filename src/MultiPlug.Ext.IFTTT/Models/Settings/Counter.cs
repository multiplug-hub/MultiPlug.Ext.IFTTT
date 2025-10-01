namespace MultiPlug.Ext.IFTTT.Models.Settings
{
    public class Counter
    {
        public bool AutoReset { get; internal set; }
        public bool Enabled { get; internal set; }
        public int Goal { get; internal set; }
        public string GoalEventDescription { get; internal set; }
        public string GoalEventId { get; internal set; }
        public string Guid { get; internal set; }
        public bool[] IncrementSubscriptionConnected { get; internal set; }
        public string[] IncrementSubscriptionGuid { get; internal set; }
        public string[] IncrementSubscriptionId { get; internal set; }
        public bool[] ResetSubscriptionConnected { get; internal set; }
        public string[] ResetSubscriptionGuid { get; internal set; }
        public string[] ResetSubscriptionId { get; internal set; }
    }
}
