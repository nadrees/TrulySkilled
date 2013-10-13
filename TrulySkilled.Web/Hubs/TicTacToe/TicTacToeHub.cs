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
            TicTacToeGame game;
            if (GamesInProgress.TryGetValue(gameId, out game))
            {
                await Groups.Add(Context.ConnectionId, GetGroupName(gameId));

                lock (game.Lock)
                {
                    game.Players.First(p => p.PlayerName == Context.GetCurrentUserName()).IsReady = true;
                    var playersNotReady = game.Players.Where(p => !p.IsReady).ToList();
                    if (playersNotReady.Any())
                        Clients.Group(GetGroupName(gameId)).SetAwaitingPlayers(playersNotReady.Select(p => p.PlayerName).ToArray());
                    else
                    {
                        game.BeginGame();
                        Clients.Group(GetGroupName(gameId)).BeginGame(game.Players.Select(p => new 
                        {
                            Player = p.PlayerName,
                            Order = p.PlayerOrder,
                            Symbol = p.PlayerSymbol
                        }).ToArray());
                    }
                }
            }
        }

        private String GetGroupName(Guid gameId)
        {
            return String.Format("Tic-Tac-Toe-{0}", gameId);
        }
    }
}