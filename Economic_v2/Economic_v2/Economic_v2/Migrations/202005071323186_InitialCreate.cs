namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActiveTargets",
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AdjustmentContracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        Type = c.String(),
                        TypeId = c.Int(nullable: false),
                        Adjustment_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adjustments", t => t.Adjustment_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Adjustment_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Adjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OldVal = c.Double(nullable: false),
                        NewVal = c.Double(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        Spend = c.Double(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Total_money = c.Double(nullable: false),
                        Free_money = c.Double(nullable: false),
                        Last_activity = c.DateTime(nullable: false, storeType: "date"),
                        Node_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nodes", t => t.Node_Id)
                .Index(t => t.Node_Id);
            
            CreateTable(
                "dbo.Incomes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IncomeName = c.String(),
                        Money = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        text = c.String(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.OneTimeTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionName = c.String(),
                        Spend = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SuspendedTargets", "User_Id", "dbo.Users");
            DropForeignKey("dbo.OneTimeTransactions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.OldTargets", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Node_Id", "dbo.Nodes");
            DropForeignKey("dbo.Incomes", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Categories", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AdjustmentContracts", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ActiveTargets", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AdjustmentContracts", "Adjustment_Id", "dbo.Adjustments");
            DropIndex("dbo.SuspendedTargets", new[] { "User_Id" });
            DropIndex("dbo.OneTimeTransactions", new[] { "User_Id" });
            DropIndex("dbo.OldTargets", new[] { "User_Id" });
            DropIndex("dbo.Incomes", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "Node_Id" });
            DropIndex("dbo.Categories", new[] { "User_Id" });
            DropIndex("dbo.AdjustmentContracts", new[] { "User_Id" });
            DropIndex("dbo.AdjustmentContracts", new[] { "Adjustment_Id" });
            DropIndex("dbo.ActiveTargets", new[] { "User_Id" });
            DropTable("dbo.SuspendedTargets");
            DropTable("dbo.OneTimeTransactions");
            DropTable("dbo.OldTargets");
            DropTable("dbo.Nodes");
            DropTable("dbo.Incomes");
            DropTable("dbo.Users");
            DropTable("dbo.Categories");
            DropTable("dbo.Adjustments");
            DropTable("dbo.AdjustmentContracts");
            DropTable("dbo.ActiveTargets");
        }
    }
}
