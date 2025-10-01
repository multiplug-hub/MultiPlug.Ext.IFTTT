using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Caches
{
    [Route("caches/cache/delete")]
    public class CacheDeleteController : SettingsApp
    {
        public Response Post(string id)
        {
            Core.Instance.Remove(Core.Instance.Caches.FirstOrDefault(t => t.Guid == id));

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.Redirect,
                Location = Context.Referrer
            };
        }
    }
}