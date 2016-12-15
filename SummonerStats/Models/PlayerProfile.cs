using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace SummonerStats.Models
{
    public class PlayerProfile
    {
        public int id { get; set; }
        public string name { get; set; }
        public int profileIconId { get; set; }
        public long revisionDate { get; set; }
        public int summonerLevel { get; set; }

        public string pullPlayer(string searchName)
        {
            string playerToFind = searchName.Replace(" ", "").ToLower();
            string apiKey = "RGAPI-ecaff961-7b62-4bd7-988f-33f0003e77e7";
            string profileURL = "https://na.api.pvp.net/api/lol/na/v1.4/summoner/by-name/" + playerToFind + "?api_key=" + apiKey;
            try
            {
                using (var client = new WebClient())
                {
                    ////string data = client.DownloadString(profileURL);
                    //string data = @"{ ""mountswolemore"": {
                    //               ""id"": 20895054,
                    //               ""name"": ""Mount Swolemore"",
                    //               ""profileIconId"": 582,
                    //               ""revisionDate"": 1481777583000,
                    //               ""summonerLevel"": 30
                    //             }
                    //}";
                    //var stats = JsonConvert.DeserializeObject<Dictionary<string, PlayerProfile>>(data);

                    //id = stats.First().Value.id;
                    //name = stats.First().Value.name;
                    //profileIconId = stats.First().Value.profileIconId;
                    //summonerLevel = stats.First().Value.summonerLevel;
                    name = searchName;
                    summonerLevel = 30;
                    profileIconId = 582;
                }
            }
            catch
            {
                name = "Player Not Found";
            }

            return name;
        }
    }
}