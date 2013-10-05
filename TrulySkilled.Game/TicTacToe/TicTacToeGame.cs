using System.Collections.Generic;

namespace TrulySkilled.Game.TicTacToe
{
    public class TicTacToeGame
    {
        private HashSet<int> playerIds;

        public TicTacToeGame(int playerId)
        {
            playerIds = new HashSet<int> { playerId };
        }

        public void AddPlayer(int playerId)
        {
            playerIds.Add(playerId);
        }
    }

    public class Player
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}
