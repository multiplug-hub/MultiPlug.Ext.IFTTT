using System.Linq;
using System.Text;
using MultiPlug.Base.Http;
using MultiPlug.Base.Attribute;
using Newtonsoft.Json;

namespace MultiPlug.Ext.IFTTT.Views.APIs
{
    [Route("counter/*")]
    public class CounterController : APIApp
    {
        public Response Get(string id)
        {
            var Co = Core.Instance.Counters.FirstOrDefault(c => c.GoalEvent.Guid == id);

            if (Co != null)
            {
                string json = JsonConvert.SerializeObject(Co.CountValue);
                return new Response { RawBytes = Encoding.ASCII.GetBytes(json), MediaType = "application/json" };
            }
            else
            {
                return new Response { StatusCode = System.Net.HttpStatusCode.Gone };
            }
        }
    }
}
