namespace AplikacjaQuizowa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poprawki2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "Contents", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Questions", "Answer1", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "Answer1", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Questions", "Contents", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
