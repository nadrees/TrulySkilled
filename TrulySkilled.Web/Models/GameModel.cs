using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    public class GameNames
    {
        public const String TicTacToe = "Tic-Tac-Toe";

        public static IEnumerable<String> GameNamesSet = new[] { TicTacToe };
    }
}