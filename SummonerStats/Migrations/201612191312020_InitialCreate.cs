namespace SummonerStats.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatchHistoryModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        matchIndex = c.Long(nullable: false),
                        timestamp = c.Long(nullable: false),
                        champion = c.Int(nullable: false),
                        region = c.String(),
                        queue = c.String(),
                        season = c.String(),
                        matchId = c.Long(nullable: false),
                        role = c.String(),
                        platformId = c.String(),
                        lane = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MatchHistoryModels");
        }
    }
}
