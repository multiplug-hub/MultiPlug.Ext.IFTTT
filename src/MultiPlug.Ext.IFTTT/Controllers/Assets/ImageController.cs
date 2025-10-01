using System.Drawing;
using MultiPlug.Base.Attribute;
using MultiPlug.Base.Http;
using MultiPlug.Ext.IFTTT.Properties;

namespace MultiPlug.Ext.IFTTT.Controllers.Assets
{
    [Route("images/*")]
    public class ImageController : AssetsEndpoint
    {
        public Response Get(string theName)
        {
            ImageConverter converter = new ImageConverter();
            return new Response { RawBytes = (byte[])converter.ConvertTo(Resources.IFTTT, typeof(byte[])), MediaType = "image/png" };
        }
    }
}
