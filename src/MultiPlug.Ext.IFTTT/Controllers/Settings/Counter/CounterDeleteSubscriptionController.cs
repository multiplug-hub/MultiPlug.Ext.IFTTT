using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Timers
{
    [Route("counters/counter/subscriptiondelete")]
    public class CounterDeleteSubscriptionController : SettingsApp
    {
        public Response Post(string id, string subtype, string subid)
        {
            var Counter = Core.Instance.Counters.FirstOrDefault(t => t.Guid == id);

            if (Counter != null)
            {
                Counter.Remove(new Subscription(subid), subtype == "reset");
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Location = Context.Referrer
            };
        }
    }
}
