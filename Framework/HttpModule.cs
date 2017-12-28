using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpModuleDemo.Test
{
    public class MyHttpModule : IHttpModule
    {

        /// <summary>
        /// 处置由实现 System.Web.IHttpModule 的模块使用的资源（内存除外）
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// 初始化模块，并使其为处理请求做好准备。
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;//在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生。
            context.EndRequest += context_EndRequest;    //在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生。
        }

        /// <summary>
        /// 在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            HttpRequest request = application.Request;
            HttpResponse response = application.Response;

            response.Write("context_EndRequest >> 在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生");
        }

        /// <summary>
        /// 在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            HttpContext context = application.Context;
            HttpRequest request = application.Request;
            HttpResponse response = application.Response;

            response.Write("context_BeginRequest >> 在 ASP.NET 响应请求时作为 HTTP 执行管线链中的第一个事件发生");
        }
    }
}