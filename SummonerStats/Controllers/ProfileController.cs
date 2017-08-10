using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SummonerStats.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SummonerStats.Controllers
{
    public class ProfileController : Controller
    {
        ProfileViewModel profile = new ProfileViewModel();
        tblMatchHistory mhm = new tblMatchHistory();
        tblMatchDetails mdm = new tblMatchDetails();
        private MatchHistoryDBContext db = new MatchHistoryDBContext();

        JObject itemData = null;
        JObject sumData = null;
        JObject champRecords = null;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string searchName)
        {
            int summonerID = profile.playerProfile.pullPlayer(searchName);

            if (profile.playerProfile.name != "The player was not found")
            {
                using (tblPlayersDB pdb = new tblPlayersDB())
                {
                    var p = pdb.Players.Where(a => a.AccountID == profile.playerProfile.accountId);
                    if (p.First().UpdatedDate != null)
                    {
                        profile.lastUpdate = (int)DateTime.UtcNow.Subtract((DateTime)p.First().UpdatedDate).TotalSeconds;
                    }
                    else
                    {
                        profile.lastUpdate = 0;
                    }
                }

                profile.mh.TopChamps(summonerID);
                //profile.mh.Top5Champs(summonerID);
                profile.matchHistory = db.MatchHistory.SqlQuery("SELECT TOP(10) * FROM dbo.tblMatchHistory WHERE id = '" + summonerID + "' ORDER BY timestamp DESC").ToList();
                if (profile.matchHistory.Count() == 0)
                {
                    profile.firstLoad = true;
                }
                else
                {
                    profile.firstLoad = false;
                }
            }
            return View("Profile", profile);
        }

        [HttpGet]
        public ActionResult UpdatePlayer(string searchName, bool firstUpdate, long accountID)
        {
            int summonerID = profile.playerProfile.pullPlayer(searchName);

            using (tblPlayersDB db = new tblPlayersDB())
            {
                tblPlayers p = new tblPlayers();
                p = db.Players.Find(accountID);

                var currentTime = DateTime.UtcNow;
                var currentTimeMS = (long)(currentTime - new DateTime(1970, 1, 1)).TotalMilliseconds;

                if (p.UpdatedDate != null)
                {
                    var updateTimeMS = (long)((DateTime)p.UpdatedDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
                    if (currentTimeMS > updateTimeMS + 300000)
                    {
                        p.UpdatedDate = DateTime.UtcNow;
                        profile.lastUpdate = 1;
                        db.Players.Attach(p);
                        var entry = db.Entry(p);
                        entry.Property(e => e.UpdatedDate).IsModified = true;
                        db.SaveChanges();

                        if (firstUpdate == true)
                        {
                            profile.mh.FirstUpdate(summonerID, searchName, profile.playerProfile.accountId);
                            Task.Run(() =>
                            {
                                profile.mh.UpdateMatchHistory(summonerID, searchName, profile.playerProfile.accountId, true);
                            });
                        }
                        else
                        {
                            profile.mh.UpdateMatchHistory(summonerID, searchName, profile.playerProfile.accountId, false);
                        }
                    }
                }
                else
                {
                    p.UpdatedDate = DateTime.UtcNow;
                    profile.lastUpdate = 1;
                    db.Players.Attach(p);
                    var entry = db.Entry(p);
                    entry.Property(e => e.UpdatedDate).IsModified = true;
                    db.SaveChanges();

                    if (firstUpdate == true)
                    {
                        profile.mh.FirstUpdate(summonerID, searchName, profile.playerProfile.accountId);
                        Task.Run(() =>
                        {
                            profile.mh.UpdateMatchHistory(summonerID, searchName, profile.playerProfile.accountId, true);
                        });
                    }
                    else
                    {
                        profile.mh.UpdateMatchHistory(summonerID, searchName, profile.playerProfile.accountId, false);
                    }
                }
            }
            profile.mh.TopChamps(summonerID);
            profile.matchHistory = db.MatchHistory.SqlQuery("SELECT TOP(10) * FROM dbo.tblMatchHistory WHERE id = '" + summonerID + "' ORDER BY timestamp DESC").ToList();

            return View("Profile", profile);
        }

        [HttpGet]
        public PartialViewResult GetTenMore(string searchName, long lastMatch)
        {
            int summonerID = profile.playerProfile.pullPlayer(searchName);
            profile.matchHistory = db.MatchHistory.SqlQuery("SELECT TOP(10) * FROM dbo.tblMatchHistory WHERE id = '" + summonerID + "' and timestamp < '" + lastMatch + "' ORDER BY timestamp DESC").ToList();
            return PartialView("MatchDetails", profile);
        }


        //public int[] FindViewPlayer(IEnumerable<tblMatchDetails> details, string summonerName)
        //{
        //    int[] playerDetails = new int[16];
        //    playerDetails[15] = 0;

        //    if (details.First().p1Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p1Gold;
        //        playerDetails[1] = details.First().p1Champ;
        //        playerDetails[2] = details.First().p1Spell1;
        //        playerDetails[3] = details.First().p1Spell2;
        //        playerDetails[4] = details.First().p1Kills;
        //        playerDetails[5] = details.First().p1Deaths;
        //        playerDetails[6] = details.First().p1Assists;
        //        playerDetails[7] = details.First().p1Item1;
        //        playerDetails[8] = details.First().p1Item2;
        //        playerDetails[9] = details.First().p1Item3;
        //        playerDetails[10] = details.First().p1Item4;
        //        playerDetails[11] = details.First().p1Item5;
        //        playerDetails[12] = details.First().p1Item6;
        //        playerDetails[13] = details.First().p1Level;
        //        playerDetails[14] = details.First().p1CS;
                
        //        if (details.First().winner == "Team1")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }
        //    else if (details.First().p2Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p2Gold;
        //        playerDetails[1] = details.First().p2Champ;
        //        playerDetails[2] = details.First().p2Spell1;
        //        playerDetails[3] = details.First().p2Spell2;
        //        playerDetails[4] = details.First().p2Kills;
        //        playerDetails[5] = details.First().p2Deaths;
        //        playerDetails[6] = details.First().p2Assists;
        //        playerDetails[7] = details.First().p2Item1;
        //        playerDetails[8] = details.First().p2Item2;
        //        playerDetails[9] = details.First().p2Item3;
        //        playerDetails[10] = details.First().p2Item4;
        //        playerDetails[11] = details.First().p2Item5;
        //        playerDetails[12] = details.First().p2Item6;
        //        playerDetails[13] = details.First().p2Level;
        //        playerDetails[14] = details.First().p2CS;

        //        if (details.First().winner == "Team1")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }
        //    else if (details.First().p3Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p3Gold;
        //        playerDetails[1] = details.First().p3Champ;
        //        playerDetails[2] = details.First().p3Spell1;
        //        playerDetails[3] = details.First().p3Spell2;
        //        playerDetails[4] = details.First().p3Kills;
        //        playerDetails[5] = details.First().p3Deaths;
        //        playerDetails[6] = details.First().p3Assists;
        //        playerDetails[7] = details.First().p3Item1;
        //        playerDetails[8] = details.First().p3Item2;
        //        playerDetails[9] = details.First().p3Item3;
        //        playerDetails[10] = details.First().p3Item4;
        //        playerDetails[11] = details.First().p3Item5;
        //        playerDetails[12] = details.First().p3Item6;
        //        playerDetails[13] = details.First().p3Level;
        //        playerDetails[14] = details.First().p3CS;

        //        if (details.First().winner == "Team1")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }
        //    else if (details.First().p4Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p4Gold;
        //        playerDetails[1] = details.First().p4Champ;
        //        playerDetails[2] = details.First().p4Spell1;
        //        playerDetails[3] = details.First().p4Spell2;
        //        playerDetails[4] = details.First().p4Kills;
        //        playerDetails[5] = details.First().p4Deaths;
        //        playerDetails[6] = details.First().p4Assists;
        //        playerDetails[7] = details.First().p4Item1;
        //        playerDetails[8] = details.First().p4Item2;
        //        playerDetails[9] = details.First().p4Item3;
        //        playerDetails[10] = details.First().p4Item4;
        //        playerDetails[11] = details.First().p4Item5;
        //        playerDetails[12] = details.First().p4Item6;
        //        playerDetails[13] = details.First().p4Level;
        //        playerDetails[14] = details.First().p4CS;

        //        if (details.First().winner == "Team1")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }
        //    else if (details.First().p5Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p5Gold;
        //        playerDetails[1] = details.First().p5Champ;
        //        playerDetails[2] = details.First().p5Spell1;
        //        playerDetails[3] = details.First().p5Spell2;
        //        playerDetails[4] = details.First().p5Kills;
        //        playerDetails[5] = details.First().p5Deaths;
        //        playerDetails[6] = details.First().p5Assists;
        //        playerDetails[7] = details.First().p5Item1;
        //        playerDetails[8] = details.First().p5Item2;
        //        playerDetails[9] = details.First().p5Item3;
        //        playerDetails[10] = details.First().p5Item4;
        //        playerDetails[11] = details.First().p5Item5;
        //        playerDetails[12] = details.First().p5Item6;
        //        playerDetails[13] = details.First().p5Level;
        //        playerDetails[14] = details.First().p5CS;

        //        if (details.First().winner == "Team1")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }
        //    else if (details.First().p6Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p6Gold;
        //        playerDetails[1] = details.First().p6Champ;
        //        playerDetails[2] = details.First().p6Spell1;
        //        playerDetails[3] = details.First().p6Spell2;
        //        playerDetails[4] = details.First().p6Kills;
        //        playerDetails[5] = details.First().p6Deaths;
        //        playerDetails[6] = details.First().p6Assists;
        //        playerDetails[7] = details.First().p6Item1;
        //        playerDetails[8] = details.First().p6Item2;
        //        playerDetails[9] = details.First().p6Item3;
        //        playerDetails[10] = details.First().p6Item4;
        //        playerDetails[11] = details.First().p6Item5;
        //        playerDetails[12] = details.First().p6Item6;
        //        playerDetails[13] = details.First().p6Level;
        //        playerDetails[14] = details.First().p6CS;

        //        if (details.First().winner == "Team2")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }
        //    else if (details.First().p7Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p7Gold;
        //        playerDetails[1] = details.First().p7Champ;
        //        playerDetails[2] = details.First().p7Spell1;
        //        playerDetails[3] = details.First().p7Spell2;
        //        playerDetails[4] = details.First().p7Kills;
        //        playerDetails[5] = details.First().p7Deaths;
        //        playerDetails[6] = details.First().p7Assists;
        //        playerDetails[7] = details.First().p7Item1;
        //        playerDetails[8] = details.First().p7Item2;
        //        playerDetails[9] = details.First().p7Item3;
        //        playerDetails[10] = details.First().p7Item4;
        //        playerDetails[11] = details.First().p7Item5;
        //        playerDetails[12] = details.First().p7Item6;
        //        playerDetails[13] = details.First().p7Level;
        //        playerDetails[14] = details.First().p7CS;

        //        if (details.First().winner == "Team2")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }
        //    else if (details.First().p8Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p8Gold;
        //        playerDetails[1] = details.First().p8Champ;
        //        playerDetails[2] = details.First().p8Spell1;
        //        playerDetails[3] = details.First().p8Spell2;
        //        playerDetails[4] = details.First().p8Kills;
        //        playerDetails[5] = details.First().p8Deaths;
        //        playerDetails[6] = details.First().p8Assists;
        //        playerDetails[7] = details.First().p8Item1;
        //        playerDetails[8] = details.First().p8Item2;
        //        playerDetails[9] = details.First().p8Item3;
        //        playerDetails[10] = details.First().p8Item4;
        //        playerDetails[11] = details.First().p8Item5;
        //        playerDetails[12] = details.First().p8Item6;
        //        playerDetails[13] = details.First().p8Level;
        //        playerDetails[14] = details.First().p8CS;

        //        if (details.First().winner == "Team2")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }
        //    else if (details.First().p9Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p9Gold;
        //        playerDetails[1] = details.First().p9Champ;
        //        playerDetails[2] = details.First().p9Spell1;
        //        playerDetails[3] = details.First().p9Spell2;
        //        playerDetails[4] = details.First().p9Kills;
        //        playerDetails[5] = details.First().p9Deaths;
        //        playerDetails[6] = details.First().p9Assists;
        //        playerDetails[7] = details.First().p9Item1;
        //        playerDetails[8] = details.First().p9Item2;
        //        playerDetails[9] = details.First().p9Item3;
        //        playerDetails[10] = details.First().p9Item4;
        //        playerDetails[11] = details.First().p9Item5;
        //        playerDetails[12] = details.First().p9Item6;
        //        playerDetails[13] = details.First().p9Level;
        //        playerDetails[14] = details.First().p9CS;

        //        if (details.First().winner == "Team2")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }
        //    else if (details.First().p10Name.Equals(summonerName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        playerDetails[0] = details.First().p10Gold;
        //        playerDetails[1] = details.First().p10Champ;
        //        playerDetails[2] = details.First().p10Spell1;
        //        playerDetails[3] = details.First().p10Spell2;
        //        playerDetails[4] = details.First().p10Kills;
        //        playerDetails[5] = details.First().p10Deaths;
        //        playerDetails[6] = details.First().p10Assists;
        //        playerDetails[7] = details.First().p10Item1;
        //        playerDetails[8] = details.First().p10Item2;
        //        playerDetails[9] = details.First().p10Item3;
        //        playerDetails[10] = details.First().p10Item4;
        //        playerDetails[11] = details.First().p10Item5;
        //        playerDetails[12] = details.First().p10Item6;
        //        playerDetails[13] = details.First().p10Level;
        //        playerDetails[14] = details.First().p10CS;

        //        if (details.First().winner == "Team2")
        //        {
        //            playerDetails[15] = 1;
        //        }
        //    }

        //    return playerDetails;
        //}

        public int[] FindViewPlayer(IEnumerable<tblMatchDetails> details, long accountId)
        {
            int[] playerDetails = new int[16];
            playerDetails[15] = 0;

            if (details.First().p1Id == accountId)
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

                if (details.First().winner == "Team1")
                {
                    playerDetails[15] = 1;
                }
            }
            else if (details.First().p2Id == accountId)
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

                if (details.First().winner == "Team1")
                {
                    playerDetails[15] = 1;
                }
            }
            else if (details.First().p3Id == accountId)
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

                if (details.First().winner == "Team1")
                {
                    playerDetails[15] = 1;
                }
            }
            else if (details.First().p4Id == accountId)
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

                if (details.First().winner == "Team1")
                {
                    playerDetails[15] = 1;
                }
            }
            else if (details.First().p5Id == accountId)
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

                if (details.First().winner == "Team1")
                {
                    playerDetails[15] = 1;
                }
            }
            else if (details.First().p6Id == accountId)
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

                if (details.First().winner == "Team2")
                {
                    playerDetails[15] = 1;
                }
            }
            else if (details.First().p7Id == accountId)
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

                if (details.First().winner == "Team2")
                {
                    playerDetails[15] = 1;
                }
            }
            else if (details.First().p8Id == accountId)
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

                if (details.First().winner == "Team2")
                {
                    playerDetails[15] = 1;
                }
            }
            else if (details.First().p9Id == accountId)
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

                if (details.First().winner == "Team2")
                {
                    playerDetails[15] = 1;
                }
            }
            else if (details.First().p10Id == accountId)
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

                if (details.First().winner == "Team2")
                {
                    playerDetails[15] = 1;
                }
            }

            return playerDetails;
        }

        //public string ChampById (int champID)
        //{
        //    string champName;

        //    string apiKey = "RGAPI-62b5d58e-b1bf-4667-be65-b901aa5e89cc";
        //    string champURL = "https://global.api.pvp.net/api/lol/static-data/na/v1.2/champion/" + champID + "?api_key=" + apiKey;

        //    using (var client = new WebClient())
        //    {
        //        string champData = client.DownloadString(champURL);
        //        JObject champRecords = JObject.Parse(champData);

        //        champName = (string)champRecords["name"];
        //    }

        //    return champName;
        //}

        public string ChampById(int champID)
        {
            string champName;
            // Had to update this to read from file for now because a change to the API now limits static data methods to be called once per 10 minutes
            // Using data from file will need to be updated if any of the data is changed in a patch

            //string champURL = "https://na1.api.riotgames.com/lol/static-data/v3/champions?dataById=true&api_key=RGAPI-62b5d58e-b1bf-4667-be65-b901aa5e89cc";

            if (champRecords == null)
            {
                //using (var client = new WebClient())
                //{
                //    string champData = client.DownloadString(champURL);
                //    champRecords = JObject.Parse(champData);
                //}

                champRecords = JObject.Parse(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/JSON/champion.json"));
            }

            champName = (string)champRecords["data"][champID.ToString()]["name"];

            return champName;
        }

        public string ConvertTimeStamp(long timestamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(timestamp).ToShortDateString();
        }

        public string ItemJSON(int itemNumber)
        {
            string itemInfo;
            if (itemData == null)
            {
                itemData = JObject.Parse(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/JSON/item.json"));
            }

            if (itemNumber != 0)
            {
                try
                {
                    itemInfo = "<span class='tt-name'>" + itemData["data"][itemNumber.ToString()]["name"] + "</span><span class='tt-text'>" + itemData["data"][itemNumber.ToString()]["plaintext"] + "</span><span class='tt-desc'>" + itemData["data"][itemNumber.ToString()]["description"] + "</span>";
                }
                catch
                {
                    //using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                    //{
                    //    string itemURL = "https://na1.api.riotgames.com/lol/static-data/v3/items/" + itemNumber + "?api_key=RGAPI-62b5d58e-b1bf-4667-be65-b901aa5e89cc";
                    //    string itemDataX = client.DownloadString(itemURL);
                    //    JObject itemJSONX = JObject.Parse(itemDataX);

                    //    itemInfo = "<span class='tt-name'>" + itemJSONX["name"] + "</span><span class='tt-text'>" + itemJSONX["plaintext"] + "</span><span class='tt-desc'>" + itemJSONX["description"] + "</span>";
                    //}
                    itemInfo = "";
                }
            }
            else
            {
                itemInfo = "";
            }
            return itemInfo;
        }

        public string SumSpellJSON(int ssID)
        {
            string ssInfo;
            //string ssURL = "https://na1.api.riotgames.com/lol/static-data/v3/summoner-spells?dataById=true&api_key=RGAPI-62b5d58e-b1bf-4667-be65-b901aa5e89cc";
            // Had to update this to read from file for now because a change to the API now limits static data methods to be called once per 10 minutes
            // Using data from file will need to be updated if any of the data is changed in a patch
            if (sumData == null)
            {
                //using (WebClient wc = new WebClient())
                //{
                //    string ssDL = wc.DownloadString(ssURL);
                //    sumData = JObject.Parse(ssDL);
                //}
                sumData = JObject.Parse(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/JSON/summoner.json"));
            }

            if (ssID != 0)
            {
                ssInfo = "<span class='tt-name'>" + sumData["data"][ssID.ToString()]["name"] + "</span><span class='tt-desc'>" + sumData["data"][ssID.ToString()]["description"] + "</span>";
            }
            else
            {
                ssInfo = "";
            }

            return ssInfo;
        }
    }
}