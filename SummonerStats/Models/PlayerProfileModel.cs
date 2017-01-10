using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Data.Entity;

namespace SummonerStats.Models
{
    public class PlayerProfileModel
    {
        //profile stats
        public int id { get; set; }
        public string name { get; set; }
        public int profileIconId { get; set; }
        public long revisionDate { get; set; }
        public int summonerLevel { get; set; }

        //league stats
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
            string apiKey = "RGAPI-ecaff961-7b62-4bd7-988f-33f0003e77e7";

            string profileURL = "https://na.api.pvp.net/api/lol/na/v1.4/summoner/by-name/" + playerToFind + "?api_key=" + apiKey;

            try
            {
                using (var client = new WebClient())
                {
                    //basic profile info
                    string profileData = client.DownloadString(profileURL);

                    var profileStats = JsonConvert.DeserializeObject<Dictionary<string, PlayerProfileModel>>(profileData);

                    id = profileStats.First().Value.id;
                    name = profileStats.First().Value.name;
                    profileIconId = profileStats.First().Value.profileIconId;
                    summonerLevel = profileStats.First().Value.summonerLevel;

                    //info from leagues request
                    string leagueURL = "https://na.api.pvp.net/api/lol/na/v2.5/league/by-summoner/" + id + "/entry?api_key=" + apiKey;
                    string leagueData = client.DownloadString(leagueURL);
                    JObject leagueStats = JObject.Parse(leagueData);

                    leaguePoints = (Int32)leagueStats[id.ToString()][0]["entries"][0]["leaguePoints"];
                    league = (string)leagueStats[id.ToString()][0]["tier"];
                    division = (string)leagueStats[id.ToString()][0]["entries"][0]["division"];
                    wins = (Int32)leagueStats[id.ToString()][0]["entries"][0]["wins"];
                    losses = (Int32)leagueStats[id.ToString()][0]["entries"][0]["losses"];

                }
        }
            catch
            {
                name = "Player Not Found";
            }

            return id;
        }


    }

    public class PlayerProfileDBContext : DbContext
    {
        public DbSet<PlayerProfileModel> PlayerProfile { get; set; }
    }
}