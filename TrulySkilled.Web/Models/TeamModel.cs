using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrulySkilled.Web.Models
{
    public class TeamModel
    {
        [Key]
        public int Id { get; set; }

        public int Position { get; set; }

        public virtual GameModel Game { get; set; }

        public virtual IEnumerable<PlayerModel> Players { get; set; }
        public virtual IEnumerable<MatchModel> Matches { get; set; }
    }
}