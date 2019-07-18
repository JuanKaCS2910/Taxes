using System.Data.Entity;

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

        public System.Data.Entity.DbSet<PropertyType> PropertyTypes { get; set; }

        public System.Data.Entity.DbSet<Department> Departments { get; set; }

        public System.Data.Entity.DbSet<Municipality> Municipalities { get; set; }
    }
}