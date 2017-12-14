using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonClass;
using WebAop.Filter;

namespace WebAop.Controllers
{
//    [NoValidate]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(PersonModel personModel)
        {
            return Content("hellp");
        }
         
        public ActionResult Index2()
        {
            return Content("hellp2222222222");
        }
    }
}