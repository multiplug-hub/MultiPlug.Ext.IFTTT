using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Counter
{
    [Route("counters/counter/delete")]
    public class CounterDeleteController : SettingsApp
    {
        public Response Post(string id)
        {
            Core.Instance.Remove(Core.Instance.Counters.FirstOrDefault(t => t.GoalEvent.Guid == id));

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Location = Context.Referrer
            };
        }
    }
}
