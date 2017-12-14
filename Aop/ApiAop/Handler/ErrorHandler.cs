using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace ApiAop.Handler
{
    public class ErrorHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is ArgumentException)
            {
                context.Result = new ResponseMessageResult(context.Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    Message = context.Exception.Message
                }));
            }
            else
            {
                context.Result = new ResponseMessageResult(context.Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Message = "服务器已被外星人绑架"
                }));
            }

            base.Handle(context);
        }
    }
}