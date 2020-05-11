namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryMirgNew3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActiveTargets", "Spend", c => c.Double(nullable: false));
            AlterColumn("dbo.OldTargets", "Spend", c => c.Double(nullable: false));
            AlterColumn("dbo.SuspendedTargets", "Spend", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SuspendedTargets", "Spend", c => c.Double());
            AlterColumn("dbo.OldTargets", "Spend", c => c.Double());
            AlterColumn("dbo.ActiveTargets", "Spend", c => c.Double());
        }
    }
}
