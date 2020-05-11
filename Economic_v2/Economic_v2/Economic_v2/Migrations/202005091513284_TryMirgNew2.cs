namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryMirgNew2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActiveTargets", "TargetTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OldTargets", "TargetTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SuspendedTargets", "TargetTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SuspendedTargets", "TargetTime", c => c.DateTime());
            AlterColumn("dbo.OldTargets", "TargetTime", c => c.DateTime());
            AlterColumn("dbo.ActiveTargets", "TargetTime", c => c.DateTime());
        }
    }
}
