using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;

namespace SummonerStats.Models
{
    public class MatchHistoryModel
    {
        [Key]
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

        public void UpdateMatchHistory(int sumID)
        {
            MatchHistoryDBContext db = new MatchHistoryDBContext();

            string mhURL;
            string apiKey = "RGAPI-ecaff961-7b62-4bd7-988f-33f0003e77e7";

            //if we have no matches stored, start at beginning of season 7, otherwise only since most recent
            if (db.MatchHistory.SqlQuery("SELECT * FROM dbo.MatchHistoryModels WHERE id = '" + sumID + "'").ToList().Count() == 0)
            {
                mhURL = "https://na.api.pvp.net/api/lol/na/v2.2/matchlist/by-summoner/" + sumID + "?beginTime=1481108400000&api_key=" + apiKey;
            } else
            {
                //long lastMatch = Int64.Parse(db.MatchHistory.SqlQuery("SELECT TOP(1) timestamp FROM dbo.MatchHistoryModels WHERE id = '" + sumID + "' ORDER BY timestamp DESC").First().ToString());
                long lastMatch = db.MatchHistory.OrderByDescending(u => u.timestamp).Where(u => u.id == sumID).Select(u => u.timestamp).First();
                mhURL = "https://na.api.pvp.net/api/lol/na/v2.2/matchlist/by-summoner/" + sumID + "?beginTime=" + lastMatch + "&api_key=" + apiKey;
            }

            using (var client = new WebClient())
            {
                string mhData = client.DownloadString(mhURL);
                JObject mhRecords = JObject.Parse(mhData);

                MatchHistoryModel mhm = new MatchHistoryModel();

                for (int i = 0; i < mhRecords["matches"].Count(); i++)
                {
                    if (db.MatchHistory.SqlQuery("SELECT * FROM dbo.MatchHistoryModels WHERE id = '" + sumID + "' AND timestamp = '" + (Int64)mhRecords["matches"][i]["timestamp"] + "'").ToList().Count() == 0)
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

                        db.MatchHistory.Add(mhm);
                        db.SaveChanges();
                    }
                }
            }
        }
    }

    public class MatchHistoryDBContext : DbContext
    {
        public DbSet<MatchHistoryModel> MatchHistory { get; set; }
    }
}
