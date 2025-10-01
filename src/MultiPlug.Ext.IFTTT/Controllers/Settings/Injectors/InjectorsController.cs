using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;

namespace MultiPlug.Ext.IFTTT.Controllers.Settings.Injectors
{
    [Route("injectors")]
    public class InjectorsController : SettingsApp
    {
        public Response Get()
        {
            return new Response
            {
                Model = new Models.Settings.InjectorsModel { InjectorComponents = Core.Instance.Injectors },
                Template = "GetInjectorsViewContents"
            };
        }
    }
}
