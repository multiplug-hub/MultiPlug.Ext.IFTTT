using System;
using System.Linq;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Components.Aggregator
{
    public class AggregatorStore : EventConsumer
    {
        private static object m_Lock = new object();

        public event EventHandler DataStored;

        public Payload EventData { get; private set; } = new Payload( string.Empty, new PayloadSubject[0] );

        public bool Stored { get; private set; }

        public void Reset()
        {
            lock(m_Lock)
            {
                EventData = new Payload( string.Empty, new PayloadSubject[0] );
                Stored = false;
            }
        }

        public override void OnEvent(Payload thePayload)
        {
            lock (m_Lock)
            {
                if (!Stored)
                {
                    EventData = new Payload
                    (
                        string.Copy(thePayload.Id),
                        thePayload.Subjects.Select(p => new PayloadSubject( string.Copy(p.Subject), string.Copy(p.Value) ) ).ToArray()
                    );
                    Stored = true;

                    DataStored?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
