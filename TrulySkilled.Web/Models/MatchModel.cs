using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrulySkilled.Web.Models
{
    public class MatchModel
    {
        public MatchModel()
        {
            Timestamp = DateTime.Now.ToUniversalTime();
        }

        [Key]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public virtual GameModel Game { get; set; }

        public virtual IEnumerable<TeamModel> Teams { get; set; }
    }
}