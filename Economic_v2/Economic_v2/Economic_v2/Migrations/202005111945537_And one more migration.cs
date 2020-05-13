namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Andonemoremigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OneTimeTransactions", newName: "Transactions");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Transactions", newName: "OneTimeTransactions");
        }
    }
}
