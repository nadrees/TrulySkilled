using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrulySkilled.Web.Models
{
    public class PlayerModel
    {
        public PlayerModel()
        {
            Mean = 25.0;
            StandardDeviation = Mean / 3.0;
        }

        [Key]
        public int Key { get; set; }

        public double Mean { get; set; }
        public double StandardDeviation { get; set; }

        public virtual GameModel Game { get; set; }
        public virtual UserProfile User { get; set; }

        public virtual IEnumerable<TeamModel> Teams { get; set; }
    }
}