namespace TrulySkilled.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Beta = c.Double(nullable: false),
                        DrawProbability = c.Double(nullable: false),
                        DynamicsFactor = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MatchModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameModels", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.PlayerModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mean = c.Double(nullable: false),
                        StandardDeviation = c.Double(nullable: false),
                        Game_Id = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameModels", t => t.Game_Id)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .Index(t => t.Game_Id)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.TeamModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Position = c.Int(nullable: false),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameModels", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TeamModels", new[] { "Game_Id" });
            DropIndex("dbo.PlayerModels", new[] { "User_UserId" });
            DropIndex("dbo.PlayerModels", new[] { "Game_Id" });
            DropIndex("dbo.MatchModels", new[] { "Game_Id" });
            DropForeignKey("dbo.TeamModels", "Game_Id", "dbo.GameModels");
            DropForeignKey("dbo.PlayerModels", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.PlayerModels", "Game_Id", "dbo.GameModels");
            DropForeignKey("dbo.MatchModels", "Game_Id", "dbo.GameModels");
            DropTable("dbo.TeamModels");
            DropTable("dbo.PlayerModels");
            DropTable("dbo.MatchModels");
            DropTable("dbo.GameModels");
        }
    }
}
