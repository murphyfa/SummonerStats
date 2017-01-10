using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using SummonerStats.Controllers;

namespace SummonerStats.Models
{
    public partial class tblMatchHistory
    {

        //ranked champ stats
        public string topChampOne;
        public int totalGamesOne;
        public int killsOne;
        public int deathsOne;
        public int assistsOne;
        public int gamesOne;
        public int winsOne;
        public int lossesOne;
        public string winrateOne;
        public string topChampTwo;
        public int totalGamesTwo;
        public int killsTwo;
        public int deathsTwo;
        public int assistsTwo;
        public int gamesTwo;
        public int winsTwo;
        public int lossesTwo;
        public string winrateTwo;
        public string topChampThree;
        public int totalGamesThree;
        public int killsThree;
        public int deathsThree;
        public int assistsThree;
        public int gamesThree;
        public int winsThree;
        public int lossesThree;
        public string winrateThree;
        public string topChampFour;
        public int totalGamesFour;
        public int killsFour;
        public int deathsFour;
        public int assistsFour;
        public int gamesFour;
        public int winsFour;
        public int lossesFour;
        public string winrateFour;
        public string topChampFive;
        public int totalGamesFive;
        public int killsFive;
        public int deathsFive;
        public int assistsFive;
        public int gamesFive;
        public int winsFive;
        public int lossesFive;
        public string winrateFive;

        public string topChampBG;

        public void UpdateMatchHistory(int sumID, string sumName)
        {
            System.Diagnostics.Debug.WriteLine("UPDATING MATCH HISTORY");
            MatchHistoryDBContext db = new MatchHistoryDBContext();

            string mhURL;
            string apiKey = "RGAPI-ecaff961-7b62-4bd7-988f-33f0003e77e7";

            //if we have no matches stored, start at beginning of season 7, otherwise only since most recent
            if (db.MatchHistory.Where(u => u.id == sumID).ToList().Count() == 0)
            {
                mhURL = "https://na.api.pvp.net/api/lol/na/v2.2/matchlist/by-summoner/" + sumID + "?beginTime=1481108400000&api_key=" + apiKey;
            }
            else
            {
                long lastMatch = db.MatchHistory.OrderByDescending(u => u.timestamp).Where(u => u.id == sumID).Select(u => u.timestamp).First();
                mhURL = "https://na.api.pvp.net/api/lol/na/v2.2/matchlist/by-summoner/" + sumID + "?beginTime=" + lastMatch + "&api_key=" + apiKey;
            }

            using (var client = new WebClient())
            {
                string mhData = client.DownloadString(mhURL);
                JObject mhRecords = JObject.Parse(mhData);

                for (int i = 0; i < mhRecords["matches"].Count(); i++)
                {
                    tblMatchHistory mhm = new tblMatchHistory();
                    tblMatchDetails mdm = new tblMatchDetails();
                    ProfileController pc = new ProfileController();
                    long matchID = (Int64)mhRecords["matches"][i]["matchId"];

                    System.Diagnostics.Debug.WriteLine("PULLING MATCH " + matchID);
                    var matchDetails = mdm.PullMatch(matchID);
                    int[] playerStats = pc.FindViewPlayer(matchDetails, sumName);

                    long lastTimestamp = (Int64)mhRecords["matches"][i]["timestamp"];
                    if (db.MatchHistory.Where(u => u.id == sumID && u.timestamp == lastTimestamp).ToList().Count() == 0)
                    {
                        mhm.id = sumID;
                        mhm.timestamp = (Int64)mhRecords["matches"][i]["timestamp"];
                        mhm.champion = (Int32)mhRecords["matches"][i]["champion"];
                        mhm.region = (string)mhRecords["matches"][i]["region"];
                        mhm.queue = (string)mhRecords["matches"][i]["queue"];
                        mhm.season = (string)mhRecords["matches"][i]["season"];
                        mhm.matchId = (Int64)mhRecords["matches"][i]["matchId"];
                        mhm.role = (string)mhRecords["matches"][i]["role"];
                        mhm.platformId = (string)mhRecords["matches"][i]["platformId"];
                        mhm.lane = (string)mhRecords["matches"][i]["lane"];
                        mhm.kills = playerStats[4];
                        mhm.deaths = playerStats[5];
                        mhm.assists = playerStats[6];
                        mhm.winner = playerStats[15];

                        db.MatchHistory.Add(mhm);
                        db.SaveChanges();
                    }

                    System.Threading.Thread.Sleep(3000);


                    //db.Database.ExecuteSqlCommand("UPDATE dbo.tblMatchHistory SET kills = " + playerStats[4] + ", deaths = " + playerStats[5] + ", assists = " + playerStats[6] + ", winner = " + playerStats[15] + " WHERE id = " + sumID + " AND matchId = " + matchID);
                }
            }
            
            Top5Champs(sumID, apiKey);
        }

