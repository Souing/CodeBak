using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace WebAop.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseAuthenticateAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (!this.IsAuthenticated(filterContext))
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    this.AjaxProcessUnauthenticatedRequest(filterContext);
                }
                else
                {
                    this.ProcessUnauthenticatedRequest(filterContext);
                }
            }
        }

        public virtual void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }

        public abstract bool IsAuthenticated(AuthenticationContext filterContext);

        /// <summary>
        /// 处理未通过验证的请求
        /// </summary>
        /// <param name="filterContext"></param>
        protected abstract void ProcessUnauthenticatedRequest(AuthenticationContext filterContext);

        /// <summary>
        /// 处理未通过验证的ajax请求
        /// </summary>
        /// <param name="filterContext"></param>
        protected abstract void AjaxProcessUnauthenticatedRequest(AuthenticationContext filterContext);
    }
}