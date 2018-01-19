using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Work.Sys.DbInterfaceService;
using Work.Sys.DbModel;

namespace Work.Web.Controllers
{
    public class TestController : Controller
    {
        private ITestService _TestServiceDb { get; set; }

        public TestController(ITestService TestServiceDb) {
            _TestServiceDb = TestServiceDb;
        }
        // GET: Test
        public ActionResult Index()
        {
            var result = _TestServiceDb.AddEntity(new Test() { ID=Guid.NewGuid(), Age=11, CreateTime=DateTime.Now, Name="Test" });
            var NewResult = _TestServiceDb.GetEntityByID(result.ID);
            Expression<Func<Test, bool>> checkStudent1 = s1 => s1.Age > 1200;
            Expression<Func<Test, bool>> checkStudent1 = s1 => s1.Age > 1200;
            Expression<Func<Test, bool>> checkStudent = Expression.Lambda<Func<Test, bool>>(
                      Expression.Or(checkStudent1.Body, checkStudent2.Body), checkStudent1.Parameters);
            _TestServiceDb.GetEntityByWhere2(p => p.Age ==1);
            _TestServiceDb.GetEntityByWhere(p => p.Name == "");
            return View();
        }
    }
}