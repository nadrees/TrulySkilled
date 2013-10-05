namespace TrulySkilled.Web.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TrulySkilled.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TrulySkilledDbContext>
    {
        private readonly bool didRunPendingMigrations;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            var migrator = new DbMigrator(this);
            didRunPendingMigrations = migrator.GetPendingMigrations().Any();
        }

        protected override void Seed(TrulySkilledDbContext context)
        {
            if (!didRunPendingMigrations)
                return;

            foreach (var gameName in GameNames.GameNamesSet)
            {
                if (context.Games.FirstOrDefault(g => g.Name == gameName) == null)
                    context.Games.Add(GameModel.NewGame(gameName));
            }

            context.SaveChanges();
        }
    }
}
