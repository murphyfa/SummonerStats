using SummonerStats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SummonerStats.Controllers
{
    public class HomeController : Controller
    {
        PlayerProfile profile = new PlayerProfile();

        public ActionResult Index()
        {
            profile.name = profile.pullPlayer("Mount Swolemore");
            ViewBag.Message = profile.name;
            return View(profile);
        }

        public ActionResult About()
        {
            ViewBag.Message = profile.pullPlayer("Mount Swolemore");

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}