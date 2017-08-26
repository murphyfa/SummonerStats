using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using System.Text;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SummonerStats.Models
{
    public class PlayerProfileModel
    {
        //profile stats
        public int id { get; set; }
        public long accountId { get; set; }
        public string name { get; set; }
        public int profileIconId { get; set; }
        public long revisionDate { get; set; }
        public int summonerLevel { get; set; }

        //league stats
        public string group { get; set; }
        public int leaguePoints { get; set; }
        public bool isFreshBlood { get; set; }
        public bool isHotStreak { get; set; }
        public string league { get; set; }
        public string division { get; set; }
        public bool isInactive { get; set; }
        public bool isVeteran { get; set; }
        public int losses { get; set; }
        public string playerOrTeamName { get; set; }
        public string playerOrTeamId { get; set; }
        public int wins { get; set; }

        public int pullPlayer(string searchName)
        {
            string playerToFind = searchName.Replace(" ", "").ToLower();
            string apiKey = "RGAPI-62b5d58e-b1bf-4667-be65-b901aa5e89cc";

            string profileURL = "https://na1.api.riotgames.com/lol/summoner/v3/summoners/by-name/" + playerToFind + "?api_key=" + apiKey;

            //try
            //{
                using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    //basic profile info
                    string profileData = client.DownloadString(profileURL);
                    JObject profileStats = JObject.Parse(profileData);

                    id = (Int32)profileStats["id"];
                    accountId = (long)profileStats["accountId"];
                    name = (string)profileStats["name"];
                    profileIconId = (Int32)profileStats["profileIconId"];
                    summonerLevel = (Int32)profileStats["summonerLevel"];

                    //try
                    //{
                        //info from leagues request
                        string leagueURL = "https://na1.api.riotgames.com/lol/league/v3/positions/by-summoner/" + id + "?api_key=" + apiKey;
                        string leagueData = client.DownloadString(leagueURL);
                        leagueData = "{\"ranks\":" + leagueData + "}";
                        JObject leagueStats = JObject.Parse(leagueData);
                        
                        if ((string)leagueStats["ranks"][0]["queueType"] == "RANKED_SOLO_5x5")
                        {
                            group = (string)leagueStats["ranks"][0]["leagueName"];
                            leaguePoints = (int)leagueStats["ranks"][0]["leaguePoints"];
                            league = (string)leagueStats["ranks"][0]["tier"];
                            division = (string)leagueStats["ranks"][0]["rank"];
                            wins = (int)leagueStats["ranks"][0]["wins"];
                            losses = (int)leagueStats["ranks"][0]["losses"];
                        }
                        else if ((string)leagueStats["ranks"][1]["queueType"] == "RANKED_SOLO_5x5")
                        {
                            group = (string)leagueStats["ranks"][1]["leagueName"];
                            leaguePoints = (int)leagueStats["ranks"][1]["leaguePoints"];
                            league = (string)leagueStats["ranks"][1]["tier"];
                            division = (string)leagueStats["ranks"][1]["rank"];
                            wins = (int)leagueStats["ranks"][1]["wins"];
                            losses = (int)leagueStats["ranks"][1]["losses"];
                        }
                        else
                        {
                            group = "";
                            leaguePoints = 0;
                            league = "UNRANKED";
                            division = "";
                            wins = 0;
                            losses = 0;
                        }

                    //}
                    //catch
                    //{
                    //    group = "";
                    //    leaguePoints = 0;
                    //    league = "UNRANKED";
                    //    division = "";
                    //    wins = 0;
                    //    losses = 0;
                    //}

                }

                //check for player in database and add if necessary
                tblPlayersDB db = new tblPlayersDB();

                if (db.Players.Where(a => a.AccountID == accountId).Count() == 0)
                {
                    tblPlayers p = new tblPlayers();
                    p.AccountID = accountId;
                    p.SummonerID = id;
                    p.Name = name;
                    p.CreatedDate = DateTime.UtcNow;

                    db.Players.Add(p);
                    db.SaveChanges();
                }
                //else if (db.Players.Where(n => n.AccountID == accountId).Select(n => n.Name).ToList().ToString() != name)
                //{
                //    tblPlayers p = new tblPlayers();
                //    var player = db.Players.Where(n => n.AccountID == accountId).Select(n => n.Name);
                //    System.Diagnostics.Debug.WriteLine("PLAYER NEEDS TO BE UPDATED: " + player.ToList().ToString());
                //}
            //}
            //catch
            //{
            //    name = "The player was not found";
            //}

            return id;
        }


    }

    public class tblPlayersDB : DbContext
    {
        public tblPlayersDB()
            : base("SummonerStatsDBEntities")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<tblPlayers> Players { get; set; }
    }
}