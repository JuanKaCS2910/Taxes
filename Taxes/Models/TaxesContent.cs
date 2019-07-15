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

        public System.Data.Entity.DbSet<Taxes.Models.PropertyType> PropertyTypes { get; set; }
    }
}