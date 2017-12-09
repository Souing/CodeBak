using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Helpers;

namespace Com.Yst.Framework.Handle
{
    /// <summary>
    /// 功能描述：CsrfHandler 防止CSRF 跨站脚本攻击
    /// 创 建 者：panzy  2015/8/3 17:23:46
    /// 审 查 者：        审查时间:       
    /// </summary>
    public class CsrfHandler : DelegatingHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post ||
                request.Method == HttpMethod.Put ||
                request.Method == HttpMethod.Delete)//这里csrf只针对这3种请求才会验证
            {
                ValidateRequestHeader(request);
            }

            return base.SendAsync(request, cancellationToken);
        }


        private void ValidateRequestHeader(HttpRequestMessage request)
        {
            string cookieToken = "";
            string formToken = "";

            IEnumerable<string> tokenHeaders;
            if (request.Headers.TryGetValues("YstCsrfToken", out tokenHeaders)) 
            {
                string[] tokens = tokenHeaders.First().Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }
            AntiForgery.Validate(cookieToken, formToken);
        }
    }
}
