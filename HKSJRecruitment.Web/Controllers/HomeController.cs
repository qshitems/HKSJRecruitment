using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Mvc;
using HKSJRecruitment.Models.Models;

namespace HKSJRecruitment.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            RightHeper rightHelper = new RightHeper();
            ViewBag.Menus = rightHelper.LoadMenu(HttpContext.AppUserId());
            ViewBag.AppUser = this.AppUser();
            return View();
        }
        public ActionResult Desktop()
        {
            return View();
        }
    }
}
