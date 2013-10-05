using System;
using System.Linq;
using System.Web.Mvc;
using TrulySkilled.Game.TicTacToe;
using TrulySkilled.Web.Hubs;
using TrulySkilled.Web.Models;

namespace TrulySkilled.Web.Controllers
{
    public class TicTacToeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Game(Guid id)
        {
            int? playerId = null;

            using (var db = new TrulySkilledDbContext())
            {
                var player = (from p in db.Players
                              where p.User.UserName == User.Identity.Name
                              select p)
                             .FirstOrDefault();

                if (player == null)
                {
                    player = new PlayerModel
                    {
                        Game = (from g in db.Games
                                where g.Name == GameNames.TicTacToe
                                select g)
                                .First(),
                        User = (from u in db.UserProfiles
                                where u.UserName == User.Identity.Name
                                select u)
                               .First()
                    };
                    db.Players.Add(player);
                    db.SaveChanges();
                }

                playerId = player.Id;
            }

            TicTacToeHub.GamesInProgress.AddOrUpdate(id,
                new TicTacToeGame(playerId.Value),
                (_, game) => { game.AddPlayer(playerId.Value); return game; });

            return View(id);
        }
    }
}
