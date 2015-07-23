using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UVa_ASP.NET_MVC.Utils;

namespace UVa_ASP.NET_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var data = Util.ranklist();
            var sorted = data.OrderByDescending(s => s.AC);
            return View(sorted);
        }
    }
}