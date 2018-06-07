namespace AplikacjaQuizowa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poprawki4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "Contents", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Questions", "Answer1", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Questions", "Answer2", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Questions", "Answer3", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Questions", "CorrectAnswer", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "CorrectAnswer", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Questions", "Answer3", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Questions", "Answer2", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Questions", "Answer1", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Questions", "Contents", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
