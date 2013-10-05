namespace TrulySkilled.Web.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TrulySkilled.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TrulySkilled.Web.Models.TrulySkilledDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TrulySkilled.Web.Models.TrulySkilledDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            double defaultBeta = 25.0 / 6.0;
            double defaultDrawProbability = .10;
            double defaultDynamicsFactor = 25.0 / 300.0;

            foreach (var gameName in GameNames.GameNamesSet)
            {
                if (context.Games.FirstOrDefault(g => g.Name == gameName) == null)
                {
                    var game = new GameModel
                    {
                        Beta = defaultBeta,
                        DrawProbability = defaultDrawProbability,
                        DynamicsFactor = defaultDynamicsFactor,
                        Name = gameName
                    };
                    context.Games.Add(game);
                }
            }

            context.SaveChanges();
        }
    }
}
