using System;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Components.Aggregator
{
    public class AggregatorReset : EventConsumer
    {
        public event EventHandler Reset;

        public override void OnEvent(Payload thePayload)
        {
            Reset?.Invoke(this, EventArgs.Empty);
        }
    }
}
