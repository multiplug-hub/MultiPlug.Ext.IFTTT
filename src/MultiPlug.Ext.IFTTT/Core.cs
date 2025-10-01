using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MultiPlug.Base;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.IFTTT.Models.Components;
using MultiPlug.Ext.IFTTT.Components.Timer;
using MultiPlug.Ext.IFTTT.Components.Cache;
using MultiPlug.Ext.IFTTT.Components.Counter;
using MultiPlug.Ext.IFTTT.Components.Aggregator;
using MultiPlug.Ext.IFTTT.Components.Injector;

namespace MultiPlug.Ext.IFTTT
{
    public class Core : MultiPlugBase
    {
        private static Core m_Instance = null;

        internal event Action EventsUpdated;
        internal event Action SubscriptionsUpdated;

        internal Subscription[] Subscriptions { get; private set; } = new Subscription[0];
        internal Event[] Events { get; private set; } = new Event[0];

        public static Core Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Core();
                }
                return m_Instance;
            }
        }

        private Core()
        {
        }

        internal void OnSystemStarted()
        {
            Array.ForEach(Timers, Timer => Timer.OnSystemStarted());
        }

        internal void Update(TimerProperties[] timerData,
            CacheProperties[] cacheData,
            CounterProperties[] counterData,
            Models.Components.Aggregator.AggregatorProperties[] theAggregatorProperties,
            Models.Components.Injector.InjectorProperties[] theInjectorProperties)
        {
            UpdateTimer(timerData);
            UpdateCache(cacheData);
            UpdateCounter(counterData);
            UpdateAggregator(theAggregatorProperties);
            UpdateInjectors(theInjectorProperties);
        }

        internal void Update(CounterProperties counterData)
        {
            UpdateCounter(new[] { counterData });
        }

        internal void Update(CacheProperties cacheData)
        {
            UpdateCache(new[] { cacheData });
        }

        internal void Update(TimerProperties timerData)
        {
            UpdateTimer(new[] { timerData });
        }

        internal void Update(Models.Components.Aggregator.AggregatorProperties theAggregatorProperties)
        {
            UpdateAggregator(new[] { theAggregatorProperties });
        }

        internal void Update(Models.Components.Injector.InjectorProperties theProperties)
        {
            UpdateInjectors(new[] { theProperties });
        }

        private void UpdateTimer(TimerProperties[] timerData)
        {
            List<TimerComponent> NewTimers = new List<TimerComponent>();

            foreach (var item in timerData)
            {
                TimerComponent Timer = Timers.FirstOrDefault(t => t.ElapsedEvent.Guid == item.ElapsedEvent.Guid);

                if (Timer == null)
                {
                    Timer = new TimerComponent(item.Guid);
                    Timer.UpdateProperties(item);
                    Timer.SubscriptionsUpdated += OnSubscriptionsUpdated;
                    Timer.EventsUpdated += OnEventsUpdated;
                    NewTimers.Add(Timer);
                }
                else
                {
                    Timer.UpdateProperties(item);
                }
            }

            if (NewTimers.Any())
            {
                var TimersList = Timers.ToList();
                TimersList.AddRange(NewTimers);
                Timers = TimersList.ToArray();
                OnSubscriptionsUpdated();
                OnEventsUpdated();
            }
        }

        private void UpdateCache(CacheProperties[] cacheData)
        {
            List<CacheComponent> NewCacheComponents = new List<CacheComponent>();

            foreach(var item in cacheData)
            {
                CacheComponent Cache = null;
                    
                if(!string.IsNullOrEmpty(item.Guid))
                {
                    Cache = Caches.FirstOrDefault(c => c.Guid == item.Guid);
                }    
                     
                if (Cache == null)
                {
                    Cache = new CacheComponent(string.IsNullOrEmpty(item.Guid)? System.Guid.NewGuid().ToString(): item.Guid);
                    Cache.UpdateProperties(item);
                    Cache.SubscriptionsUpdated += OnSubscriptionsUpdated;
                    Cache.EventUpdated += OnEventsUpdated;
                    NewCacheComponents.Add(Cache);
                }
                else
                {
                    Cache.UpdateProperties(item);
                }
            }

            if (NewCacheComponents.Any())
            {
                var CacheList = Caches.ToList();
                CacheList.AddRange(NewCacheComponents);
                Caches = CacheList.ToArray();
                OnSubscriptionsUpdated();
                OnEventsUpdated();
            }
        }

        private void UpdateCounter(CounterProperties[] counterData)
        {
            List<CounterComponent> NewCounters = new List<CounterComponent>();

            foreach (var item in counterData)
            {
                CounterComponent Counter = Counters.FirstOrDefault(c => c.GoalEvent.Guid == item.GoalEvent.Guid);

                if (Counter == null)
                {
                    Counter = new CounterComponent(item.Guid);
                    Counter.UpdateProperties(item);
                    Counter.SubscriptionsUpdated += OnSubscriptionsUpdated;
                    Counter.EventsUpdated += OnEventsUpdated;
                    NewCounters.Add(Counter);
                }
                else
                {
                    Counter.UpdateProperties(item);
                }
            }

            if (NewCounters.Any())
            {
                var CounterList = Counters.ToList();
                CounterList.AddRange(NewCounters);
                Counters = CounterList.ToArray();
                OnSubscriptionsUpdated();
                OnEventsUpdated();
            }
        }

        private void UpdateAggregator(Models.Components.Aggregator.AggregatorProperties[] theAggregatorProperties)
        {
            List<AggregatorComponent> NewAggregators = new List<AggregatorComponent>();

            foreach (var item in theAggregatorProperties)
            {
                AggregatorComponent Aggregator = Aggregators.FirstOrDefault(t => t.Guid == item.Event.Guid);

                if (Aggregator == null)
                {
                    Aggregator = new AggregatorComponent(item.Guid);
                    Aggregator.UpdateProperties(item);
                    Aggregator.SubscriptionsUpdated += OnSubscriptionsUpdated;
                    Aggregator.EventsUpdated += OnEventsUpdated;
                    NewAggregators.Add(Aggregator);
                }
                else
                {
                    Aggregator.UpdateProperties(item);
                }
            }

            if (NewAggregators.Any())
            {
                var AggregatorList = Aggregators.ToList();
                AggregatorList.AddRange(NewAggregators);
                Aggregators = AggregatorList.ToArray();
                OnSubscriptionsUpdated();
                OnEventsUpdated();
            }
        }

        private void UpdateInjectors(Models.Components.Injector.InjectorProperties[] theInjectorProperties)
        {
            List<InjectorComponent> NewInjectors = new List<InjectorComponent>();

            foreach (var item in theInjectorProperties)
            {
                InjectorComponent Injector = Injectors.FirstOrDefault(t => t.Guid == item.Event.Guid);

                if (Injector == null)
                {
                    Injector = new InjectorComponent(item.Guid);
                    Injector.UpdateProperties(item);
                    Injector.SubscriptionsUpdated += OnSubscriptionsUpdated;
                    Injector.EventsUpdated += OnEventsUpdated;
                    NewInjectors.Add(Injector);
                }
                else
                {
                    Injector.UpdateProperties(item);
                }
            }

            if (NewInjectors.Any())
            {
                var InjectorList = Injectors.ToList();
                InjectorList.AddRange(NewInjectors);
                Injectors = InjectorList.ToArray();
                OnSubscriptionsUpdated();
                OnEventsUpdated();
            }
        }

        private void OnEventsUpdated()
        {

            var EventsList = new List<Event>();
            Array.ForEach(Timers, t => EventsList.Add(t.ElapsedEvent));
            Array.ForEach(Caches, c => EventsList.Add(c.ResponseEvent));
            Array.ForEach(Counters, c => EventsList.Add(c.GoalEvent));
            Array.ForEach(Aggregators, a => EventsList.Add(a.Event));
            Array.ForEach(Injectors, i => EventsList.Add(i.Event));
            Events = EventsList.ToArray();
            EventsUpdated?.Invoke();
        }

        private void OnSubscriptionsUpdated()
        {
            var SubscriptionList = new List<Subscription>();
            Array.ForEach(Timers, Timer => { SubscriptionList.AddRange(Timer.StartSubscriptions); SubscriptionList.AddRange(Timer.StopSubscriptions); });
            Array.ForEach(Caches, Cache => SubscriptionList.AddRange(Cache.Subscriptions));
            Array.ForEach(Caches, Cache => SubscriptionList.AddRange(Cache.RequestSubscriptions));
            Array.ForEach(Counters, Counter => { SubscriptionList.AddRange(Counter.IncrementSubscriptions); SubscriptionList.AddRange(Counter.ResetSubscriptions); });
            Array.ForEach(Aggregators, Aggregator => { SubscriptionList.AddRange(Aggregator.AggregatedSubscriptions); SubscriptionList.AddRange(Aggregator.ResetSubscriptions); });
            Array.ForEach(Injectors, Injector => { SubscriptionList.AddRange(Injector.Subscriptions); });
            Subscriptions = SubscriptionList.ToArray();
            SubscriptionsUpdated?.Invoke();
        }

        internal void Remove(TimerComponent theTimer)
        {
            if (theTimer == null)
            {
                return;
            }

            theTimer.Stop();
            theTimer.SubscriptionsUpdated -= OnSubscriptionsUpdated;
            theTimer.EventsUpdated -= OnEventsUpdated;

            var TimersList = Timers.ToList();
            TimersList.Remove(theTimer);

            Timers = TimersList.ToArray();
            OnSubscriptionsUpdated();
            OnEventsUpdated();
        }

        internal void Remove(CacheComponent theCache)
        {
            if (theCache == null)
            {
                return;
            }

            theCache.SubscriptionsUpdated -= OnSubscriptionsUpdated;
            theCache.EventUpdated -= OnEventsUpdated;

            var CachesList = Caches.ToList();
            CachesList.Remove(theCache);

            Caches = CachesList.ToArray();
            OnSubscriptionsUpdated();
            OnEventsUpdated();
        }

        internal void Remove(CounterComponent theCounter)
        {
            if(theCounter == null)
            {
                return;
            }

            theCounter.SubscriptionsUpdated -= OnSubscriptionsUpdated;
            theCounter.EventsUpdated -= OnEventsUpdated;

            var CountersList = Counters.ToList();
            CountersList.Remove(theCounter);

            Counters = CountersList.ToArray();
            OnSubscriptionsUpdated();
            OnEventsUpdated();
        }

        internal void Remove(InjectorComponent theInjector)
        {
            if (theInjector == null)
            {
                return;
            }

            theInjector.SubscriptionsUpdated -= OnSubscriptionsUpdated;
            theInjector.EventsUpdated -= OnEventsUpdated;

            var InjectorsList = Injectors.ToList();
            InjectorsList.Remove(theInjector);

            Injectors = InjectorsList.ToArray();
            OnSubscriptionsUpdated();
            OnEventsUpdated();
        }

        internal void Remove(AggregatorComponent theAggregator)
        {
            if (theAggregator == null)
            {
                return;
            }

            theAggregator.SubscriptionsUpdated -= OnSubscriptionsUpdated;
            theAggregator.EventsUpdated -= OnEventsUpdated;

            var AggregatorsList = Aggregators.ToList();
            AggregatorsList.Remove(theAggregator);

            Aggregators = AggregatorsList.ToArray();
            OnSubscriptionsUpdated();
            OnEventsUpdated();
        }

        [DataMember]
        public TimerComponent[] Timers { get; set; } = new TimerComponent[0];

        [DataMember]
        public CacheComponent[] Caches { get; set; } = new CacheComponent[0];

        [DataMember]
        public CounterComponent[] Counters { get; set; } = new CounterComponent[0];

        [DataMember]
        public AggregatorComponent[] Aggregators { get; set; } = new AggregatorComponent[0];

        [DataMember]
        public InjectorComponent[] Injectors { get; set; } = new InjectorComponent[0];

    }
}
