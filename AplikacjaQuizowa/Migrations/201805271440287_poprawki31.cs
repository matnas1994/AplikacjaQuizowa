namespace AplikacjaQuizowa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poprawki31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Answer1", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Questions", "Answer2", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.Questions", "Answer3", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Answer3");
            DropColumn("dbo.Questions", "Answer2");
            DropColumn("dbo.Questions", "Answer1");
        }
    }
}
