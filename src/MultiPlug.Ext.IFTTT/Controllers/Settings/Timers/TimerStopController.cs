using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Timers
{
    [Route("timers/timer/stop")]
    public class TimerStopController : SettingsApp
    {
        public Response Post(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var Timer = Core.Instance.Timers.FirstOrDefault(t => t.Guid == id);

                if (Timer != null)
                {
                    Timer.Stop();
                }
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
