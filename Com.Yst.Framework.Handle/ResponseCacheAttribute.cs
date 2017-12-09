using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Com.Yst.Framework.Handle
{
    /// <summary>
    /// 功能描述：已经请求，如果请求相同则缓存响应结果
    /// 创 建 者：panzy  2015/6/2 10:33:02
    /// 审 查 者：        审查时间:       
    /// </summary>
    public class ResponseCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
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
