using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.WebPages;

namespace WebAop.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BasicAuthenticateAttribute : BaseAuthenticateAttribute
    {
        protected const string AuthorizationHeaderName = "Authorization";
        protected const string WwwAuthenticationHeaderName = "WWW-Authenticate";
        protected const string BasicAuthenticationScheme = "Basic";

        public override bool IsAuthenticated(AuthenticationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any() ||filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            {
                return true;
            }
//         var a =   filterContext.ActionDescriptor.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

            string userName = null;
            string userPswd = null;
            var result = ParseHeader(filterContext, ref userName, ref userPswd);

            if (!result)
            {
                return false;
            }
            if (!OnAuthorize(userName, userPswd, filterContext))
            {
                return false;
            }
            return true;
        }

        public virtual bool ParseHeader(AuthenticationContext filterContext, ref string userName, ref string userPswd)
        {

            var authValue = filterContext.RequestContext.HttpContext.Request.Headers[AuthorizationHeaderName];
            if (string.IsNullOrEmpty(authValue))
            {
                return false;
            }

            authValue = Encoding.Default.GetString(Convert.FromBase64String(authValue.Split(' ')[1]));

            var authToken = authValue.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (authToken.Length < 2)
            {
                return false;
            }
            userName = authToken[0];
            userPswd = authToken[1];
            return true;
        }

        private bool OnAuthorize(string userName, string userPassword, AuthenticationContext filterContext)
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
     

        protected override void ProcessUnauthenticatedRequest(AuthenticationContext filterContext)
        {
            // ReSharper disable once PossibleNullReferenceException
            string parameter = string.Format("Basic realm=\"{0}\"", filterContext.RequestContext.HttpContext.Request.Url.DnsSafeHost);
            filterContext.HttpContext.Response.Headers[WwwAuthenticationHeaderName] = parameter;
            filterContext.Result = new HttpUnauthorizedResult();
        }

        protected override void AjaxProcessUnauthenticatedRequest(AuthenticationContext filterContext)
        {
            filterContext.Result = new ContentResult { Content = "没有权限" };
        }
    }
}