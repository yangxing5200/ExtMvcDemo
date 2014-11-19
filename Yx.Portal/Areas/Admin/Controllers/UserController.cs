using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yx.Portal.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Admin/User/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Admin/User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}