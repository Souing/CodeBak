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
    public class ModelValidationAttribute : ActionFilterAttribute
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
                if (modelState.Values.Where(value => value.Errors.Any()).Any(value => value.Errors.Select(error => error.ErrorMessage).Any()))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { name = "123", age = 123 }), System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
                    };
                    return;
                }
            }
            base.OnActionExecuting(actionContext);
        }
    }
}