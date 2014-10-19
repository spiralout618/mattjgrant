using mattjgrant.DAL;
using mattjgrant.Models;
using mattjgrant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using Microsoft.AspNet.Identity;//For getting the userid of the current user https://stackoverflow.com/questions/18448637/how-to-get-current-user-and-how-to-use-user-class-in-mvc5

namespace mattjgrant.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Dashboard");
            }

            return View();
        }

        public ActionResult MainNavigation()
        {
            //For getting the userid of the current user https://stackoverflow.com/questions/18448637/how-to-get-current-user-and-how-to-use-user-class-in-mvc5
            // requires using Microsoft.AspNet.Identity;
            //Intelisense didn't know about the Microsoft.AspNet.Identity namespace for some reason
            var loggedInUserID = User.Identity.GetUserId();

            //Test it without using EF for the time being
            var viewModel = new MainNavigationViewModel();
            return PartialView("_MainNavigation", viewModel);
        }
    }
}