using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrulySkilled.Game.TicTacToe;
using TrulySkilled.Web.Hubs.TicTacToe;
using TrulySkilled.Web.Models;
using TrulySkilled.Web.ViewModels.Game;

namespace TrulySkilled.Web.Controllers
{
    public class TicTacToeController : Controller, IGameController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Game(Guid id)
        {
            /**
            ActionResult result = RedirectToAction("Index");

            TicTacToeGame game;
            if (TicTacToeGameHub.GamesInProgress.TryGetValue(id, out game) && game != null)
            {
                if (game.Players.Any(p => p.PlayerName == User.Identity.Name && !p.PlayerLeft))
                {
                    result = View(id);
                }
            }

            return result;
             */
            return View(id);
        }

        
        public void RegisterGame(Guid gameId, IEnumerable<string> players)
        {
            //TicTacToeGameHub.GamesInProgress.TryAdd(gameId, new TicTacToeGame(players));
        }
    }
}
