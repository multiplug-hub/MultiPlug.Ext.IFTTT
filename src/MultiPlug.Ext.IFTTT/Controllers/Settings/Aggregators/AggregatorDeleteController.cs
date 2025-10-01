using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Aggregators
{
    [Route("aggregators/aggregator/delete")]
    public class AggregatorDeleteController : SettingsApp
    {
        public Response Post(string id)
        {
            Core.Instance.Remove(Core.Instance.Aggregators.FirstOrDefault(a => a.Guid == id));

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Location = Context.Referrer
            };
        }
    }
}
