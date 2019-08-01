namespace Taxes.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    //[Table("Deparment")]
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("Deparment_Name_Index", IsUnique = true)]
        [StringLength(30, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characters", MinimumLength = 1)]
        [Display(Name="Department name")]
        public string Name { get; set; }

        public virtual ICollection<Municipality> Municipalities { get; set; }

        public virtual ICollection<TaxPaer> TaxPaers { get; set; }
        public virtual ICollection<Property> Properties { get; set; }

    }
}