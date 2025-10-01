using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Counter
{
    [Route("counters")]
    public class CountersController : SettingsApp
    {
        public Response Get()
        {
            return new Response
            {
                Model = new Models.Settings.Counters { CounterComponents = Core.Instance.Counters },
                Template = "GetCountersViewContents"
            };
        }
    }
}
