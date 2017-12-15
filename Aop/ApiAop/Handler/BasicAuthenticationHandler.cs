using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ApiAop.Handler
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        private const string AuthenticationHeader = "WWW-Authenticate";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string userName = null;
            string userPswd = null;
            var result = ParseHeader(request, ref userName, ref userPswd);
            if (!result)
            {
                return EmptyChallenge(request);
            }
            if (!OnAuthorize(userName, userPswd, request))
            {
                return ErrorChallenge(request);
            }
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                var response = task.Result;
                return response;
            }, cancellationToken);
        }

        public virtual bool ParseHeader(HttpRequestMessage actionContext, ref string userName, ref string userPswd)
        {
            string authParameter = null;

            var authValue = actionContext.Headers.Authorization;
            if (authValue != null && authValue.Scheme == "Basic")
                authParameter = authValue.Parameter;

            if (string.IsNullOrEmpty(authParameter))
            {
                return false;
            }

            authParameter = Encoding.Default.GetString(Convert.FromBase64String(authParameter));

            var authToken = authParameter.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (authToken.Length < 2)
            {
                return false;
            }
            userName = authToken[0];
            userPswd = authToken[1];
            return true;
        }

        public virtual bool OnAuthorize(string userName, string userPassword, HttpRequestMessage actionContext)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userPassword))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Task<HttpResponseMessage> EmptyChallenge(HttpRequestMessage actionContext)
        {
            var host = actionContext.RequestUri.DnsSafeHost;
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Headers.Add(AuthenticationHeader, string.Format("Basic realm=\"{0}\"", host));
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        private Task<HttpResponseMessage> ErrorChallenge(HttpRequestMessage actionContext)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("账号密码有误"),
                StatusCode = HttpStatusCode.Unauthorized
            };
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}