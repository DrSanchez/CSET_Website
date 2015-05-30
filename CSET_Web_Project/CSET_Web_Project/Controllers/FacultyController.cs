using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSET_Web_Project.Models;

namespace CSET_Web_Project.Controllers
{
    public class FacultyController : Controller
    {
        // GET: /Faculty/
        public ActionResult Index()
        {
            return View(Faculty.GetAllActiveFaculty());
        }

		// GET: /Faculty/Profile
		public ActionResult Profile(uint id)
		{
			try
			{
				return View(new Faculty((int) id));
			}
			catch
			{
				return View("Page not found...");
			}
		}
    }
}