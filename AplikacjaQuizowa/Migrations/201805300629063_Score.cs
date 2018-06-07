namespace AplikacjaQuizowa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Score : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Scores",
                c => new
                    {
                        ScoreId = c.Int(nullable: false, identity: true),
                        Result = c.Double(nullable: false),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ScoreId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.UserId_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Scores", "UserId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Scores", new[] { "UserId_Id" });
            DropTable("dbo.Scores");
        }
    }
}
