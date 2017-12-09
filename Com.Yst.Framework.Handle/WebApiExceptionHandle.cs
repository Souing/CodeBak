using System.Web.Http.Filters;
using Com.Yst.Framework.Logger;

namespace Com.Yst.Framework.Handle
{
    public class WebApiExceptionHandle : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LoggerFactory.GetLoggerManage().Error(actionExecutedContext.Exception.Message);
        }
    }
}
