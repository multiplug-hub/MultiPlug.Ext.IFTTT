using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Timers
{
    [Route("timers/timer/delete")]
    public class TimerDeleteController : SettingsApp
    {
        public Response Post(string id)
        {
            Core.Instance.Remove(Core.Instance.Timers.FirstOrDefault(t => t.ElapsedEvent.Guid == id));

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Location = Context.Referrer
            };
        }
    }
}
