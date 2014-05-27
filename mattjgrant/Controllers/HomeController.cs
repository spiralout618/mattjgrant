using mattjgrant.DAL;
using mattjgrant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mattjgrant.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new ChecklistContext();
            context.Checklists.Add(new Checklist { CreatedDate = DateTime.Now, IsActive = true, Name = "Test", UpdatedDate = DateTime.Now });
            context.SaveChanges();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}