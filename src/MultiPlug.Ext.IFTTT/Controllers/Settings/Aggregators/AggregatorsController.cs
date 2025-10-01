using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Aggregators
{
    [Route("aggregators")]
    public class AggregatorsController : SettingsApp
    {
        public Response Get()
        {
            return new Response
            {
                Model = new Models.Settings.Aggregators { AggregatorComponents = Core.Instance.Aggregators },
                Template = "GetAggregatorsViewContents"
            };
        }
    }
}
