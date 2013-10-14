using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using TrulySkilled.Game.TicTacToe;

namespace TrulySkilled.Web.Hubs.TicTacToe
{
    public class TicTacToeGameHub : ChatHub
    {
        public static readonly ConcurrentDictionary<Guid, TicTacToeGame> GamesInProgress =
            new ConcurrentDictionary<Guid, TicTacToeGame>();
        protected static readonly ConcurrentDictionary<String, Guid> ConnectionIdToGameIdMap =
            new ConcurrentDictionary<string, Guid>();

        public async Task UserArrived(Guid gameId)
        {
            var game = GetGame(gameId);
            if (game != null)
            {
                ConnectionIdToGameIdMap.TryAdd(Context.ConnectionId, gameId);
                await Groups.Add(Context.ConnectionId, GetGroupName(gameId));

                lock (game.Lock)
                {
                    if (!game.GameHasStarted)
                    {
                        game.Players.First(p => p.PlayerName == Context.GetCurrentUserName()).IsReady = true;
                        var playersNotReady = game.Players.Where(p => !p.IsReady).ToList();
                        if (playersNotReady.Any())
                            Clients.Group(GetGroupName(gameId)).SetAwaitingPlayers(playersNotReady.Select(p => p.PlayerName).ToArray());
                        else
                        {
                            String groupName = GetGroupName(gameId);

                            game.BeginGame();
                            Clients.Group(groupName).BeginGame(game.Players.Select(p => new
                            {
                                Player = p.PlayerName,
                                Order = p.PlayerOrder,
                                Symbol = p.PlayerSymbol
                            }).ToArray());
                            Clients.Group(groupName).SetPlayerTurn(game.Players[game.CurrentPlayersTurn].PlayerName);
                        }
                    }
                    else
                    {
                        var currentState = new
                        {
                            Board = game.GetCurrentBoard(),
                            Symbol = game.Players.First(p => p.PlayerName == Context.GetCurrentUserName()).PlayerSymbol,
                            CurrentTurn = game.Players[game.CurrentPlayersTurn].PlayerName
                        };
                        Clients.Caller.RestoreState(currentState);
                    }
                }
            }
        }

        public void SquareClicked(int x, int y, String symbol)
        {
            Guid gameId;
            if (ConnectionIdToGameIdMap.TryGetValue(Context.ConnectionId, out gameId))
            {
                var game = GetGame(gameId);
                if (game != null)
                {
                    lock (game.Lock)
                    {
                        String errorMessage;
                        if (game.TrySubmitMove(x, y, game.Players.First(p => p.PlayerSymbol == symbol), out errorMessage))
                        {
                            String groupName = GetGroupName(gameId);

                            Clients.Group(groupName).UpdateBoard(game.GetCurrentBoard());
                            if (game.GameHasFinished)
                            {
                                var finalRank = game.Players.OrderBy(p => p.Rank).ToList();
                                if (finalRank[0].Rank == finalRank[1].Rank)
                                    Clients.Group(groupName).SetDraw();
                                else
                                {
                                    Clients.Group(groupName).SetWinner(new
                                    {
                                        Name = finalRank[0].PlayerName,
                                        WinningLine = game.GetWinningLine()
                                    });
                                }
                            }
                            else
                                Clients.Group(groupName).SetPlayerTurn(game.Players[game.CurrentPlayersTurn].PlayerName);
                        }
                        else
                        {
                            Clients.Caller.SetErrorMessage(errorMessage);
                        }
                    }
                }
            }
        }

        private TicTacToeGame GetGame(Guid gameId)
        {
            TicTacToeGame game;
            if (GamesInProgress.TryGetValue(gameId, out game))
            {
                if (game.Players.Any(p => p.PlayerName == Context.GetCurrentUserName()))
                    return game;
            }

            return null;
        }

        private String GetGroupName(Guid gameId)
        {
            return String.Format("Tic-Tac-Toe-{0}", gameId);
        }

        #region Lifetime Events
        public override Task OnDisconnected()
        {
            Guid _;
            ConnectionIdToGameIdMap.TryRemove(Context.ConnectionId, out _);

            return base.OnDisconnected();
        }
        #endregion
    }
}