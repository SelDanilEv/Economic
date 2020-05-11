namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MSSQLTRY4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SuspendedTargets", "TargetName");
            DropColumn("dbo.SuspendedTargets", "Spend");
            DropColumn("dbo.SuspendedTargets", "TotalSum");
            DropColumn("dbo.SuspendedTargets", "CurrentSum");
            DropColumn("dbo.SuspendedTargets", "TargetTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SuspendedTargets", "TargetTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.SuspendedTargets", "CurrentSum", c => c.Double(nullable: false));
            AddColumn("dbo.SuspendedTargets", "TotalSum", c => c.Double(nullable: false));
            AddColumn("dbo.SuspendedTargets", "Spend", c => c.Double(nullable: false));
            AddColumn("dbo.SuspendedTargets", "TargetName", c => c.String());
        }
    }
}
