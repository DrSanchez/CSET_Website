using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSETWebsite_Build.Controllers
{
	public class DegreesController : Controller
	{
		//
		// GET: /Degrees/Index
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ConcurrentDegree()
		{
			return View();
		}
		public ActionResult EmbeddedDegree()
		{
			return View();
		}
		public ActionResult HardwareDegree()
		{
			return View();
		}
		public ActionResult SoftwareDegree()
		{
			return View();
		}
	}
}
