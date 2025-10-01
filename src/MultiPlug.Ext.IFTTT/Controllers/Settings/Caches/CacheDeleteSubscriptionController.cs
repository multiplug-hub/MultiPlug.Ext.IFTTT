using System.Linq;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Base.Exchange;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Caches
{
    [Route("caches/cache/subscriptiondelete")]
    public class CacheDeleteSubscriptionController : SettingsApp
    {
        public Response Post(string id, string subtype, string subid)
        {
            var Cache = Core.Instance.Caches.FirstOrDefault(t => t.Guid == id);

            if (Cache != null)
            {
                if(Cache.Remove(new Subscription(subid), subtype == "request") )
                {
                    return new Response
                    {
                        StatusCode = System.Net.HttpStatusCode.OK
                    };
                }
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
    }
}
