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
        private int[][] winningIndexes;

        public bool GameHasFinished
        {
            get
            {
                return Players.Any(p => p.Rank != null);
            }
        }

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

        public String[,] GetCurrentBoard()
        {
            return board;
        }

        public bool TrySubmitMove(int x, int y, Player player, out String errorMessage)
        {
            // check to ensure this mesage is from the player whos turn it is
            var currentPlayer = Players[CurrentPlayersTurn];
            if (currentPlayer != player)
            {
                errorMessage = "It is not your turn.";
                return false;
            }

            // player who submitted the move is the current player
            // validate move is legal
            if (board[x, y] != null)
            {
                errorMessage = "You cannot place a piece there. Choose an empty square";
                return false;
            }

            // move is legal
            board[x, y] = player.PlayerSymbol;

            // check for win conditions
            if (board[x, 0] == player.PlayerSymbol && board[x, 0] == board[x, 1] 
                && board[x, 0] == board[x, 2]) // check row
            {
                player.Rank = 1;
                Players.First(p => p != player).Rank = 2;
                winningIndexes = new int[][]
                {
                    new[] { x, 0 },
                    new[] { x, 1 },
                    new[] { x, 2 }
                };
            }
            else if (board[0, y] == player.PlayerSymbol && board[0, y] == board[1, y] &&
                board[2, y] == board[0, y]) // check column
            {
                player.Rank = 1;
                Players.First(p => p != player).Rank = 2;
                winningIndexes = new int[][]
                {
                    new[] { 0, y },
                    new[] { 1, y },
                    new[] { 2, y }
                };
            }
            else if (board[0, 0] == player.PlayerSymbol && board[0, 0] == board[1, 1] &&
                board[0, 0] == board[2, 2]) // check diag
            {
                player.Rank = 1;
                Players.First(p => p != player).Rank = 2;
                winningIndexes = new int[][]
                {
                    new[] { 0, 0 },
                    new[] { 1, 1 },
                    new[] { 2, 2 }
                };
            }
            else if (board[2, 0] == player.PlayerSymbol && board[2, 0] == board[1, 1] &&
                board[2, 0] == board[0, 2]) // check other diag
            {
                player.Rank = 1;
                Players.First(p => p != player).Rank = 2;
                winningIndexes = new int[][]
                {
                    new[] { 2, 0 },
                    new[] { 1, 1 },
                    new[] { 2, 0 }
                };
            }
            else
            {
                // check for draw condition
                bool isDraw = true;
                for (int i = 0; i < 3 && isDraw; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board[i, j] == null)
                        {
                            isDraw = false;
                            break;
                        }
                    }
                }

                if (isDraw)
                {
                    foreach (var p in Players)
                        p.Rank = 1;
                }
            }

            CurrentPlayersTurn = (CurrentPlayersTurn + 1) % Players.Count;
            errorMessage = null;
            return true;
        }

        public int[][] GetWinningLine()
        {
            return winningIndexes;
        }
    }

    public class Player
    {
        public string PlayerName { get; internal set; }
        public bool IsReady { get; set; }

        public string PlayerSymbol { get; internal set; }
        public int PlayerOrder { get; internal set; }
        public int? Rank { get; set; }
    }
}
