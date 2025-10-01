using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Timers
{
    [Route("timers")]
    public class TimersController : SettingsApp
    {
        public Response Get()
        {
            return new Response
            {
                Model = new Models.Settings.Timers { TimerComponents = Core.Instance.Timers },
                Template = "GetTimersViewContents"
            };
        }
    }
}
