using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SummonerStats.Models
{
    public class MatchHistoryModel
    {
        public long matchIndex { get; set; }
        public int id { get; set; }
        public long timestamp { get; set; }
        public int champion { get; set; }
        public string region { get; set; }
        public string queue { get; set; }
        public string season { get; set; }
        public long matchId { get; set; }
        public string role { get; set; }
        public string platformId { get; set; }
        public string lane { get; set; }

        public void PullMatchHistory(int sumID)
        {
            string apiKey = "RGAPI-ecaff961-7b62-4bd7-988f-33f0003e77e7";
            string mhURL = "https://na.api.pvp.net/api/lol/na/v2.2/matchlist/by-summoner/" + sumID + "?beginTime=1481108400000&api_key=" + apiKey; //only season 7


        }
    }


    public class Rootobject
    {
        public MatchHistoryModel[] matches { get; set; }
        public int totalGames { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
    }

    public class MatchHistoryDBContext : DbContext
    {
        public DbSet<MatchHistoryModel> MatchHistory { get; set; }
    }
}