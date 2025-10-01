using System.Linq;
using System.Text;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using Newtonsoft.Json;

namespace MultiPlug.Ext.IFTTT.Views.APIs
{
    [Route("cache/*")]
    public class CacheController : APIApp
    {
        public Response Get(string id)
        {
            var CC = Core.Instance.Caches.FirstOrDefault(c => c.Guid == id);

            if (CC != null)
            {
                string json = JsonConvert.SerializeObject(CC.Cache);
                return new Response { RawBytes = Encoding.ASCII.GetBytes(json), MediaType = "application/json" };
            }
            else
            {
                return new Response { StatusCode = System.Net.HttpStatusCode.Gone };
            }
        }
    }
}
