using System.Reflection;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.IFTTT.Controllers.Settings;

namespace MultiPlug.Ext.Transform.Controllers.Settings.About
{
    [Route("about")]
    public class AboutController : SettingsApp
    {
        public Response Get()
        {
            Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();

            return new Response
            {
                Template = "IFTTTAbout",
                Model = new Models.Settings.About.About
                {
                    Title = ExecutingAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title,
                    Description = ExecutingAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description,
                    Company = ExecutingAssembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company,
                    Product = ExecutingAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product,
                    Copyright = ExecutingAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright,
                    Trademark = ExecutingAssembly.GetCustomAttribute<AssemblyTrademarkAttribute>().Trademark,
                    Version = ExecutingAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version,
                }
            };
        }
    }
}
