namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changeusermigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Reserve_money", c => c.Double(nullable: false));
            DropColumn("dbo.Users", "Free_money");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Free_money", c => c.Double(nullable: false));
            DropColumn("dbo.Users", "Reserve_money");
        }
    }
}
