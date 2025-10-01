using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Exchange;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Injectors
{
    [Route("injectors/injector/subscriptiondelete")]
    public class InjectorDeleteSubscriptionController : SettingsApp
    {
        public Response Post(string id, string subid)
        {
            var Injector = Core.Instance.Injectors.FirstOrDefault(t => t.Guid == id);

            if (Injector != null)
            {
                Injector.Remove(new Subscription(subid));
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
