using System.Web.Mvc;
using Souing.Net.Admin.Automapper;
using Souing.Net.BLL.IService;
using Souing.Net.ViewModel.Admin.UserInfo;

namespace Souing.Net.Admin.Controllers
{
    public class UserInfoController : Controller
    {

        private readonly IUserInfoService _userInfoService;

        public UserInfoController(IUserInfoService userInfoService)
        {
            this._userInfoService = userInfoService;
        }

        //
        // GET: /UserInfo/

        public ActionResult Index()
        {
            var modelList = _userInfoService.GetEntities(p => true).ToModelList();
            return View(modelList);
        }

        //
        // GET: /UserInfo/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /UserInfo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserInfo/Create

        [HttpPost]
        public ActionResult Create(UserInfoViewModel request)
        {
            try
            {
                // TODO: Add insert logic here
                _userInfoService.Insert(request.ToEntity());
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /UserInfo/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /UserInfo/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /UserInfo/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /UserInfo/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
