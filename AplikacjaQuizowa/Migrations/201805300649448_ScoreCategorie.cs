namespace AplikacjaQuizowa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScoreCategorie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Scores", "CategoriId_CategorieId", c => c.Int());
            CreateIndex("dbo.Scores", "CategoriId_CategorieId");
            AddForeignKey("dbo.Scores", "CategoriId_CategorieId", "dbo.Categories", "CategorieId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Scores", "CategoriId_CategorieId", "dbo.Categories");
            DropIndex("dbo.Scores", new[] { "CategoriId_CategorieId" });
            DropColumn("dbo.Scores", "CategoriId_CategorieId");
        }
    }
}
