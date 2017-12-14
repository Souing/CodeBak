using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAop.Filter
{
    public class ModelValidationAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            if (controller != null && controller.Request.HttpMethod.ToUpper() != "POST")
            {
                return;
            }
            if (controller != null && !controller.ModelState.IsValid)
            {
                string errorMsg = "服务器繁忙,请稍后再试";
                foreach (var value in controller.ModelState.Values)
                {
                    if (!value.Errors.Any())
                    {
                        continue;
                    }
                    foreach (var error in value.Errors)
                    {
                        errorMsg = error.ErrorMessage;
                        break;
                    }
                }
                var obj = new { errorMsg };
                filterContext.Result = new JsonResult() { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}