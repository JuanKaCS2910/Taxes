using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Taxes.Models
{
    public class TaxesContent : DbContext
    {
        public TaxesContent() : base("DefaultConnection")
        {
        }

        /*
        El método dispose es importante para cerrar la conexxión a la BD. 
        */
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);    
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Preveer la eliminación en cascada
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //base.OnModelCreating(modelBuilder); 
        }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Municipality> Municipalities { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<TaxPaer> TaxPaers { get; set; }
        public DbSet<Property> Properties { get; set; }

        public DbSet<Tax> Taxes { get; set; }

        public DbSet<TaxProperty> TaxProperties { get; set; }
    }
}