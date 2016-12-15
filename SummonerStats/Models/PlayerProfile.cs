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
        public string name { get; set; }

        public string pullPlayer(string searchName)
        {
            string apiKey = "RGAPI-ecaff961-7b62-4bd7-988f-33f0003e77e7";
            string profileURL = "https://na.api.pvp.net/api/lol/na/v1.4/summoner/by-name/" + searchName.Replace(" ","").ToLower() + "?api_key=" + apiKey;
            try
            {
                using (var client = new WebClient())
                {
                    string data = client.DownloadString(profileURL);
                    dynamic dyn = JsonConvert.DeserializeObject(data);
                    name = dyn.name;
                }
            } catch
            {
                name = "Player Not Found";
            }

            return name;
        }
    }
}