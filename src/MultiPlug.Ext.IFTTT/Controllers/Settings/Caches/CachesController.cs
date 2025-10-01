using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Caches
{
    [Route("caches")]
    public class CachesController : SettingsApp
    {
        public Response Get()
        {
            var model = new Models.Settings.Caches();
            model.CacheComponents = Core.Instance.Caches;

            return new Response
            {
                Model = model,
                Template = "GetCachesViewContents"
            };
        }
    }
}
