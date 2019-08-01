namespace Taxes.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        public int TaxPaerId { get; set; }

        public string Phone { get; set; }

        public int DepartmentId { get; set; }

        public int MunicipalityId { get; set; }

        public string Address { get; set; }

        public int PropertyTypeId { get; set; }

        public int Stratum { get; set; }
        public float Area { get; set; }

        public virtual TaxPaer TaxPaer { get; set; }
        public virtual Department Department { get; set; }
        public virtual Municipality Municipality { get; set; }
        public virtual PropertyType PropertyType { get; set; }

    }
}