namespace Economic_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class littlerename : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Nodes", newName: "Notes");
            RenameColumn(table: "dbo.Users", name: "Node_Id", newName: "Note_Id");
            RenameIndex(table: "dbo.Users", name: "IX_Node_Id", newName: "IX_Note_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Users", name: "IX_Note_Id", newName: "IX_Node_Id");
            RenameColumn(table: "dbo.Users", name: "Note_Id", newName: "Node_Id");
            RenameTable(name: "dbo.Notes", newName: "Nodes");
        }
    }
}
