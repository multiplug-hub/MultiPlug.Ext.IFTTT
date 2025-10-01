using System.Collections.Generic;
using MultiPlug.Base.Exchange;
using MultiPlug.Extension.Core;
using MultiPlug.Extension.Core.Http;
using MultiPlug.Ext.IFTTT.Properties;
using MultiPlug.Ext.IFTTT.Models.Load;
using MultiPlug.Ext.IFTTT.Models.Components;
using MultiPlug.Ext.IFTTT.Models.Components.Aggregator;
using MultiPlug.Ext.IFTTT.Models.Components.Injector;

namespace MultiPlug.Ext.IFTTT
{
    public class IFTTT : MultiPlugExtension
    {
        private bool Initialised = false;

        private List<Root> m_LoadQ = new List<Root>();

        public IFTTT()
        {
            Core.Instance.EventsUpdated += () => { MultiPlugActions.Extension.Updates.Events(); };
            Core.Instance.SubscriptionsUpdated += () => { MultiPlugActions.Extension.Updates.Subscriptions(); };
        }

        public override Event[] Events
        {
            get
            {
                return Core.Instance.Events;
            }
        }

        public override Subscription[] Subscriptions
        {
            get
            {
                return Core.Instance.Subscriptions;
            }
        }

        public override RazorTemplate[] RazorTemplates
        {
            get
            {
                return new RazorTemplate[]
                {
                    new RazorTemplate("IFTTTNavigation", Resources.Navigation),
                    new RazorTemplate("IFTTTAbout", Resources.About),
                    new RazorTemplate("GetAggregatorViewContents", Resources.Aggregator),
                    new RazorTemplate("GetAggregatorsViewContents", Resources.Aggregators),
                    new RazorTemplate("GetCacheViewContents", Resources.Cache),
                    new RazorTemplate("GetCachesViewContents", Resources.Caches),
                    new RazorTemplate("GetCounterViewContents", Resources.Counter),
                    new RazorTemplate("GetCountersViewContents", Resources.Counters),
                    new RazorTemplate("GetHomeViewContents", Resources.Home),
                    new RazorTemplate("GetInjectorViewContents", Resources.Injector),
                    new RazorTemplate("GetInjectorsViewContents", Resources.Injectors),
                    new RazorTemplate("GetTimerViewContents", Resources.Timer),
                    new RazorTemplate("GetTimersViewContents", Resources.Timers),
                };
            }
        }

        public override void Initialise()
        {
            m_LoadQ.ForEach(LoadObject => {
                Core.Instance.Update(
                    LoadObject.Timers != null ? LoadObject.Timers : new TimerProperties[0],
                    LoadObject.Caches != null ? LoadObject.Caches : new CacheProperties[0],
                    LoadObject.Counters != null ? LoadObject.Counters : new CounterProperties[0],
                    LoadObject.Aggregators != null ? LoadObject.Aggregators : new AggregatorProperties[0],
                    LoadObject.Injectors != null ? LoadObject.Injectors : new InjectorProperties[0]);
            });

            if( m_LoadQ.Count > 0)
            {
                Initialised = true;
            }

            m_LoadQ.Clear();
        }

        public void Load(Root config)
        {
            m_LoadQ.Add(config);
        }

        public override object Save()
        {
            return Core.Instance;
        }

        public override void Start()
        {
            if(Initialised) // Don't call start until a Recipe has been loaded and Initialised
            {
                Core.Instance.OnSystemStarted();
            }
        }
    }
}
