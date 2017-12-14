using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;

namespace ApiAop.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Method != HttpMethod.Post)
            {
                return;
            }
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                foreach (var value in modelState.Values)
                {
                    if (!value.Errors.Any())
                    {
                        continue;
                    }
                    foreach (var error in value.Errors)
                    {
                        string errorMsg = error.ErrorMessage;
                        // 默认的格式
                                         //       actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMsg);
                        // 自定义格式 todo
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
                                             {
                                                 Content = new StringContent(JsonConvert.SerializeObject(new { name = "123", age = 123 }), System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
                                             };
                        break;
                    }
                }


            }
            base.OnActionExecuting(actionContext);
        }
    }
}