        public void Top5Champs(int sumID, string key)
        {
            using (var client = new WebClient())
            {
                //stats from ranked games
                //currently only looking at season 7
                string statsURL = "https://na.api.pvp.net/api/lol/na/v1.3/stats/by-summoner/" + sumID + "/ranked?season=SEASON2017&api_key=" + key;
                string statsData = client.DownloadString(statsURL);
                JObject statsStats = JObject.Parse(statsData);

                var champsPlayed = new List<Tuple<int, int>>();

                for (int i = 0; i < statsStats["champions"].ToList().Count; i++)
                {
                    champsPlayed.Add(Tuple.Create((Int32)statsStats["champions"][i]["id"], (Int32)statsStats["champions"][i]["stats"]["totalSessionsPlayed"]));
                }

                champsPlayed.RemoveAll(item => item.Item1 == 0);
                champsPlayed.Sort((x, y) => y.Item2.CompareTo(x.Item2));

                //requesting champ names by id
                //request from static api to not count against pull limit
                string[] topFiveNames = new string[5];

                for (int i = 0; i < 5; i++)
                {
                    string staticChampInfo = "https://global.api.pvp.net/api/lol/static-data/na/v1.2/champion/" + champsPlayed[i].Item1 + "?api_key=" + key;
                    string champData = client.DownloadString(staticChampInfo);
                    JObject champInfo = JObject.Parse(champData);
                    topFiveNames[i] = (string)champInfo["name"];
                }

                topChampBG = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + topFiveNames[0].Replace(" ", "") + "_0.jpg";

                ProfileController pc = new ProfileController();
                MatchHistoryDBContext db = new MatchHistoryDBContext();


                int champOne = champsPlayed[0].Item1;
                topChampOne = pc.ChampById(champOne);
                totalGamesOne = champsPlayed[0].Item2;
                killsOne = db.MatchHistory.Where(u => u.id == sumID && u.champion == champOne).Sum(u => (Int32)u.kills) / totalGamesOne;
                deathsOne = db.MatchHistory.Where(u => u.id == sumID && u.champion == champOne).Sum(u => (Int32)u.deaths) / totalGamesOne;
                assistsOne = db.MatchHistory.Where(u => u.id == sumID && u.champion == champOne).Sum(u => (Int32)u.assists) / totalGamesOne;
                winsOne = db.MatchHistory.Where(u => u.id == sumID && u.champion == champOne).Sum(u => (Int32)u.winner);
                lossesOne = totalGamesOne - db.MatchHistory.Where(u => u.id == sumID && u.champion == champOne).Sum(u => (Int32)u.winner);
                winrateOne = Math.Round((double)winsOne * 100 / totalGamesOne).ToString() + "%";

                topChampTwo = pc.ChampById(champsPlayed[1].Item1);
                totalGamesTwo = champsPlayed[1].Item2;
                killsTwo = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[1].Item1).Sum(u => (Int32)u.kills) / totalGamesTwo;
                deathsTwo = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[1].Item1).Sum(u => (Int32)u.deaths) / totalGamesTwo;
                assistsTwo = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[1].Item1).Sum(u => (Int32)u.assists) / totalGamesTwo;
                winsTwo = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[1].Item1).Sum(u => (Int32)u.winner);
                lossesTwo = totalGamesTwo - db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[1].Item1).Sum(u => (Int32)u.winner);
                winrateTwo = String.Format("{0:0%}", winsTwo / totalGamesTwo);

                topChampThree = pc.ChampById(champsPlayed[2].Item1);
                totalGamesThree = champsPlayed[2].Item2;
                killsThree = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[2].Item1).Sum(u => (Int32)u.kills) / totalGamesThree;
                deathsThree = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[2].Item1).Sum(u => (Int32)u.deaths) / totalGamesThree;
                assistsThree = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[2].Item1).Sum(u => (Int32)u.assists) / totalGamesThree;
                winsThree = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[2].Item1).Sum(u => (Int32)u.winner);
                lossesThree = totalGamesThree - db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[2].Item1).Sum(u => (Int32)u.winner);
                winrateThree = String.Format("{0:0%}", winsThree / totalGamesThree);

                topChampFour = pc.ChampById(champsPlayed[3].Item1);
                totalGamesFour = champsPlayed[3].Item2;
                killsFour = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[3].Item1).Sum(u => (Int32)u.kills) / totalGamesFour;
                deathsFour = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[3].Item1).Sum(u => (Int32)u.deaths) / totalGamesFour;
                assistsFour = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[3].Item1).Sum(u => (Int32)u.assists) / totalGamesFour;
                winsFour = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[3].Item1).Sum(u => (Int32)u.winner);
                lossesFour = totalGamesFour - db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[3].Item1).Sum(u => (Int32)u.winner);
                winrateFour = String.Format("{0:0%}", winsFour / totalGamesFour);

                topChampFive = pc.ChampById(champsPlayed[4].Item1);
                totalGamesFive = champsPlayed[4].Item2;
                killsFive = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[4].Item1).Sum(u => (Int32)u.kills) / totalGamesFive;
                deathsFive = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[4].Item1).Sum(u => (Int32)u.deaths) / totalGamesFive;
                assistsFive = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[4].Item1).Sum(u => (Int32)u.assists) / totalGamesFive;
                winsFive = db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[4].Item1).Sum(u => (Int32)u.winner);
                lossesFive = totalGamesFive - db.MatchHistory.AsEnumerable().Where(u => u.id == sumID && u.champion == champsPlayed[4].Item1).Sum(u => (Int32)u.winner);
                winrateFive = String.Format("{0:0%}", winsFive / totalGamesFive);
            }
        }
    }

    public class MatchHistoryDBContext : DbContext
    {
        public MatchHistoryDBContext()
            : base("SummonerStatsDBEntities")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<tblMatchHistory> MatchHistory { get; set; }
    }
}
