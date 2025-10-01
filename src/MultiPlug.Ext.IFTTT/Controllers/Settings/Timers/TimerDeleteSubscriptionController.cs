using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Timers
{
    [Route("timers/timer/subscriptiondelete")]
    public class TimerDeleteSubscriptionController : SettingsApp
    {
        public Response Post(string id, string subtype, string subid)
        {
            var Timer = Core.Instance.Timers.FirstOrDefault(t => t.ElapsedEvent.Guid == id);

            if(Timer != null)
            {
                Timer.Remove(new Subscription(subid), subtype == "stop");
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Location = Context.Referrer
            };
        }
    }
}
