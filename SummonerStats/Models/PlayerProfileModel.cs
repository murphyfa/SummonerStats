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

        //ranked champ stats
        public string topChampOne { get; set; }
        public int topGamesOne { get; set; }
        public string topChampTwo { get; set; }
        public int topGamesTwo { get; set; }
        public string topChampThree { get; set; }
        public int topGamesThree { get; set; }
        public string topChampFour { get; set; }
        public int topGamesFour { get; set; }
        public string topChampFive { get; set; }
        public int topGamesFive { get; set; }
        public string topChampBG { get; set; }


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

                    //dummy data for testing
                    //string profileData = @"{ ""mountswolemore"": {
                    //               ""id"": 20895054,
                    //               ""name"": ""Mount Swolemore"",
                    //               ""profileIconId"": 582,
                    //               ""revisionDate"": 1481777583000,
                    //               ""summonerLevel"": 30
                    //             }
                    //}";

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

                    Top5Champs(id, apiKey);
                }
        }
            catch
            {
                name = "Player Not Found";
            }

            return id;
        }

        public void Top5Champs(int sumID, string key)
        {
            using (var client = new WebClient())
            {
                //stats from ranked games
                //currently only looking at season 7
                string statsURL = "https://na.api.pvp.net/api/lol/na/v1.3/stats/by-summoner/" + id + "/ranked?season=SEASON2017&api_key=" + key;
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

                topChampBG = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + topFiveNames[0].Replace(" ","") + "_0.jpg";

                topChampOne = topFiveNames[0];
                topGamesOne = champsPlayed[0].Item2;
                topChampTwo = topFiveNames[1];
                topGamesTwo = champsPlayed[1].Item2;
                topChampThree = topFiveNames[2];
                topGamesThree = champsPlayed[2].Item2;
                topChampFour = topFiveNames[3];
                topGamesFour = champsPlayed[3].Item2;
                topChampFive = topFiveNames[4];
                topGamesFive = champsPlayed[4].Item2;
            }
        }
    }

    public class PlayerProfileDBContext : DbContext
    {
        public DbSet<PlayerProfileModel> PlayerProfile { get; set; }
    }
}