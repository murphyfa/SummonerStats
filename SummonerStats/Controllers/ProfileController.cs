using Newtonsoft.Json.Linq;
using SummonerStats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SummonerStats.Controllers
{
    public class ProfileController : Controller
    {
        ProfileViewModel profile = new ProfileViewModel();
        MatchHistoryModel mhm = new MatchHistoryModel();
        MatchDetailsModel mdm = new MatchDetailsModel();
        private MatchHistoryDBContext db = new MatchHistoryDBContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string searchName)
        {

            int summonerID = profile.playerProfile.pullPlayer(searchName);

            mhm.UpdateMatchHistory(summonerID);

            profile.matchHistory = db.MatchHistory.SqlQuery("SELECT TOP(3) * FROM dbo.MatchHistoryModels WHERE id = '" + summonerID + "' ORDER BY timestamp DESC").ToList();

            return View("Profile", profile);
        }


        public int[] FindViewPlayer(IEnumerable<MatchDetailsModel> details, string summonerName)
        {
            int[] playerDetails = new int[15];

            if (details.First().p1Name == summonerName)
            {
                playerDetails[0] = details.First().p1Gold;
                playerDetails[1] = details.First().p1Champ;
                playerDetails[2] = details.First().p1Spell1;
                playerDetails[3] = details.First().p1Spell2;
                playerDetails[4] = details.First().p1Kills;
                playerDetails[5] = details.First().p1Deaths;
                playerDetails[6] = details.First().p1Assists;
                playerDetails[7] = details.First().p1Item1;
                playerDetails[8] = details.First().p1Item2;
                playerDetails[9] = details.First().p1Item3;
                playerDetails[10] = details.First().p1Item4;
                playerDetails[11] = details.First().p1Item5;
                playerDetails[12] = details.First().p1Item6;
                playerDetails[13] = details.First().p1Level;
                playerDetails[14] = details.First().p1CS;
            }
            else if (details.First().p2Name == summonerName)
            {
                playerDetails[0] = details.First().p2Gold;
                playerDetails[1] = details.First().p2Champ;
                playerDetails[2] = details.First().p2Spell1;
                playerDetails[3] = details.First().p2Spell2;
                playerDetails[4] = details.First().p2Kills;
                playerDetails[5] = details.First().p2Deaths;
                playerDetails[6] = details.First().p2Assists;
                playerDetails[7] = details.First().p2Item1;
                playerDetails[8] = details.First().p2Item2;
                playerDetails[9] = details.First().p2Item3;
                playerDetails[10] = details.First().p2Item4;
                playerDetails[11] = details.First().p2Item5;
                playerDetails[12] = details.First().p2Item6;
                playerDetails[13] = details.First().p2Level;
                playerDetails[14] = details.First().p2CS;
            }
            else if (details.First().p3Name == summonerName)
            {
                playerDetails[0] = details.First().p3Gold;
                playerDetails[1] = details.First().p3Champ;
                playerDetails[2] = details.First().p3Spell1;
                playerDetails[3] = details.First().p3Spell2;
                playerDetails[4] = details.First().p3Kills;
                playerDetails[5] = details.First().p3Deaths;
                playerDetails[6] = details.First().p3Assists;
                playerDetails[7] = details.First().p3Item1;
                playerDetails[8] = details.First().p3Item2;
                playerDetails[9] = details.First().p3Item3;
                playerDetails[10] = details.First().p3Item4;
                playerDetails[11] = details.First().p3Item5;
                playerDetails[12] = details.First().p3Item6;
                playerDetails[13] = details.First().p3Level;
                playerDetails[14] = details.First().p3CS;
            }
            else if (details.First().p4Name == summonerName)
            {
                playerDetails[0] = details.First().p4Gold;
                playerDetails[1] = details.First().p4Champ;
                playerDetails[2] = details.First().p4Spell1;
                playerDetails[3] = details.First().p4Spell2;
                playerDetails[4] = details.First().p4Kills;
                playerDetails[5] = details.First().p4Deaths;
                playerDetails[6] = details.First().p4Assists;
                playerDetails[7] = details.First().p4Item1;
                playerDetails[8] = details.First().p4Item2;
                playerDetails[9] = details.First().p4Item3;
                playerDetails[10] = details.First().p4Item4;
                playerDetails[11] = details.First().p4Item5;
                playerDetails[12] = details.First().p4Item6;
                playerDetails[13] = details.First().p4Level;
                playerDetails[14] = details.First().p4CS;
            }
            else if (details.First().p5Name == summonerName)
            {
                playerDetails[0] = details.First().p5Gold;
                playerDetails[1] = details.First().p5Champ;
                playerDetails[2] = details.First().p5Spell1;
                playerDetails[3] = details.First().p5Spell2;
                playerDetails[4] = details.First().p5Kills;
                playerDetails[5] = details.First().p5Deaths;
                playerDetails[6] = details.First().p5Assists;
                playerDetails[7] = details.First().p5Item1;
                playerDetails[8] = details.First().p5Item2;
                playerDetails[9] = details.First().p5Item3;
                playerDetails[10] = details.First().p5Item4;
                playerDetails[11] = details.First().p5Item5;
                playerDetails[12] = details.First().p5Item6;
                playerDetails[13] = details.First().p5Level;
                playerDetails[14] = details.First().p5CS;
            }
            else if (details.First().p6Name == summonerName)
            {
                playerDetails[0] = details.First().p6Gold;
                playerDetails[1] = details.First().p6Champ;
                playerDetails[2] = details.First().p6Spell1;
                playerDetails[3] = details.First().p6Spell2;
                playerDetails[4] = details.First().p6Kills;
                playerDetails[5] = details.First().p6Deaths;
                playerDetails[6] = details.First().p6Assists;
                playerDetails[7] = details.First().p6Item1;
                playerDetails[8] = details.First().p6Item2;
                playerDetails[9] = details.First().p6Item3;
                playerDetails[10] = details.First().p6Item4;
                playerDetails[11] = details.First().p6Item5;
                playerDetails[12] = details.First().p6Item6;
                playerDetails[13] = details.First().p6Level;
                playerDetails[14] = details.First().p6CS;
            }
            else if (details.First().p7Name == summonerName)
            {
                playerDetails[0] = details.First().p7Gold;
                playerDetails[1] = details.First().p7Champ;
                playerDetails[2] = details.First().p7Spell1;
                playerDetails[3] = details.First().p7Spell2;
                playerDetails[4] = details.First().p7Kills;
                playerDetails[5] = details.First().p7Deaths;
                playerDetails[6] = details.First().p7Assists;
                playerDetails[7] = details.First().p7Item1;
                playerDetails[8] = details.First().p7Item2;
                playerDetails[9] = details.First().p7Item3;
                playerDetails[10] = details.First().p7Item4;
                playerDetails[11] = details.First().p7Item5;
                playerDetails[12] = details.First().p7Item6;
                playerDetails[13] = details.First().p7Level;
                playerDetails[14] = details.First().p7CS;
            }
            else if (details.First().p8Name == summonerName)
            {
                playerDetails[0] = details.First().p8Gold;
                playerDetails[1] = details.First().p8Champ;
                playerDetails[2] = details.First().p8Spell1;
                playerDetails[3] = details.First().p8Spell2;
                playerDetails[4] = details.First().p8Kills;
                playerDetails[5] = details.First().p8Deaths;
                playerDetails[6] = details.First().p8Assists;
                playerDetails[7] = details.First().p8Item1;
                playerDetails[8] = details.First().p8Item2;
                playerDetails[9] = details.First().p8Item3;
                playerDetails[10] = details.First().p8Item4;
                playerDetails[11] = details.First().p8Item5;
                playerDetails[12] = details.First().p8Item6;
                playerDetails[13] = details.First().p8Level;
                playerDetails[14] = details.First().p8CS;
            }
            else if (details.First().p9Name == summonerName)
            {
                playerDetails[0] = details.First().p9Gold;
                playerDetails[1] = details.First().p9Champ;
                playerDetails[2] = details.First().p9Spell1;
                playerDetails[3] = details.First().p9Spell2;
                playerDetails[4] = details.First().p9Kills;
                playerDetails[5] = details.First().p9Deaths;
                playerDetails[6] = details.First().p9Assists;
                playerDetails[7] = details.First().p9Item1;
                playerDetails[8] = details.First().p9Item2;
                playerDetails[9] = details.First().p9Item3;
                playerDetails[10] = details.First().p9Item4;
                playerDetails[11] = details.First().p9Item5;
                playerDetails[12] = details.First().p9Item6;
                playerDetails[13] = details.First().p9Level;
                playerDetails[14] = details.First().p9CS;
            }
            else if (details.First().p10Name == summonerName)
            {
                playerDetails[0] = details.First().p10Gold;
                playerDetails[1] = details.First().p10Champ;
                playerDetails[2] = details.First().p10Spell1;
                playerDetails[3] = details.First().p10Spell2;
                playerDetails[4] = details.First().p10Kills;
                playerDetails[5] = details.First().p10Deaths;
                playerDetails[6] = details.First().p10Assists;
                playerDetails[7] = details.First().p10Item1;
                playerDetails[8] = details.First().p10Item2;
                playerDetails[9] = details.First().p10Item3;
                playerDetails[10] = details.First().p10Item4;
                playerDetails[11] = details.First().p10Item5;
                playerDetails[12] = details.First().p10Item6;
                playerDetails[13] = details.First().p10Level;
                playerDetails[14] = details.First().p10CS;
            }

            return playerDetails;
        }

        public string ChampById (int champID)
        {
            string champName;

            string apiKey = "RGAPI-ecaff961-7b62-4bd7-988f-33f0003e77e7";
            string champURL = "https://global.api.pvp.net/api/lol/static-data/na/v1.2/champion/" + champID + "?api_key=" + apiKey;

            using (var client = new WebClient())
            {
                string champData = client.DownloadString(champURL);
                JObject champRecords = JObject.Parse(champData);

                champName = (string)champRecords["name"];
            }

                return champName;
        }

        public string ItemURL (int itemID)
        {
            string itemURL;
            if (itemID != 0)
            {
                itemURL = "http://ddragon.leagueoflegends.com/cdn/6.24.1/img/item/" + itemID + ".png";
            } else
            {
                itemURL = "";
            }
            return itemURL;
        }
    }
}