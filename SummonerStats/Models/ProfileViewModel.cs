using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummonerStats.Models
{
    public class ProfileViewModel
    {
        public PlayerProfileModel playerProfile = new PlayerProfileModel();
        public tblMatchHistory mh = new tblMatchHistory();
        public List<tblMatchHistory> matchHistory { get; set; }
        public IEnumerable<tblMatchDetails> matchDetails { get; set; }
    }
}