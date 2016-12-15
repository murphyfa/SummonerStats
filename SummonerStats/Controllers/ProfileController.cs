using SummonerStats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SummonerStats.Controllers
{
    public class ProfileController : Controller
    {
        PlayerProfile profile = new PlayerProfile();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string searchName)
        {
            profile.pullPlayer(searchName);

            return View("Profile", profile);
        }
    }
}