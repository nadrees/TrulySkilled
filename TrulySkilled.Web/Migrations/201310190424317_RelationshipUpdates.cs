namespace TrulySkilled.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationshipUpdates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamModelMatchModels",
                c => new
                    {
                        TeamModel_Id = c.Int(nullable: false),
                        MatchModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeamModel_Id, t.MatchModel_Id })
                .ForeignKey("dbo.TeamModels", t => t.TeamModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.MatchModels", t => t.MatchModel_Id, cascadeDelete: true)
                .Index(t => t.TeamModel_Id)
                .Index(t => t.MatchModel_Id);
            
            CreateTable(
                "dbo.PlayerModelTeamModels",
                c => new
                    {
                        PlayerModel_Id = c.Int(nullable: false),
                        TeamModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PlayerModel_Id, t.TeamModel_Id })
                .ForeignKey("dbo.PlayerModels", t => t.PlayerModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.TeamModels", t => t.TeamModel_Id, cascadeDelete: true)
                .Index(t => t.PlayerModel_Id)
                .Index(t => t.TeamModel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerModelTeamModels", "TeamModel_Id", "dbo.TeamModels");
            DropForeignKey("dbo.PlayerModelTeamModels", "PlayerModel_Id", "dbo.PlayerModels");
            DropForeignKey("dbo.TeamModelMatchModels", "MatchModel_Id", "dbo.MatchModels");
            DropForeignKey("dbo.TeamModelMatchModels", "TeamModel_Id", "dbo.TeamModels");
            DropIndex("dbo.PlayerModelTeamModels", new[] { "TeamModel_Id" });
            DropIndex("dbo.PlayerModelTeamModels", new[] { "PlayerModel_Id" });
            DropIndex("dbo.TeamModelMatchModels", new[] { "MatchModel_Id" });
            DropIndex("dbo.TeamModelMatchModels", new[] { "TeamModel_Id" });
            DropTable("dbo.PlayerModelTeamModels");
            DropTable("dbo.TeamModelMatchModels");
        }
    }
}
