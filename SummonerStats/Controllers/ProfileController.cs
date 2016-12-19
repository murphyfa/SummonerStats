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
        ProfileViewModel profile = new ProfileViewModel();
        private MatchHistoryDBContext db = new MatchHistoryDBContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string searchName)
        {

            int summonerID = profile.playerProfile.pullPlayer(searchName);
            //profile.playerProfile.pullPlayer(searchName);

            profile.matchHistory = db.MatchHistory.SqlQuery("SELECT * FROM dbo.MatchHistoryModels").ToList();
            //profile.matchHistory = db.MatchHistory.ToList();

            return View("Profile", profile);
        }
    }
}