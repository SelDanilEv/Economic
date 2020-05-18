namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtips : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tips");
        }
    }
}
