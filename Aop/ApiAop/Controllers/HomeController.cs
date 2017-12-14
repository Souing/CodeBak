using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiAop.Filter;
using CommonClass;

namespace ApiAop.Controllers
{
    public class HomeController : ApiController
    {
//        [BasicAuthorizationFilter]
        [ValidateModel]
        [HttpGet]
        public string Index(PersonModel person)
        {
            bool boolResult = ModelState.IsValid;
            string str = person.Email;
            return boolResult.ToString();
        }
        [HttpGet]
        public string Index2()
        {
            return "234";
        }

        public HttpResponseMessage Post(PersonModel product)
        {
            if (ModelState.IsValid)
            {
                // Do something with the product (not shown).

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}
