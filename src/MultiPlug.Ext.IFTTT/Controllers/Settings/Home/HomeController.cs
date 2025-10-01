using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Home
{
    [Route("")]
    public class HomeController : SettingsApp
    {
        public Response Get()
        {
            return new Response
            {
                Model = new Models.Settings.Home
                {
                    TimerCount = Core.Instance.Timers.Length.ToString(),
                    CacheCount = Core.Instance.Caches.Length.ToString(),
                    CounterCount = Core.Instance.Counters.Length.ToString(),
                    AggregatorCount = Core.Instance.Aggregators.Length.ToString(),
                    InjectorCount = Core.Instance.Injectors.Length.ToString()
                },
                Template = "GetHomeViewContents"
            };
        }
    }
}
