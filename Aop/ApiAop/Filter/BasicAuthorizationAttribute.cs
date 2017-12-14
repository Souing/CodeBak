using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ApiAop.Filter
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BasicAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string userName = null;
            string userPswd = null;
            var result = ParseHeader(actionContext, ref userName, ref userPswd);
            if (!result)
            {
                EmptyChallenge(actionContext);
                return;
            }

            if (!OnAuthorize(userName, userPswd, actionContext))
            {
                ErrorChallenge(actionContext);
                return;
            }

            base.OnAuthorization(actionContext);
        }

        public virtual bool ParseHeader(HttpActionContext actionContext, ref string userName, ref string userPswd)
        {
            string authParameter = null;

            var authValue = actionContext.Request.Headers.Authorization;
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

        public virtual bool OnAuthorize(string userName, string userPassword, HttpActionContext actionContext)
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

        private void EmptyChallenge(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            HttpContent httpContent = new StringContent("请输入用户名密码验证");
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = httpContent
            };
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));

        }

        private void ErrorChallenge(HttpActionContext actionContext)
        {
            HttpContent httpContent = new StringContent("账号密码有误");
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = httpContent,
                StatusCode = HttpStatusCode.Unauthorized
            };
        }
    }
}