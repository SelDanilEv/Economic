namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addstatisticclass : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ActiveTargets", newName: "Targets");
            DropForeignKey("dbo.OldTargets", "User_Id", "dbo.Users");
            DropForeignKey("dbo.SuspendedTargets", "User_Id", "dbo.Users");
            DropIndex("dbo.OldTargets", new[] { "User_Id" });
            DropIndex("dbo.SuspendedTargets", new[] { "User_Id" });
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CounterTargets = c.Int(nullable: false),
                        MoodChange = c.Int(nullable: false),
                        LargestTarget_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Targets", t => t.LargestTarget_Id)
                .Index(t => t.LargestTarget_Id);
            
            AddColumn("dbo.Targets", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Targets", "User_Id1", c => c.Int());
            AddColumn("dbo.Targets", "User_Id2", c => c.Int());
            AddColumn("dbo.Users", "Statistic_Id", c => c.Int());
            CreateIndex("dbo.Targets", "User_Id1");
            CreateIndex("dbo.Targets", "User_Id2");
            CreateIndex("dbo.Users", "Statistic_Id");
            AddForeignKey("dbo.Users", "Statistic_Id", "dbo.Statistics", "Id");
            AddForeignKey("dbo.Targets", "User_Id1", "dbo.Users", "Id");
            AddForeignKey("dbo.Targets", "User_Id2", "dbo.Users", "Id");
            DropTable("dbo.OldTargets");
            DropTable("dbo.SuspendedTargets");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SuspendedTargets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetName = c.String(),
                        Spend = c.Double(nullable: false),
                        TotalSum = c.Double(nullable: false),
                        CurrentSum = c.Double(nullable: false),
                        TargetTime = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OldTargets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetName = c.String(),
                        Spend = c.Double(nullable: false),
                        TotalSum = c.Double(nullable: false),
                        CurrentSum = c.Double(nullable: false),
                        TargetTime = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Targets", "User_Id2", "dbo.Users");
            DropForeignKey("dbo.Targets", "User_Id1", "dbo.Users");
            DropForeignKey("dbo.Users", "Statistic_Id", "dbo.Statistics");
            DropForeignKey("dbo.Statistics", "LargestTarget_Id", "dbo.Targets");
            DropIndex("dbo.Users", new[] { "Statistic_Id" });
            DropIndex("dbo.Statistics", new[] { "LargestTarget_Id" });
            DropIndex("dbo.Targets", new[] { "User_Id2" });
            DropIndex("dbo.Targets", new[] { "User_Id1" });
            DropColumn("dbo.Users", "Statistic_Id");
            DropColumn("dbo.Targets", "User_Id2");
            DropColumn("dbo.Targets", "User_Id1");
            DropColumn("dbo.Targets", "Discriminator");
            DropTable("dbo.Statistics");
            CreateIndex("dbo.SuspendedTargets", "User_Id");
            CreateIndex("dbo.OldTargets", "User_Id");
            AddForeignKey("dbo.SuspendedTargets", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.OldTargets", "User_Id", "dbo.Users", "Id");
            RenameTable(name: "dbo.Targets", newName: "ActiveTargets");
        }
    }
}
