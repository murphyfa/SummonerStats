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
    public class MatchDetailsModel
    {
        //general info
        [Key]
        public int matchIndex { get; set; }
        public string region { get; set; }
        public string matchType { get; set; }
        public long matchCreation { get; set; }
        public string platformId { get; set; }
        public string matchMode { get; set; }
        public int mapId { get; set; }
        public string season { get; set; }
        public string queueType { get; set; }
        public long matchId { get; set; }
        public int matchDuration { get; set; }
        public string winner { get; set; }

        //player names
        public string p1Name { get; set; }
        public string p2Name { get; set; }
        public string p3Name { get; set; }
        public string p4Name { get; set; }
        public string p5Name { get; set; }
        public string p6Name { get; set; }
        public string p7Name { get; set; }
        public string p8Name { get; set; }
        public string p9Name { get; set; }
        public string p10Name { get; set; }

        //player summoner skills
        public int p1Spell1 { get; set; }
        public int p1Spell2 { get; set; }
        public int p2Spell1 { get; set; }
        public int p2Spell2 { get; set; }
        public int p3Spell1 { get; set; }
        public int p3Spell2 { get; set; }
        public int p4Spell1 { get; set; }
        public int p4Spell2 { get; set; }
        public int p5Spell1 { get; set; }
        public int p5Spell2 { get; set; }
        public int p6Spell1 { get; set; }
        public int p6Spell2 { get; set; }
        public int p7Spell1 { get; set; }
        public int p7Spell2 { get; set; }
        public int p8Spell1 { get; set; }
        public int p8Spell2 { get; set; }
        public int p9Spell1 { get; set; }
        public int p9Spell2 { get; set; }
        public int p10Spell1 { get; set; }
        public int p10Spell2 { get; set; }

        //player champions
        public int p1Champ { get; set; }
        public int p2Champ { get; set; }
        public int p3Champ { get; set; }
        public int p4Champ { get; set; }
        public int p5Champ { get; set; }
        public int p6Champ { get; set; }
        public int p7Champ { get; set; }
        public int p8Champ { get; set; }
        public int p9Champ { get; set; }
        public int p10Champ { get; set; }

        //player ranks
        public string p1Tier { get; set; }
        public string p2Tier { get; set; }
        public string p3Tier { get; set; }
        public string p4Tier { get; set; }
        public string p5Tier { get; set; }
        public string p6Tier { get; set; }
        public string p7Tier { get; set; }
        public string p8Tier { get; set; }
        public string p9Tier { get; set; }
        public string p10Tier { get; set; }

        //player items - item0-5
        public int p1Item1 { get; set; }
        public int p1Item2 { get; set; }
        public int p1Item3 { get; set; }
        public int p1Item4 { get; set; }
        public int p1Item5 { get; set; }
        public int p1Item6 { get; set; }
        public int p2Item1 { get; set; }
        public int p2Item2 { get; set; }
        public int p2Item3 { get; set; }
        public int p2Item4 { get; set; }
        public int p2Item5 { get; set; }
        public int p2Item6 { get; set; }
        public int p3Item1 { get; set; }
        public int p3Item2 { get; set; }
        public int p3Item3 { get; set; }
        public int p3Item4 { get; set; }
        public int p3Item5 { get; set; }
        public int p3Item6 { get; set; }
        public int p4Item1 { get; set; }
        public int p4Item2 { get; set; }
        public int p4Item3 { get; set; }
        public int p4Item4 { get; set; }
        public int p4Item5 { get; set; }
        public int p4Item6 { get; set; }
        public int p5Item1 { get; set; }
        public int p5Item2 { get; set; }
        public int p5Item3 { get; set; }
        public int p5Item4 { get; set; }
        public int p5Item5 { get; set; }
        public int p5Item6 { get; set; }
        public int p6Item1 { get; set; }
        public int p6Item2 { get; set; }
        public int p6Item3 { get; set; }
        public int p6Item4 { get; set; }
        public int p6Item5 { get; set; }
        public int p6Item6 { get; set; }
        public int p7Item1 { get; set; }
        public int p7Item2 { get; set; }
        public int p7Item3 { get; set; }
        public int p7Item4 { get; set; }
        public int p7Item5 { get; set; }
        public int p7Item6 { get; set; }
        public int p8Item1 { get; set; }
        public int p8Item2 { get; set; }
        public int p8Item3 { get; set; }
        public int p8Item4 { get; set; }
        public int p8Item5 { get; set; }
        public int p8Item6 { get; set; }
        public int p9Item1 { get; set; }
        public int p9Item2 { get; set; }
        public int p9Item3 { get; set; }
        public int p9Item4 { get; set; }
        public int p9Item5 { get; set; }
        public int p9Item6 { get; set; }
        public int p10Item1 { get; set; }
        public int p10Item2 { get; set; }
        public int p10Item3 { get; set; }
        public int p10Item4 { get; set; }
        public int p10Item5 { get; set; }
        public int p10Item6 { get; set; }

        //player kills/deaths/assists
        public int p1Kills { get; set; }
        public int p1Deaths { get; set; }
        public int p1Assists { get; set; }
        public int p2Kills { get; set; }
        public int p2Deaths { get; set; }
        public int p2Assists { get; set; }
        public int p3Kills { get; set; }
        public int p3Deaths { get; set; }
        public int p3Assists { get; set; }
        public int p4Kills { get; set; }
        public int p4Deaths { get; set; }
        public int p4Assists { get; set; }
        public int p5Kills { get; set; }
        public int p5Deaths { get; set; }
        public int p5Assists { get; set; }
        public int p6Kills { get; set; }
        public int p6Deaths { get; set; }
        public int p6Assists { get; set; }
        public int p7Kills { get; set; }
        public int p7Deaths { get; set; }
        public int p7Assists { get; set; }
        public int p8Kills { get; set; }
        public int p8Deaths { get; set; }
        public int p8Assists { get; set; }
        public int p9Kills { get; set; }
        public int p9Deaths { get; set; }
        public int p9Assists { get; set; }
        public int p10Kills { get; set; }
        public int p10Deaths { get; set; }
        public int p10Assists { get; set; }

        //player levels - champLevel
        public int p1Level { get; set; }
        public int p2Level { get; set; }
        public int p3Level { get; set; }
        public int p4Level { get; set; }
        public int p5Level { get; set; }
        public int p6Level { get; set; }
        public int p7Level { get; set; }
        public int p8Level { get; set; }
        public int p9Level { get; set; }
        public int p10Level { get; set; }

        //creeps kills - minionsKilled
        public int p1CS { get; set; }
        public int p2CS { get; set; }
        public int p3CS { get; set; }
        public int p4CS { get; set; }
        public int p5CS { get; set; }
        public int p6CS { get; set; }
        public int p7CS { get; set; }
        public int p8CS { get; set; }
        public int p9CS { get; set; }
        public int p10CS { get; set; }

        //player damage - totalDamageDealt
        public int p1Damage { get; set; }
        public int p2Damage { get; set; }
        public int p3Damage { get; set; }
        public int p4Damage { get; set; }
        public int p5Damage { get; set; }
        public int p6Damage { get; set; }
        public int p7Damage { get; set; }
        public int p8Damage { get; set; }
        public int p9Damage { get; set; }
        public int p10Damage { get; set; }

        //player gold
        public int p1Gold { get; set; }
        public int p2Gold { get; set; }
        public int p3Gold { get; set; }
        public int p4Gold { get; set; }
        public int p5Gold { get; set; }
        public int p6Gold { get; set; }
        public int p7Gold { get; set; }
        public int p8Gold { get; set; }
        public int p9Gold { get; set; }
        public int p10Gold { get; set; }

        public List<MatchDetailsModel> PullMatch(long matchID)
        {
            MatchDetailsDBContext db = new MatchDetailsDBContext();
            List<MatchDetailsModel> matchDetails = null;

            if (db.MatchDetails.Where(u => u.matchId == matchID).ToList().Count() > 0)
            {
                matchDetails = db.MatchDetails.Where(u => u.matchId == matchID).ToList();
                System.Diagnostics.Debug.WriteLine("Match found in database!");
            } else
            {
                System.Diagnostics.Debug.WriteLine("No match found, retrieving data!");
                UpdateMatch(matchID);
                matchDetails = db.MatchDetails.Where(u => u.matchId == matchID).ToList();
            }

            
            return matchDetails;
        }

        public void UpdateMatch(long matchID)
        {
            string apiKey = "RGAPI-ecaff961-7b62-4bd7-988f-33f0003e77e7";
            string mdURL = "https://na.api.pvp.net/api/lol/na/v2.2/match/" + matchID + "?api_key=" + apiKey;

            using (var client = new WebClient())
            {
                string mdData = client.DownloadString(mdURL);
                JObject mdRecords = JObject.Parse(mdData);

                MatchDetailsModel mdm = new MatchDetailsModel();

                mdm.region = (string)mdRecords["region"];
                mdm.matchType = (string)mdRecords["matchType"];
                mdm.matchCreation = (Int64)mdRecords["matchCreation"];
                mdm.platformId = (string)mdRecords["platformId"];
                mdm.matchMode = (string)mdRecords["matchMode"];
                mdm.mapId = (Int32)mdRecords["mapId"];
                mdm.season = (string)mdRecords["season"];
                mdm.queueType = (string)mdRecords["queueType"];
                mdm.matchId = (Int64)mdRecords["matchId"];
                mdm.matchDuration = (Int32)mdRecords["matchDuration"];
                mdm.winner = (string)((string)mdRecords["teams"][0]["winner"] == "true" ? "Team1" : "Team2");

                //player names
                mdm.p1Name = (string)mdRecords["participantIdentities"][0]["player"]["summonerName"];
                mdm.p2Name = (string)mdRecords["participantIdentities"][1]["player"]["summonerName"];
                mdm.p3Name = (string)mdRecords["participantIdentities"][2]["player"]["summonerName"];
                mdm.p4Name = (string)mdRecords["participantIdentities"][3]["player"]["summonerName"];
                mdm.p5Name = (string)mdRecords["participantIdentities"][4]["player"]["summonerName"];
                mdm.p6Name = (string)mdRecords["participantIdentities"][5]["player"]["summonerName"];
                mdm.p7Name = (string)mdRecords["participantIdentities"][6]["player"]["summonerName"];
                mdm.p8Name = (string)mdRecords["participantIdentities"][7]["player"]["summonerName"];
                mdm.p9Name = (string)mdRecords["participantIdentities"][8]["player"]["summonerName"];
                mdm.p10Name = (string)mdRecords["participantIdentities"][9]["player"]["summonerName"];

                //player summoner skills
                mdm.p1Spell1 = (Int32)mdRecords["participants"][0]["spell1Id"];
                mdm.p1Spell2 = (Int32)mdRecords["participants"][0]["spell2Id"];
                mdm.p2Spell1 = (Int32)mdRecords["participants"][1]["spell1Id"];
                mdm.p2Spell2 = (Int32)mdRecords["participants"][1]["spell2Id"];
                mdm.p3Spell1 = (Int32)mdRecords["participants"][2]["spell1Id"];
                mdm.p3Spell2 = (Int32)mdRecords["participants"][2]["spell2Id"];
                mdm.p4Spell1 = (Int32)mdRecords["participants"][3]["spell1Id"];
                mdm.p4Spell2 = (Int32)mdRecords["participants"][3]["spell2Id"];
                mdm.p5Spell1 = (Int32)mdRecords["participants"][4]["spell1Id"];
                mdm.p5Spell2 = (Int32)mdRecords["participants"][4]["spell2Id"];
                mdm.p6Spell1 = (Int32)mdRecords["participants"][5]["spell1Id"];
                mdm.p6Spell2 = (Int32)mdRecords["participants"][5]["spell2Id"];
                mdm.p7Spell1 = (Int32)mdRecords["participants"][6]["spell1Id"];
                mdm.p7Spell2 = (Int32)mdRecords["participants"][6]["spell2Id"];
                mdm.p8Spell1 = (Int32)mdRecords["participants"][7]["spell1Id"];
                mdm.p8Spell2 = (Int32)mdRecords["participants"][7]["spell2Id"];
                mdm.p9Spell1 = (Int32)mdRecords["participants"][8]["spell1Id"];
                mdm.p9Spell2 = (Int32)mdRecords["participants"][8]["spell2Id"];
                mdm.p10Spell1 = (Int32)mdRecords["participants"][9]["spell1Id"];
                mdm.p10Spell2 = (Int32)mdRecords["participants"][9]["spell2Id"];

                //player champions
                mdm.p1Champ = (Int32)mdRecords["participants"][0]["championId"];
                mdm.p2Champ = (Int32)mdRecords["participants"][1]["championId"];
                mdm.p3Champ = (Int32)mdRecords["participants"][2]["championId"];
                mdm.p4Champ = (Int32)mdRecords["participants"][3]["championId"];
                mdm.p5Champ = (Int32)mdRecords["participants"][4]["championId"];
                mdm.p6Champ = (Int32)mdRecords["participants"][5]["championId"];
                mdm.p7Champ = (Int32)mdRecords["participants"][6]["championId"];
                mdm.p8Champ = (Int32)mdRecords["participants"][7]["championId"];
                mdm.p9Champ = (Int32)mdRecords["participants"][8]["championId"];
                mdm.p10Champ = (Int32)mdRecords["participants"][9]["championId"];

                //player ranks
                mdm.p1Tier = (string)mdRecords["participants"][0]["highestAchievedSeasonTier"];
                mdm.p2Tier = (string)mdRecords["participants"][1]["highestAchievedSeasonTier"];
                mdm.p3Tier = (string)mdRecords["participants"][2]["highestAchievedSeasonTier"];
                mdm.p4Tier = (string)mdRecords["participants"][3]["highestAchievedSeasonTier"];
                mdm.p5Tier = (string)mdRecords["participants"][4]["highestAchievedSeasonTier"];
                mdm.p6Tier = (string)mdRecords["participants"][5]["highestAchievedSeasonTier"];
                mdm.p7Tier = (string)mdRecords["participants"][6]["highestAchievedSeasonTier"];
                mdm.p8Tier = (string)mdRecords["participants"][7]["highestAchievedSeasonTier"];
                mdm.p9Tier = (string)mdRecords["participants"][8]["highestAchievedSeasonTier"];
                mdm.p10Tier = (string)mdRecords["participants"][9]["highestAchievedSeasonTier"];

                //player items - item0-5
                mdm.p1Item1 = (Int32)mdRecords["participants"][0]["stats"]["item0"];
                mdm.p1Item2 = (Int32)mdRecords["participants"][0]["stats"]["item1"];
                mdm.p1Item3 = (Int32)mdRecords["participants"][0]["stats"]["item2"];
                mdm.p1Item4 = (Int32)mdRecords["participants"][0]["stats"]["item3"];
                mdm.p1Item5 = (Int32)mdRecords["participants"][0]["stats"]["item4"];
                mdm.p1Item6 = (Int32)mdRecords["participants"][0]["stats"]["item5"];
                mdm.p2Item1 = (Int32)mdRecords["participants"][1]["stats"]["item0"];
                mdm.p2Item2 = (Int32)mdRecords["participants"][1]["stats"]["item1"];
                mdm.p2Item3 = (Int32)mdRecords["participants"][1]["stats"]["item2"];
                mdm.p2Item4 = (Int32)mdRecords["participants"][1]["stats"]["item3"];
                mdm.p2Item5 = (Int32)mdRecords["participants"][1]["stats"]["item4"];
                mdm.p2Item6 = (Int32)mdRecords["participants"][1]["stats"]["item5"];
                mdm.p3Item1 = (Int32)mdRecords["participants"][2]["stats"]["item0"];
                mdm.p3Item2 = (Int32)mdRecords["participants"][2]["stats"]["item1"];
                mdm.p3Item3 = (Int32)mdRecords["participants"][2]["stats"]["item2"];
                mdm.p3Item4 = (Int32)mdRecords["participants"][2]["stats"]["item3"];
                mdm.p3Item5 = (Int32)mdRecords["participants"][2]["stats"]["item4"];
                mdm.p3Item6 = (Int32)mdRecords["participants"][2]["stats"]["item5"];
                mdm.p4Item1 = (Int32)mdRecords["participants"][3]["stats"]["item0"];
                mdm.p4Item2 = (Int32)mdRecords["participants"][3]["stats"]["item1"];
                mdm.p4Item3 = (Int32)mdRecords["participants"][3]["stats"]["item2"];
                mdm.p4Item4 = (Int32)mdRecords["participants"][3]["stats"]["item3"];
                mdm.p4Item5 = (Int32)mdRecords["participants"][3]["stats"]["item4"];
                mdm.p4Item6 = (Int32)mdRecords["participants"][3]["stats"]["item5"];
                mdm.p5Item1 = (Int32)mdRecords["participants"][4]["stats"]["item0"];
                mdm.p5Item2 = (Int32)mdRecords["participants"][4]["stats"]["item1"];
                mdm.p5Item3 = (Int32)mdRecords["participants"][4]["stats"]["item2"];
                mdm.p5Item4 = (Int32)mdRecords["participants"][4]["stats"]["item3"];
                mdm.p5Item5 = (Int32)mdRecords["participants"][4]["stats"]["item4"];
                mdm.p5Item6 = (Int32)mdRecords["participants"][4]["stats"]["item5"];
                mdm.p6Item1 = (Int32)mdRecords["participants"][5]["stats"]["item0"];
                mdm.p6Item2 = (Int32)mdRecords["participants"][5]["stats"]["item1"];
                mdm.p6Item3 = (Int32)mdRecords["participants"][5]["stats"]["item2"];
                mdm.p6Item4 = (Int32)mdRecords["participants"][5]["stats"]["item3"];
                mdm.p6Item5 = (Int32)mdRecords["participants"][5]["stats"]["item4"];
                mdm.p6Item6 = (Int32)mdRecords["participants"][5]["stats"]["item5"];
                mdm.p7Item1 = (Int32)mdRecords["participants"][6]["stats"]["item0"];
                mdm.p7Item2 = (Int32)mdRecords["participants"][6]["stats"]["item1"];
                mdm.p7Item3 = (Int32)mdRecords["participants"][6]["stats"]["item2"];
                mdm.p7Item4 = (Int32)mdRecords["participants"][6]["stats"]["item3"];
                mdm.p7Item5 = (Int32)mdRecords["participants"][6]["stats"]["item4"];
                mdm.p7Item6 = (Int32)mdRecords["participants"][6]["stats"]["item5"];
                mdm.p8Item1 = (Int32)mdRecords["participants"][7]["stats"]["item0"];
                mdm.p8Item2 = (Int32)mdRecords["participants"][7]["stats"]["item1"];
                mdm.p8Item3 = (Int32)mdRecords["participants"][7]["stats"]["item2"];
                mdm.p8Item4 = (Int32)mdRecords["participants"][7]["stats"]["item3"];
                mdm.p8Item5 = (Int32)mdRecords["participants"][7]["stats"]["item4"];
                mdm.p8Item6 = (Int32)mdRecords["participants"][7]["stats"]["item5"];
                mdm.p9Item1 = (Int32)mdRecords["participants"][8]["stats"]["item0"];
                mdm.p9Item2 = (Int32)mdRecords["participants"][8]["stats"]["item1"];
                mdm.p9Item3 = (Int32)mdRecords["participants"][8]["stats"]["item2"];
                mdm.p9Item4 = (Int32)mdRecords["participants"][8]["stats"]["item3"];
                mdm.p9Item5 = (Int32)mdRecords["participants"][8]["stats"]["item4"];
                mdm.p9Item6 = (Int32)mdRecords["participants"][8]["stats"]["item5"];
                mdm.p10Item1 = (Int32)mdRecords["participants"][9]["stats"]["item0"];
                mdm.p10Item2 = (Int32)mdRecords["participants"][9]["stats"]["item1"];
                mdm.p10Item3 = (Int32)mdRecords["participants"][9]["stats"]["item2"];
                mdm.p10Item4 = (Int32)mdRecords["participants"][9]["stats"]["item3"];
                mdm.p10Item5 = (Int32)mdRecords["participants"][9]["stats"]["item4"];
                mdm.p10Item6 = (Int32)mdRecords["participants"][9]["stats"]["item5"];

                //player kills/deaths/assists
                mdm.p1Kills = (Int32)mdRecords["participants"][0]["stats"]["kills"];
                mdm.p1Deaths = (Int32)mdRecords["participants"][0]["stats"]["deaths"];
                mdm.p1Assists = (Int32)mdRecords["participants"][0]["stats"]["assists"];
                mdm.p2Kills = (Int32)mdRecords["participants"][1]["stats"]["kills"];
                mdm.p2Deaths = (Int32)mdRecords["participants"][1]["stats"]["deaths"];
                mdm.p2Assists = (Int32)mdRecords["participants"][1]["stats"]["assists"];
                mdm.p3Kills = (Int32)mdRecords["participants"][2]["stats"]["kills"];
                mdm.p3Deaths = (Int32)mdRecords["participants"][2]["stats"]["deaths"];
                mdm.p3Assists = (Int32)mdRecords["participants"][2]["stats"]["assists"];
                mdm.p4Kills = (Int32)mdRecords["participants"][3]["stats"]["kills"];
                mdm.p4Deaths = (Int32)mdRecords["participants"][3]["stats"]["deaths"];
                mdm.p4Assists = (Int32)mdRecords["participants"][3]["stats"]["assists"];
                mdm.p5Kills = (Int32)mdRecords["participants"][4]["stats"]["kills"];
                mdm.p5Deaths = (Int32)mdRecords["participants"][4]["stats"]["deaths"];
                mdm.p5Assists = (Int32)mdRecords["participants"][4]["stats"]["assists"];
                mdm.p6Kills = (Int32)mdRecords["participants"][5]["stats"]["kills"];
                mdm.p6Deaths = (Int32)mdRecords["participants"][5]["stats"]["deaths"];
                mdm.p6Assists = (Int32)mdRecords["participants"][5]["stats"]["assists"];
                mdm.p7Kills = (Int32)mdRecords["participants"][6]["stats"]["kills"];
                mdm.p7Deaths = (Int32)mdRecords["participants"][6]["stats"]["deaths"];
                mdm.p7Assists = (Int32)mdRecords["participants"][6]["stats"]["assists"];
                mdm.p8Kills = (Int32)mdRecords["participants"][7]["stats"]["kills"];
                mdm.p8Deaths = (Int32)mdRecords["participants"][7]["stats"]["deaths"];
                mdm.p8Assists = (Int32)mdRecords["participants"][7]["stats"]["assists"];
                mdm.p9Kills = (Int32)mdRecords["participants"][8]["stats"]["kills"];
                mdm.p9Deaths = (Int32)mdRecords["participants"][8]["stats"]["deaths"];
                mdm.p9Assists = (Int32)mdRecords["participants"][8]["stats"]["assists"];
                mdm.p10Kills = (Int32)mdRecords["participants"][9]["stats"]["kills"];
                mdm.p10Deaths = (Int32)mdRecords["participants"][9]["stats"]["deaths"];
                mdm.p10Assists = (Int32)mdRecords["participants"][9]["stats"]["assists"];

                //player levels - champLevel
                mdm.p1Level = (Int32)mdRecords["participants"][0]["stats"]["champLevel"];
                mdm.p2Level = (Int32)mdRecords["participants"][1]["stats"]["champLevel"];
                mdm.p3Level = (Int32)mdRecords["participants"][2]["stats"]["champLevel"];
                mdm.p4Level = (Int32)mdRecords["participants"][3]["stats"]["champLevel"];
                mdm.p5Level = (Int32)mdRecords["participants"][4]["stats"]["champLevel"];
                mdm.p6Level = (Int32)mdRecords["participants"][5]["stats"]["champLevel"];
                mdm.p7Level = (Int32)mdRecords["participants"][6]["stats"]["champLevel"];
                mdm.p8Level = (Int32)mdRecords["participants"][7]["stats"]["champLevel"];
                mdm.p9Level = (Int32)mdRecords["participants"][8]["stats"]["champLevel"];
                mdm.p10Level = (Int32)mdRecords["participants"][9]["stats"]["champLevel"];

                //minion kills - minionsKilled
                mdm.p1CS = (Int32)mdRecords["participants"][0]["stats"]["minionsKilled"];
                mdm.p2CS = (Int32)mdRecords["participants"][1]["stats"]["minionsKilled"];
                mdm.p3CS = (Int32)mdRecords["participants"][2]["stats"]["minionsKilled"];
                mdm.p4CS = (Int32)mdRecords["participants"][3]["stats"]["minionsKilled"];
                mdm.p5CS = (Int32)mdRecords["participants"][4]["stats"]["minionsKilled"];
                mdm.p6CS = (Int32)mdRecords["participants"][5]["stats"]["minionsKilled"];
                mdm.p7CS = (Int32)mdRecords["participants"][6]["stats"]["minionsKilled"];
                mdm.p8CS = (Int32)mdRecords["participants"][7]["stats"]["minionsKilled"];
                mdm.p9CS = (Int32)mdRecords["participants"][8]["stats"]["minionsKilled"];
                mdm.p10CS = (Int32)mdRecords["participants"][9]["stats"]["minionsKilled"];

                //player damage - totalDamageDealt
                mdm.p1Damage = (Int32)mdRecords["participants"][0]["stats"]["totalDamageDealt"];
                mdm.p2Damage = (Int32)mdRecords["participants"][1]["stats"]["totalDamageDealt"];
                mdm.p3Damage = (Int32)mdRecords["participants"][2]["stats"]["totalDamageDealt"];
                mdm.p4Damage = (Int32)mdRecords["participants"][3]["stats"]["totalDamageDealt"];
                mdm.p5Damage = (Int32)mdRecords["participants"][4]["stats"]["totalDamageDealt"];
                mdm.p6Damage = (Int32)mdRecords["participants"][5]["stats"]["totalDamageDealt"];
                mdm.p7Damage = (Int32)mdRecords["participants"][6]["stats"]["totalDamageDealt"];
                mdm.p8Damage = (Int32)mdRecords["participants"][7]["stats"]["totalDamageDealt"];
                mdm.p9Damage = (Int32)mdRecords["participants"][8]["stats"]["totalDamageDealt"];
                mdm.p10Damage = (Int32)mdRecords["participants"][9]["stats"]["totalDamageDealt"];

                mdm.p1Gold = (Int32)mdRecords["participants"][0]["stats"]["goldEarned"];
                mdm.p2Gold = (Int32)mdRecords["participants"][1]["stats"]["goldEarned"];
                mdm.p3Gold = (Int32)mdRecords["participants"][2]["stats"]["goldEarned"];
                mdm.p4Gold = (Int32)mdRecords["participants"][3]["stats"]["goldEarned"];
                mdm.p5Gold = (Int32)mdRecords["participants"][4]["stats"]["goldEarned"];
                mdm.p6Gold = (Int32)mdRecords["participants"][5]["stats"]["goldEarned"];
                mdm.p7Gold = (Int32)mdRecords["participants"][6]["stats"]["goldEarned"];
                mdm.p8Gold = (Int32)mdRecords["participants"][7]["stats"]["goldEarned"];
                mdm.p9Gold = (Int32)mdRecords["participants"][8]["stats"]["goldEarned"];
                mdm.p10Gold = (Int32)mdRecords["participants"][9]["stats"]["goldEarned"];

                MatchDetailsDBContext db = new MatchDetailsDBContext();
                db.MatchDetails.Add(mdm);
                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine("Match added to database");
    }
}
    }

    public class MatchDetailsDBContext : DbContext
    {
        public DbSet<MatchDetailsModel> MatchDetails { get; set; }
    }
}