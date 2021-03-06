﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ApiAop.Filter;

namespace ApiAop
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // Web API 配置和服务

            // Web API 路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
