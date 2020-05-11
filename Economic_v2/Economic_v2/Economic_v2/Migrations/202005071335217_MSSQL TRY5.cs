namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MSSQLTRY5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SuspendedTargets", "TargetName", c => c.String());
            AddColumn("dbo.SuspendedTargets", "Spend", c => c.Double(nullable: false));
            AddColumn("dbo.SuspendedTargets", "TotalSum", c => c.Double(nullable: false));
            AddColumn("dbo.SuspendedTargets", "CurrentSum", c => c.Double(nullable: false));
            AddColumn("dbo.SuspendedTargets", "TargetTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SuspendedTargets", "TargetTime");
            DropColumn("dbo.SuspendedTargets", "CurrentSum");
            DropColumn("dbo.SuspendedTargets", "TotalSum");
            DropColumn("dbo.SuspendedTargets", "Spend");
            DropColumn("dbo.SuspendedTargets", "TargetName");
        }
    }
}
