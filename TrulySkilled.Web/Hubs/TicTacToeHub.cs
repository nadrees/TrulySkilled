using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrulySkilled.Game.TicTacToe;

namespace TrulySkilled.Web.Hubs
{
    public class TicTacToeHub : ChatHub
    {
        public static ConcurrentDictionary<Guid, TicTacToeGame> GamesInProgress = 
            new ConcurrentDictionary<Guid, TicTacToeGame>();
    }
}