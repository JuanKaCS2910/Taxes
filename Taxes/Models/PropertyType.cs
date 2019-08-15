
namespace Taxes.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PropertyType
    {
        [Key]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage ="The field {0} is required")]
        [Index("PropertyType_Description_Index", IsUnique =true)]
        [StringLength(30, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characters",MinimumLength =1)]
        [Display(Name = "Property type")]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}