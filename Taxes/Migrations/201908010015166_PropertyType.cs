namespace Taxes.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PropertyType : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.PropertyTypes");
            //
            //
            //AddColumn("dbo.PropertyTypes", "PropertyTypeId", c => c.Int(nullable: false, /identity:/ true));
            //AddPrimaryKey("dbo.PropertyTypes", "PropertyTypeId");
            //DropColumn("dbo.PropertyTypes", "PropertyId");

            CreateTable(
                "dbo.Properties",
                c => new
                {
                    PropertyId = c.Int(nullable: false, identity: true),
                    TaxPaerId = c.Int(nullable: false),
                    Phone = c.String(),
                    DepartmentId = c.Int(nullable: false),
                    MunicipalityId = c.Int(nullable: false),
                    Address = c.String(),
                    PropertyTypeId = c.Int(nullable: false),
                    Stratum = c.Int(nullable: false),
                    Area = c.Single(nullable: false),
                })
                .PrimaryKey(t => t.PropertyId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .ForeignKey("dbo.Municipalities", t => t.MunicipalityId)
                .ForeignKey("dbo.PropertyTypes", t => t.PropertyTypeId)
                .ForeignKey("dbo.TaxPaers", t => t.TaxPaerId)
                .Index(t => t.TaxPaerId)
                .Index(t => t.DepartmentId)
                .Index(t => t.MunicipalityId)
                .Index(t => t.PropertyTypeId);
        }
        
        public override void Down()
        {
            AddColumn("dbo.PropertyTypes", "PropertyId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Properties", "TaxPaerId", "dbo.TaxPaers");
            DropForeignKey("dbo.Properties", "PropertyTypeId", "dbo.PropertyTypes");
            DropForeignKey("dbo.Properties", "MunicipalityId", "dbo.Municipalities");
            DropForeignKey("dbo.Properties", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Properties", new[] { "PropertyTypeId" });
            DropIndex("dbo.Properties", new[] { "MunicipalityId" });
            DropIndex("dbo.Properties", new[] { "DepartmentId" });
            DropIndex("dbo.Properties", new[] { "TaxPaerId" });
            DropPrimaryKey("dbo.PropertyTypes");
            DropColumn("dbo.PropertyTypes", "PropertyTypeId");
            DropTable("dbo.Properties");
            AddPrimaryKey("dbo.PropertyTypes", "PropertyId");
        }
    }
}
