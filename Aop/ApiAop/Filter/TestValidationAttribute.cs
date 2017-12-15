using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;

namespace ApiAop.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class TestValidationAttribute : ActionFilterAttribute
    {
        public string errorMsg { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
 
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { ErrorMsg = errorMsg }), System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
                    };
        }
    }
}