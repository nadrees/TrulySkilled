using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrulySkilled.Web.Models
{
    public class GameModel
    {
        [Key]
        public int Id { get; set; }

        public String Name { get; set; }
        public double Beta { get; set; }
        public double DrawProbability { get; set; }
        public double DynamicsFactor { get; set; }

        public virtual IEnumerable<PlayerModel> Players { get; set; }
        public virtual IEnumerable<MatchModel> Matches { get; set; }
        public virtual IEnumerable<TeamModel> Teams { get; set; }
    }
}