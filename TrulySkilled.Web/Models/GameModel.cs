using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrulySkilled.Web.Models
{
    public class GameModel
    {
        public static GameModel NewGame(String name)
        {
            return new GameModel
            {
                Name = name,
                Beta = Defaults.DefaultBeta,
                DrawProbability = Defaults.DefaultDrawProbability,
                DynamicsFactor = Defaults.DefaultDynamicsFactor
            };
        }

        [Key]
        public int Id { get; set; }

        public String Name { get; set; }
        public double Beta { get; set; }
        public double DrawProbability { get; set; }
        public double DynamicsFactor { get; set; }

        public virtual ICollection<PlayerModel> Players { get; set; }
        public virtual ICollection<MatchModel> Matches { get; set; }
        public virtual ICollection<TeamModel> Teams { get; set; }
    }

    public class GameNames
    {
        public const String TicTacToe = "Tic-Tac-Toe";

        public static IEnumerable<String> GameNamesSet = new[] { TicTacToe };
    }
}