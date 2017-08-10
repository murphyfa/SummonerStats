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
using System.Text;

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

        JObject champInfo = null;

        public long lastMatchPulled;

        //delegate for pulling match history async during first pull because it takes a while
        public delegate void UpdateMatchesAsync(int sumID, string sumName, long accountId);

        //this is for the initial pull on a player to pull 10 games and load the page to show them data quickly while the rest download async
        public void FirstUpdate(int sumID, string sumName, long accountId)
        {
            MatchHistoryDBContext db = new MatchHistoryDBContext();

            string mhURL = "https://na1.api.riotgames.com/lol/match/v3/matchlists/by-account/" + accountId + "?season=8&api_key=RGAPI-62b5d58e-b1bf-4667-be65-b901aa5e89cc";

            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                string mhData = null;

                bool retry = true;
                while (retry)
                {
                    try
                    {
                        mhData = client.DownloadString(mhURL);
                        retry = false;
                    }
                    catch (WebException we)
                    {
                        var response = ((HttpWebResponse)we.Response).StatusCode;
                        if ((int)response == 429)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                        else
                        {
                            retry = false;
                        }
                        System.Diagnostics.Debug.WriteLine("MATCH HISTORY RESPONDED WITH STATUS " + response);
                    }
                }
                JObject mhRecords = JObject.Parse(mhData);

                if ((Int32)mhRecords["matches"].Count() > 0)
                {
                    //pull 10 most recent games or all if less than 10 in results
                    int n;
                    if ((int)mhRecords["matches"].Count() >= 10)
                    {
                        n = 10;
                    }
                    else
                    {
                        n = (int)mhRecords["matches"].Count();
                    }

                    for (int i = 0; i < n; i++)
                    {
                        tblMatchHistory mhm = new tblMatchHistory();
                        tblMatchDetails mdm = new tblMatchDetails();
                        ProfileController pc = new ProfileController();
                        long matchID = (Int64)mhRecords["matches"][i]["gameId"];
                        long lastTimestamp = (Int64)mhRecords["matches"][i]["timestamp"];

                        if (db.MatchHistory.Where(u => u.id == sumID && u.timestamp == lastTimestamp).ToList().Count() == 0)
                        {
                            IEnumerable<tblMatchDetails> matchDetails = mdm.PullMatch(matchID);
                            int[] playerStats = pc.FindViewPlayer(matchDetails, accountId);

                            mhm.id = sumID;
                            mhm.timestamp = (Int64)mhRecords["matches"][i]["timestamp"];
                            mhm.champion = (Int32)mhRecords["matches"][i]["champion"];
                            mhm.region = (string)mhRecords["matches"][i]["platformId"];
                            mhm.queue = (string)mhRecords["matches"][i]["queue"];
                            mhm.season = (string)mhRecords["matches"][i]["season"];
                            mhm.matchId = (Int64)mhRecords["matches"][i]["gameId"];
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
                    }
                }
            }
        }

        public void UpdateMatchHistory(int sumID, string sumName, long accountId, bool firstUpdate)
        {
            MatchHistoryDBContext db = new MatchHistoryDBContext();

            string mhURL;
            string apiKey = "RGAPI-62b5d58e-b1bf-4667-be65-b901aa5e89cc";

            //if we have no matches stored, start at beginning of season 7, otherwise only since most recent
            if (firstUpdate == true)
            {
                mhURL = "https://na1.api.riotgames.com/lol/match/v3/matchlists/by-account/" + accountId + "?season=8&api_key=" + apiKey;
            }
            else
            {
                long lastMatch = db.MatchHistory.OrderByDescending(u => u.timestamp).Where(u => u.id == sumID).Select(u => u.timestamp).First();
                mhURL = "https://na1.api.riotgames.com/lol/match/v3/matchlists/by-account/" + accountId + "?beginTime=" + lastMatch + "&api_key=" + apiKey;
            }

            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                string mhData = null;

                bool retry = true;
                while (retry)
                {
                    try
                    {
                        mhData = client.DownloadString(mhURL);
                        retry = false;
                    }
                    catch (WebException we)
                    {
                        var response = ((HttpWebResponse)we.Response).StatusCode;
                        if ((int)response == 429)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                        else
                        {
                            retry = false;
                        }
                        System.Diagnostics.Debug.WriteLine("MATCH HISTORY RESPONDED WITH STATUS " + response);
                    }
                }
                JObject mhRecords = JObject.Parse(mhData);

                if ((Int32)mhRecords["matches"].Count() > 0)
                {
                    for (int i = 0; i < mhRecords["matches"].Count(); i++)
                    {
                        tblMatchHistory mhm = new tblMatchHistory();
                        tblMatchDetails mdm = new tblMatchDetails();
                        ProfileController pc = new ProfileController();
                        long matchID = (Int64)mhRecords["matches"][i]["gameId"];
                        long lastTimestamp = (Int64)mhRecords["matches"][i]["timestamp"];

                        if (db.MatchHistory.Where(u => u.id == sumID && u.timestamp == lastTimestamp).ToList().Count() == 0)
                        {
                            IEnumerable<tblMatchDetails> matchDetails = mdm.PullMatch(matchID);
                            int[] playerStats = pc.FindViewPlayer(matchDetails, accountId);

                            mhm.id = sumID;
                            mhm.timestamp = (Int64)mhRecords["matches"][i]["timestamp"];
                            mhm.champion = (Int32)mhRecords["matches"][i]["champion"];
                            mhm.region = (string)mhRecords["matches"][i]["platformId"];
                            mhm.queue = (string)mhRecords["matches"][i]["queue"];
                            mhm.season = (string)mhRecords["matches"][i]["season"];
                            mhm.matchId = (Int64)mhRecords["matches"][i]["gameId"];
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
                    }
                }
            }

        }

        public void TopChamps(long summonerID)
        {
            MatchHistoryDBContext db = new MatchHistoryDBContext();
            tblMatchHistory mh = new tblMatchHistory();
            var recentGamesList = db.MatchHistory.Where(i => i.id == summonerID).OrderByDescending(t => t.timestamp).Take(20);

            var champsList = new List<int>();
            foreach (var game in recentGamesList)
            {
                champsList.Add(game.champion);
            }

            var topChamps = champsList.GroupBy(c => c).Select(g => new { champ = g.Key, count = g.Count() }).OrderByDescending(g => g.count).ToList();
            var topChampsList = new List<Tuple<int, int>>();

            foreach (var champ in topChamps)
            {
                topChampsList.Add(Tuple.Create(champ.champ, champ.count));
            }

            string[] topFiveNames = new string[5];

            ProfileController pc = new ProfileController();
            int n = 0;
            foreach (var champ in topChampsList)
            {
                while (n < topChampsList.Count() && n <= 4)
                {
                    if (champ.Item1 != 0)
                    {
                        topFiveNames[n] = pc.ChampById(champ.Item1);
                        n++;
                    }
                }
            }

            if (topChampsList.Count() >= 1)
            {
                int champOne = topChampsList[0].Item1;
                var totalGamesPlayedOne = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champOne);
                if (totalGamesPlayedOne.Count() > 0)
                {
                    if (topFiveNames[0] == "Fiddlesticks")
                    {
                        topChampBG = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/FiddleSticks_0.jpg";
                    }
                    else if (topFiveNames[0] == "Kha'Zix")
                    {
                        topChampBG = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/Khazix_0.jpg";
                    }
                    else if (topFiveNames[0] == "Vel'Koz")
                    {
                        topChampBG = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/Velkoz_0.jpg";
                    }
                    else if (topFiveNames[0] == "Wukong")
                    {
                        topChampBG = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/MonkeyKing_0.jpg";
                    }
                    else
                    {
                        topChampBG = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + topFiveNames[0].Replace(" ", "") + "_0.jpg";
                    }

                    var detailsOne = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champOne).GroupBy(s => 1).Select(s => new
                    {
                        killsOne = s.Sum(x => x.kills),
                        deathsOne = s.Sum(x => x.deaths),
                        assistsOne = s.Sum(x => x.assists),
                        winsOne = s.Sum(x => x.winner)
                    });
                    topChampOne = pc.ChampById(champOne);
                    totalGamesOne = totalGamesPlayedOne.Count();
                    killsOne = (Int32)detailsOne.First().killsOne / totalGamesOne;
                    deathsOne = (Int32)detailsOne.First().deathsOne / totalGamesOne;
                    assistsOne = (Int32)detailsOne.First().assistsOne / totalGamesOne;
                    winsOne = (Int32)detailsOne.First().winsOne;
                    lossesOne = totalGamesOne - winsOne;
                    winrateOne = Math.Round((double)winsOne * 100 / totalGamesOne).ToString() + "%";
                }
            }

            if (topChampsList.Count() >= 2)
            {
                int champTwo = topChampsList[1].Item1;
                var totalGamesPlayedTwo = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champTwo);
                if (totalGamesPlayedTwo.Count() > 0)
                {
                    var detailsTwo = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champTwo).GroupBy(s => 1).Select(s => new
                    {
                        killsTwo = s.Sum(x => x.kills),
                        deathsTwo = s.Sum(x => x.deaths),
                        assistsTwo = s.Sum(x => x.assists),
                        winsTwo = s.Sum(x => x.winner)
                    });
                    topChampTwo = pc.ChampById(champTwo);
                    totalGamesTwo = totalGamesPlayedTwo.Count();
                    killsTwo = (Int32)detailsTwo.First().killsTwo / totalGamesTwo;
                    deathsTwo = (Int32)detailsTwo.First().deathsTwo / totalGamesTwo;
                    assistsTwo = (Int32)detailsTwo.First().assistsTwo / totalGamesTwo;
                    winsTwo = (Int32)detailsTwo.First().winsTwo;
                    lossesTwo = totalGamesTwo - winsTwo;
                    winrateTwo = Math.Round((double)winsTwo * 100 / totalGamesTwo).ToString() + "%";
                }
            }

            if (topChampsList.Count() >= 3)
            {
                int champThree = topChampsList[2].Item1;
                var totalGamesPlayedThree = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champThree);
                if (totalGamesPlayedThree.Count() > 0)
                {
                    var detailsThree = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champThree).GroupBy(s => 1).Select(s => new
                    {
                        killsThree = s.Sum(x => x.kills),
                        deathsThree = s.Sum(x => x.deaths),
                        assistsThree = s.Sum(x => x.assists),
                        winsThree = s.Sum(x => x.winner)
                    });
                    topChampThree = pc.ChampById(champThree);
                    totalGamesThree = totalGamesPlayedThree.Count();
                    killsThree = (Int32)detailsThree.First().killsThree / totalGamesThree;
                    deathsThree = (Int32)detailsThree.First().deathsThree / totalGamesThree;
                    assistsThree = (Int32)detailsThree.First().assistsThree / totalGamesThree;
                    winsThree = (Int32)detailsThree.First().winsThree;
                    lossesThree = totalGamesThree - winsThree;
                    winrateThree = Math.Round((double)winsThree * 100 / totalGamesThree).ToString() + "%";
                }
            }

            if (topChampsList.Count() >= 4)
            {
                int champFour = topChampsList[3].Item1;
                var totalGamesPlayedFour = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champFour);
                if (totalGamesPlayedFour.Count() > 0)
                {
                    var detailsFour = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champFour).GroupBy(s => 1).Select(s => new
                    {
                        killsFour = s.Sum(x => x.kills),
                        deathsFour = s.Sum(x => x.deaths),
                        assistsFour = s.Sum(x => x.assists),
                        winsFour = s.Sum(x => x.winner)
                    });
                    topChampFour = pc.ChampById(champFour);
                    totalGamesFour = totalGamesPlayedFour.Count();
                    killsFour = (Int32)detailsFour.First().killsFour / totalGamesFour;
                    deathsFour = (Int32)detailsFour.First().deathsFour / totalGamesFour;
                    assistsFour = (Int32)detailsFour.First().assistsFour / totalGamesFour;
                    winsFour = (Int32)detailsFour.First().winsFour;
                    lossesFour = totalGamesFour - winsFour;
                    winrateFour = Math.Round((double)winsFour * 100 / totalGamesFour).ToString() + "%";
                }
            }

            if (topChampsList.Count() >= 5)
            {
                int champFive = topChampsList[4].Item1;
                var totalGamesPlayedFive = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champFive);
                if (totalGamesPlayedFive.Count() > 0)
                {
                    var detailsFive = db.MatchHistory.Where(s => s.id == summonerID && s.champion == champFive).GroupBy(s => 1).Select(s => new
                    {
                        killsFive = s.Sum(x => x.kills),
                        deathsFive = s.Sum(x => x.deaths),
                        assistsFive = s.Sum(x => x.assists),
                        winsFive = s.Sum(x => x.winner)
                    });
                    topChampFive = pc.ChampById(champFive);
                    totalGamesFive = totalGamesPlayedFive.Count();
                    killsFive = (Int32)detailsFive.First().killsFive / totalGamesFive;
                    deathsFive = (Int32)detailsFive.First().deathsFive / totalGamesFive;
                    assistsFive = (Int32)detailsFive.First().assistsFive / totalGamesFive;
                    winsFive = (Int32)detailsFive.First().winsFive;
                    lossesFive = totalGamesFive - winsFive;
                    winrateFive = Math.Round((double)winsFive * 100 / totalGamesFive).ToString() + "%";
                }
            }
        }



        //old top 5 champs method. the api is being deprecated so this data is now pulled from the database rather than the api
        public void Top5Champs(int sumID)
        {
            using (var client = new WebClient())
            {
                //stats from ranked games
                //currently only looking at season 7
                string statsURL = "https://na.api.pvp.net/api/lol/na/v1.3/stats/by-summoner/" + sumID + "/ranked?season=SEASON2017&api_key=RGAPI-62b5d58e-b1bf-4667-be65-b901aa5e89cc";

                try
                {
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

                    //for (int i = 0; i < 5; i++)
                    //{
                    //    string staticChampInfo = "https://global.api.pvp.net/api/lol/static-data/na/v1.2/champion/" + champsPlayed[i].Item1 + "?api_key=" + key;
                    //    string champData = client.DownloadString(staticChampInfo);
                    //    JObject champInfo = JObject.Parse(champData);
                    //    topFiveNames[i] = (string)champInfo["name"];
                    //}

                    int n = 0;
                    foreach (Tuple<int, int> champ in champsPlayed)
                    {
                        while (n < champsPlayed.Count() && n <= 4)
                        {
                            if (champsPlayed[n].Item1 != 0)
                            {

                                if (champInfo == null)
                                {
                                    string staticChampInfo = "https://global.api.pvp.net/api/lol/static-data/na/v1.2/champion?dataById=true&api_key=RGAPI-62b5d58e-b1bf-4667-be65-b901aa5e89cc";
                                    string champData = client.DownloadString(staticChampInfo);
                                    champInfo = JObject.Parse(champData);
                                }

                                topFiveNames[n] = (string)champInfo["data"][champ.Item1.ToString()]["name"];
                                n++;
                            }
                        }
                    }

                    ProfileController pc = new ProfileController();
                    MatchHistoryDBContext db = new MatchHistoryDBContext();

                    if (champsPlayed.Count() >= 1)
                    {
                        int champOne = champsPlayed[0].Item1;
                        if (db.MatchHistory.Where(s => s.id == sumID && s.champion == champOne).Count() > 0)
                        {
                            if (topFiveNames[0] == "Fiddlesticks")
                            {
                                topChampBG = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/FiddleSticks_0.jpg";
                            }
                            else
                            {
                                topChampBG = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + topFiveNames[0].Replace(" ", "") + "_0.jpg";
                            }

                            var detailsOne = db.MatchHistory.Where(s => s.id == sumID && s.champion == champOne).GroupBy(s => 1).Select(s => new
                            {
                                killsOne = s.Sum(x => x.kills),
                                deathsOne = s.Sum(x => x.deaths),
                                assistsOne = s.Sum(x => x.assists),
                                winsOne = s.Sum(x => x.winner)
                            });
                            topChampOne = pc.ChampById(champOne);
                            totalGamesOne = champsPlayed[0].Item2;
                            killsOne = (Int32)detailsOne.First().killsOne / totalGamesOne;
                            deathsOne = (Int32)detailsOne.First().deathsOne / totalGamesOne;
                            assistsOne = (Int32)detailsOne.First().assistsOne / totalGamesOne;
                            winsOne = (Int32)detailsOne.First().winsOne;
                            lossesOne = totalGamesOne - winsOne;
                            winrateOne = Math.Round((double)winsOne * 100 / totalGamesOne).ToString() + "%";
                        }
                    }

                    if (champsPlayed.Count() >= 2)
                    {
                        int champTwo = champsPlayed[1].Item1;
                        if (db.MatchHistory.Where(s => s.id == sumID && s.champion == champTwo).Count() > 0)
                        {
                            var detailsTwo = db.MatchHistory.Where(s => s.id == sumID && s.champion == champTwo).GroupBy(s => 1).Select(s => new
                            {
                                killsTwo = s.Sum(x => x.kills),
                                deathsTwo = s.Sum(x => x.deaths),
                                assistsTwo = s.Sum(x => x.assists),
                                winsTwo = s.Sum(x => x.winner)
                            });
                            topChampTwo = pc.ChampById(champTwo);
                            totalGamesTwo = champsPlayed[1].Item2;
                            killsTwo = (Int32)detailsTwo.First().killsTwo / totalGamesTwo;
                            deathsTwo = (Int32)detailsTwo.First().deathsTwo / totalGamesTwo;
                            assistsTwo = (Int32)detailsTwo.First().assistsTwo / totalGamesTwo;
                            winsTwo = (Int32)detailsTwo.First().winsTwo;
                            lossesTwo = totalGamesTwo - winsTwo;
                            winrateTwo = Math.Round((double)winsTwo * 100 / totalGamesTwo).ToString() + "%";
                        }
                    }

                    if (champsPlayed.Count() >= 3)
                    {
                        int champThree = champsPlayed[2].Item1;
                        if (db.MatchHistory.Where(s => s.id == sumID && s.champion == champThree).Count() > 0)
                        {
                            var detailsThree = db.MatchHistory.Where(s => s.id == sumID && s.champion == champThree).GroupBy(s => 1).Select(s => new
                            {
                                killsThree = s.Sum(x => x.kills),
                                deathsThree = s.Sum(x => x.deaths),
                                assistsThree = s.Sum(x => x.assists),
                                winsThree = s.Sum(x => x.winner)
                            });
                            topChampThree = pc.ChampById(champThree);
                            totalGamesThree = champsPlayed[2].Item2;
                            killsThree = (Int32)detailsThree.First().killsThree / totalGamesThree;
                            deathsThree = (Int32)detailsThree.First().deathsThree / totalGamesThree;
                            assistsThree = (Int32)detailsThree.First().assistsThree / totalGamesThree;
                            winsThree = (Int32)detailsThree.First().winsThree;
                            lossesThree = totalGamesThree - winsThree;
                            winrateThree = Math.Round((double)winsThree * 100 / totalGamesThree).ToString() + "%";
                        }
                    }

                    if (champsPlayed.Count() >= 4)
                    {
                        int champFour = champsPlayed[3].Item1;
                        if (db.MatchHistory.Where(s => s.id == sumID && s.champion == champFour).Count() > 0)
                        {
                            var detailsFour = db.MatchHistory.Where(s => s.id == sumID && s.champion == champFour).GroupBy(s => 1).Select(s => new
                            {
                                killsFour = s.Sum(x => x.kills),
                                deathsFour = s.Sum(x => x.deaths),
                                assistsFour = s.Sum(x => x.assists),
                                winsFour = s.Sum(x => x.winner)
                            });
                            topChampFour = pc.ChampById(champFour);
                            totalGamesFour = champsPlayed[3].Item2;
                            killsFour = (Int32)detailsFour.First().killsFour / totalGamesFour;
                            deathsFour = (Int32)detailsFour.First().deathsFour / totalGamesFour;
                            assistsFour = (Int32)detailsFour.First().assistsFour / totalGamesFour;
                            winsFour = (Int32)detailsFour.First().winsFour;
                            lossesFour = totalGamesFour - winsFour;
                            winrateFour = Math.Round((double)winsFour * 100 / totalGamesFour).ToString() + "%";
                        }
                    }

                    if (champsPlayed.Count() >= 5)
                    {
                        int champFive = champsPlayed[4].Item1;
                        if (db.MatchHistory.Where(s => s.id == sumID && s.champion == champFive).Count() > 0)
                        {
                            var detailsFive = db.MatchHistory.Where(s => s.id == sumID && s.champion == champFive).GroupBy(s => 1).Select(s => new
                            {
                                killsFive = s.Sum(x => x.kills),
                                deathsFive = s.Sum(x => x.deaths),
                                assistsFive = s.Sum(x => x.assists),
                                winsFive = s.Sum(x => x.winner)
                            });
                            topChampFive = pc.ChampById(champFive);
                            totalGamesFive = champsPlayed[4].Item2;
                            killsFive = (Int32)detailsFive.First().killsFive / totalGamesFive;
                            deathsFive = (Int32)detailsFive.First().deathsFive / totalGamesFive;
                            assistsFive = (Int32)detailsFive.First().assistsFive / totalGamesFive;
                            winsFive = (Int32)detailsFive.First().winsFive;
                            lossesFive = totalGamesFive - winsFive;
                            winrateFive = Math.Round((double)winsFive * 100 / totalGamesFive).ToString() + "%";
                        }
                    }
                }
                catch (WebException we)
                {
                    var response = ((HttpWebResponse)we.Response).StatusCode;
                    System.Diagnostics.Debug.WriteLine("PULLING CHAMP HISTORY RETURNED STATUS CODE " + response);
                }
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
