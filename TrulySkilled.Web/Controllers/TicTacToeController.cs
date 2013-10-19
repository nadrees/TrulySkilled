using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Game(Guid gameId)
        {
            ActionResult result = RedirectToAction("Index");

            TicTacToeGame game;
            if (TicTacToeGameHub.GamesInProgress.TryGetValue(gameId, out game))
            {
                if (game.Players.Any(p => p.PlayerName == User.Identity.Name))
                {
                    result = View(gameId);
                }
            }

            return result;
        }

        /// <summary>
        /// Not routable.
        /// </summary>
        public async Task RecordResults(Dictionary<int, List<String>> playerRanks)
        {
            using (var db = new TrulySkilledDbContext())
            {
                var game = await db.Games.Where(g => g.Name == GameNames.TicTacToe).FirstAsync();

                var allUserNames = playerRanks.SelectMany(kvp => kvp.Value);
                var users = await (from up in db.UserProfiles
                                   where allUserNames.Contains(up.UserName)
                                   select up)
                                  .ToListAsync();

                var userIds = users.Select(u => u.UserId).ToList();
                var players = await (from p in db.Players
                                     where userIds.Contains(p.User.UserId)
                                     where p.Game.Name == GameNames.TicTacToe
                                     select p)
                                    .ToListAsync();
                foreach (var user in users)
                {
                    var player = players.FirstOrDefault(p => p.User.UserName == user.UserName);
                    if (player == null)
                    {
                        player = new PlayerModel
                        {
                            Game = game,
                            Mean = Defaults.DefaultMean,
                            StandardDeviation = Defaults.DefaultStandardDeviation,
                            User = user
                        };
                        db.Players.Add(player);
                    }

                    players.Add(player);
                }

                if (db.ChangeTracker.HasChanges())
                    await db.SaveChangesAsync();

                var teams = playerRanks.Select(teamRank =>
                {
                    var team = new TeamModel
                    {

                        Game = game,
                        Position = teamRank.Key,
                        Players = teamRank.Value.Select(userName => players.First(p => p.User.UserName == userName)).ToList()
                    };
                    db.Teams.Add(team);

                    return team;
                }).ToList();

                await db.SaveChangesAsync();

                var match = new MatchModel
                {
                    Game = game,
                    Teams = teams
                };
                db.Matches.Add(match);

                await db.SaveChangesAsync();
            }
        }
        
        /// <summary>
        /// Not routable.
        /// </summary>
        public void RegisterGame(Guid gameId, IEnumerable<string> players)
        {
            TicTacToeGameHub.GamesInProgress.TryAdd(gameId, new TicTacToeGame(players));
        }
    }
}
