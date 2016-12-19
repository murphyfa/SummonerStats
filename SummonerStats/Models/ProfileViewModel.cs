using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummonerStats.Models
{
    public class ProfileViewModel
    {
        public PlayerProfileModel playerProfile = new PlayerProfileModel();
        public List<MatchHistoryModel> matchHistory { get; set;}
    }
}