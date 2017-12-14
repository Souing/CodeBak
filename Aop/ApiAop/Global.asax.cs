using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Routing;
using ApiAop.Filter;
using ApiAop.Handler;

namespace ApiAop
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
//            GlobalConfiguration.Configuration.Filters.Add(new BasicAuthorizationAttribute());
//            GlobalConfiguration.Configuration.MessageHandlers.Add(new BasicAuthenticationHandler());
            // todo 未验证
//            GlobalConfiguration.Configuration.Filters.Add(new ModelValidAttribute());
            // todo 异常
          //  GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionHandler), new ErrorHandler());
        }
    }
}
