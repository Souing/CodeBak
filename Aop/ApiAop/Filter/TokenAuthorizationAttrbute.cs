using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace ApiAop.Filter
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class TokenAuthorizationAttrbute : AuthorizationFilterAttribute
    {
    }
}