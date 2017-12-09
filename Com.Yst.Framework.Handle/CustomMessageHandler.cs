using System;
using System.Diagnostics;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Yst.Framework.Logging;

namespace Com.Yst.Framework.Handle
{
    /// <summary>
    /// Webapi消息处理程序
    /// </summary>
    public class CustomMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        private static readonly LogHelp _logger = new LogHelp();

        /// <summary>
        /// 把请求的报文和返回的报文保存到日志中
        /// 用法：
        /// Gloab中 GlobalConfiguration.Configuration.MessageHandlers.Add(new CustomMessageHandler());
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <param name="cancellationToken">取消操作的标记</param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string id = Guid.NewGuid().ToString("N");
            var sc = Stopwatch.StartNew();
            //请求
            if (request.Content != null)
            {
                var sb = new StringBuilder();
                string ip = GetClientIp(request);
                sb.AppendLine(string.Format("请求IP:{0}", ip));
                string method = request.Method.Method;
                if (method.ToLower().Equals("post"))
                {
                    sb.AppendLine(string.Format("请求Id:{0}", id));
                    sb.AppendLine(string.Format("请求Url-{0}", request.RequestUri.ToString()));
                    sb.AppendLine(string.Format("请求Content:{0}", request.Content.ReadAsStringAsync().Result));
                }
                else
                {
                    sb.AppendLine(string.Format("请求Url-{0}:{1}", id, request.RequestUri.ToString()));
                }
                _logger.Info(sb.ToString());

                
            }

            return base.SendAsync(request, cancellationToken).ContinueWith(
            task =>
            {
                string response = task.Result.Content.ReadAsStringAsync().Result;
                
                var sb = new StringBuilder();
                sb.AppendLine("响应Id：" + id);
                sb.AppendLine("响应时间:" + sc.ElapsedMilliseconds);
                sb.AppendLine("响应报文:" + response);

                //_logger.Info(string.Format("响应Content-{2}:{0}{1}——————————————————————————————", response, Environment.NewLine, id));
                _logger.Info(sb.ToString());
                return task.Result;
            }, cancellationToken);
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            return null;
        }
    }
}
