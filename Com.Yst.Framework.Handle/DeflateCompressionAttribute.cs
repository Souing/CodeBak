
using System.Net.Http;
using System.Web.Http.Filters;

namespace Com.Yst.Framework.Handle
{
    /// <summary>
    /// webapi压缩
    /// 用法
    /// [DeflateCompression]
    /// public string Get(int id)
    /// {
    ///     return "ok"+id;
    /// }
    /// </summary>
    public class DeflateCompressionAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuted(HttpActionExecutedContext actContext)
        {
            var content = actContext.Response.Content;
            var bytes = content == null ? null : content.ReadAsByteArrayAsync().Result;
            var zlibbedContent = bytes == null ? new byte[0] :
            bytes = CompressionHelper.GZipByte(bytes);
            actContext.Response.Content = new ByteArrayContent(zlibbedContent);
            actContext.Response.Content.Headers.Remove("Content-Type");
            actContext.Response.Content.Headers.Add("Content-encoding", "gzip");
            actContext.Response.Content.Headers.Add("Content-Type", "application/json");
            base.OnActionExecuted(actContext);
        }
    }


}
