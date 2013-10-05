using System;
using System.Collections.Concurrent;
using TrulySkilled.Game.TicTacToe;

namespace TrulySkilled.Web.Hubs
{
    public class TicTacToeHub : ChatHub
    {
        public static ConcurrentDictionary<Guid, TicTacToeGame> GamesInProgress = 
            new ConcurrentDictionary<Guid, TicTacToeGame>();
    }
}