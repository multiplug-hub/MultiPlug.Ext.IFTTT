using System.Linq;
using MultiPlug.Base.Http;
using MultiPlug.Base.Attribute;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Injectors
{
    [Route("injectors/injector/delete")]
    public class InjectorDeleteController : SettingsApp
    {
        public Response Post(string id)
        {
            Core.Instance.Remove(Core.Instance.Injectors.FirstOrDefault(a => a.Guid == id));

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Location = Context.Referrer
            };
        }
    }
}
