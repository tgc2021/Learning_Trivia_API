using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Learning_Trivia_API.Models
{
    public class LeaderboardResult
    {
        //public string UserId { get; set; }
        //public int PointsScored { get; set; }
        //public string Name { get; set; }
        //public int Rank { get; set; }
        public List<dynamic> TopUsers { get; set; } = new List<dynamic>();
    }
}