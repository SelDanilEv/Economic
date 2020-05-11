namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MSSQLTRY6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActiveTargets", "Spend", c => c.Double());
            AlterColumn("dbo.ActiveTargets", "TargetTime", c => c.DateTime());
            AlterColumn("dbo.OldTargets", "Spend", c => c.Double());
            AlterColumn("dbo.OldTargets", "TargetTime", c => c.DateTime());
            AlterColumn("dbo.SuspendedTargets", "Spend", c => c.Double());
            AlterColumn("dbo.SuspendedTargets", "TargetTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SuspendedTargets", "TargetTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SuspendedTargets", "Spend", c => c.Double(nullable: false));
            AlterColumn("dbo.OldTargets", "TargetTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OldTargets", "Spend", c => c.Double(nullable: false));
            AlterColumn("dbo.ActiveTargets", "TargetTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ActiveTargets", "Spend", c => c.Double(nullable: false));
        }
    }
}
