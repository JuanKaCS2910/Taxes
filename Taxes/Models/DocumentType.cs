
namespace Taxes.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("DocumentType_Description_Index", IsUnique = true)]
        [StringLength(30, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characters", MinimumLength = 1)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual ICollection<TaxPaer> TaxPaers { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}