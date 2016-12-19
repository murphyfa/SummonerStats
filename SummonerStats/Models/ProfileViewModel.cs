using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummonerStats.Models
{
    public class ProfileViewModel
    {
        public PlayerProfile playerProfile = new PlayerProfile();
        public MatchHistoryModel matchHistory = new MatchHistoryModel();
    }
}