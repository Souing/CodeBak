using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace FilterTest.Filter
{
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BaseAuthenticateAttribute : FilterAttribute, IAuthenticationFilter
    {
        public const string AuthorizationHeaderName = "Authorization";
        public const string WwwAuthenticationHeaderName = "WWW-Authenticate";
        public const string BasicAuthenticationScheme = "Basic";
        private static Dictionary<string, string> userAccounters;

        static BaseAuthenticateAttribute()
        {
            userAccounters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            userAccounters.Add("Foo", "Password");
            userAccounters.Add("Bar", "Password");
            userAccounters.Add("Baz", "Password");
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            IPrincipal user;
            if (this.IsAuthenticated(filterContext, out user))
            {
                filterContext.Principal = user;
            }
            else
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new ContentResult() {Content = "没有权限"};
                }
                else
                {
                    this.ProcessUnauthenticatedRequest(filterContext);
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
//            throw new NotImplementedException();
        }

        protected virtual AuthenticationHeaderValue GetAuthenticationHeaderValue(AuthenticationContext filterContext)
        {
            string rawValue = filterContext.RequestContext.HttpContext.Request.Headers[AuthorizationHeaderName];
            if (string.IsNullOrEmpty(rawValue))
            {
                return null;
            }
            string[] split = rawValue.Split(' ');
            if (split.Length != 2)
            {
                return null;
            }
            return new AuthenticationHeaderValue(split[0], split[1]);
        }

        protected virtual bool IsAuthenticated(AuthenticationContext filterContext, out IPrincipal user)
        {
            user = filterContext.Principal;
            if (null != user & user.Identity.IsAuthenticated)
            {
                return true;
            }

            AuthenticationHeaderValue token = this.GetAuthenticationHeaderValue(filterContext);
            if (null != token && token.Scheme == BasicAuthenticationScheme)
            {
                string credential = Encoding.Default.GetString(Convert.FromBase64String(token.Parameter));
                string[] split = credential.Split(':');
                if (split.Length == 2)
                {
                    string userName = split[0];
                    string password;
                    if (userAccounters.TryGetValue(userName, out password))
                    {
                        if (password == split[1])
                        {
                            GenericIdentity identity = new GenericIdentity(userName);
                            user = new GenericPrincipal(identity, new string[0]);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        protected virtual void ProcessUnauthenticatedRequest(AuthenticationContext filterContext)
        {
            string parameter = string.Format("realm=\"{0}\"", filterContext.RequestContext.HttpContext.Request.Url.DnsSafeHost);
            var challenge = new AuthenticationHeaderValue(BasicAuthenticationScheme, parameter);
            filterContext.HttpContext.Response.Headers[WwwAuthenticationHeaderName] = challenge.ToString();
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }

    public class AuthorizateAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }
    }

    public class AuthenticateTest : BaseAuthenticateAttribute
    {
        protected override bool IsAuthenticated(AuthenticationContext filterContext, out IPrincipal user)
        {
            return base.IsAuthenticated(filterContext, out user);
        }

        protected override AuthenticationHeaderValue GetAuthenticationHeaderValue(AuthenticationContext filterContext)
        {
            return base.GetAuthenticationHeaderValue(filterContext);
        }
    }
}