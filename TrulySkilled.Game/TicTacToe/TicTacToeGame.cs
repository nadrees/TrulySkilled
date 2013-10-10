using System.Collections.Generic;
using System.Linq;

namespace TrulySkilled.Game.TicTacToe
{
    public class TicTacToeGame
    {
        public HashSet<Player> Players { get; private set; }

        // using an instance variable as a lock instead of a static variable
        // so that players in different instances of the game will not block
        // each other. However, players playing in the same instance of the 
        // game will block each other since the game is effectively a 
        // singleton to them.
        public object Lock { get; private set; }

        public TicTacToeGame(IEnumerable<string> players)
        {
            this.Players = new HashSet<Player>(players.Select(p => new Player() { PlayerName = p }));
            this.Lock = new object();
        }

        /// <summary>
        /// Signals that a player is ready to begin the game.
        /// </summary>
        /// <param name="playerName">The player who is ready to begin the game</param>
        /// <returns>A list of players who have not yet signaled they are ready to begin the game.</returns>
        public IEnumerable<string> SetPlayerReady(string playerName)
        {
            var player = Players.FirstOrDefault(p => p.PlayerName == playerName);
            if (player != null)
                player.IsReady = true;

            return (from p in Players
                    where !p.IsReady
                    select p.PlayerName)
                    .ToList();
        }

        /// <summary>
        /// Sets if a player has left a game.
        /// </summary>
        /// <param name="playerName">The player being updated</param>
        /// <param name="hasLeftGame">True if the player has left the game, false if they've
        /// rejoined in an appropriate amount of time. (For handling network hiccups and page
        /// refreshes)</param>
        /// <returns>True if only one player is left in the game, false otherwise</returns>
        public bool SetPlayerLeftGame(string playerName, bool hasLeftGame)
        {
            var player = Players.FirstOrDefault(p => p.PlayerName == playerName);
            if (player != null)
                player.PlayerLeft = hasLeftGame;

            return Players.Count(p => !p.PlayerLeft) == 1;
        }
    }

    public class Player
    {
        public string PlayerName { get; set; }
        public bool IsReady { get; set; }
        public bool PlayerLeft { get; set; }
    }
}
