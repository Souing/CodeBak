using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;

namespace WebAop.Filter
{
    public class FormAuthenticateAttribute : BaseAuthenticateAttribute
    {
        private const string TokenKey = "token";
        public override bool IsAuthenticated(AuthenticationContext filterContext)
        {
            string tokenValue = filterContext.HttpContext.Request.Headers[TokenKey];
            if (string.IsNullOrEmpty(tokenValue))
            {
                tokenValue = filterContext.HttpContext.Request[TokenKey];
            }
            if (tokenValue == null)
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
            throw new NotImplementedException();
        }

        protected override void AjaxProcessUnauthenticatedRequest(AuthenticationContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}