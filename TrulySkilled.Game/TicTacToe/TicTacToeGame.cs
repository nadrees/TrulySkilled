using System;
using System.Collections.Generic;
using System.Linq;

namespace TrulySkilled.Game.TicTacToe
{
    public class TicTacToeGame
    {
        public List<Player> Players { get; private set; }

        // using an instance variable as a lock instead of a static variable
        // so that players in different instances of the game will not block
        // each other. However, players playing in the same instance of the 
        // game will block each other since the game is effectively a 
        // singleton to them.
        public object Lock { get; private set; }
        public bool GameHasStarted { get; private set; }
        public int CurrentPlayersTurn { get; private set; }

        private String[,] board = new String[3, 3];

        public TicTacToeGame(IEnumerable<string> players)
        {
            bool isXs = Utils.GetRandomBool();
            bool isFirst = Utils.GetRandomBool();

            this.Players = players.Select(p => 
            {
                var player = new Player()
                {
                    PlayerName = p,
                    PlayerSymbol = isXs ? "X" : "O",
                    PlayerOrder = isFirst ? 0 : 1
                };
                isXs = !isXs;
                isFirst = !isFirst;

                return player;
            })
            .OrderBy(p => p.PlayerOrder)
            .ToList();
            
            this.Lock = new object();
            this.GameHasStarted = false;
            this.CurrentPlayersTurn = 0;
        }

        public void BeginGame()
        {
            GameHasStarted = true;
        }
    }

    public class Player
    {
        public string PlayerName { get; internal set; }
        public bool IsReady { get; set; }

        public string PlayerSymbol { get; internal set; }
        public int PlayerOrder { get; internal set; }
    }
}
