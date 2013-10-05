using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using TrulySkilled.Game.TicTacToe;

namespace TrulySkilled.Web.Hubs.TicTacToe
{
    public class TicTacToeHub : ChatHub
    {
        public static ConcurrentDictionary<Guid, TicTacToeGame> GamesInProgress = 
            new ConcurrentDictionary<Guid, TicTacToeGame>();

        public void UserArrived(Guid gameId, int playerId)
        {
            TicTacToeGame game;
            if (GamesInProgress.TryGetValue(gameId, out game))
            {
                
            }
        }
    }
}