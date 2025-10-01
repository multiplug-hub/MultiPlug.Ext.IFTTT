using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Exchange;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Aggregators
{
    [Route("aggregators/aggregator/subscriptiondelete")]
    public class AggregatorDeleteSubscriptionController : SettingsApp
    {
        public Response Get(string id, string subtype, string subid)
        {
            var Aggregator = Core.Instance.Aggregators.FirstOrDefault(t => t.Guid == id);

            if (Aggregator != null)
            {
                Aggregator.Remove(new Subscription(subid), subtype == "stop");
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
