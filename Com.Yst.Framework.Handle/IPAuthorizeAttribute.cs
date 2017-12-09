using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Com.Yst.Framework.Config;
using Com.Yst.Framework.Logger;

namespace Com.Yst.Framework.Handle
{
    public class IpAuthorizeAttribute : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var allowIpSwitch = WebConfig.WebEnvConfig.ItemList["AllowIPsSwitch"];
            if (allowIpSwitch == "1")
            {
                var ip = GetClientIp(request);
                var allowIPs = WebConfig.WebEnvConfig.ItemList["AllowIPs"];
                var index = allowIPs.IndexOf(ip, System.StringComparison.Ordinal);

                if (index == -1)
                {
                    
                    var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    string log = string.Format("{0}访问被拒绝", ip);
                    LoggerFactory.GetLoggerManage().Info(log);

                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(response);
                    return tsc.Task;
                }    
            }
            return base.SendAsync(request, cancellationToken);
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
