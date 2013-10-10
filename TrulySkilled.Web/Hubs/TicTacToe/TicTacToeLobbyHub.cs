using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TrulySkilled.Web.Controllers;

namespace TrulySkilled.Web.Hubs.TicTacToe
{
    public class TicTacToeLobbyHub : LobbyHub<TicTacToeController>
    {
        public TicTacToeLobbyHub() : base("Tic-Tac-Toe") 
        {
        }

        public Task Ready()
        {
            return base.JoinGameLobby();
        }
    }
}