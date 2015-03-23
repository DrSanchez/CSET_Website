using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSETWebsite_Build.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Computer Systems Engineering Technology";

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Oregon Institute of Technology CSET Department";

			return View();
		}

		public ActionResult Contact()
		{
			return View();
		}

		public ActionResult ExtendedBlurb()
		{
			ViewBag.Message = "";

			return View();
		}
	}
}
