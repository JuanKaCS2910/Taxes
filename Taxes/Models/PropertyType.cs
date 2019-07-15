
namespace Taxes.Models
{
    using System.ComponentModel.DataAnnotations;
    public class PropertyType
    {
        [Key]
        public int PropertyId { get; set; }

        [Required(ErrorMessage ="The field {0} is required")]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
    }
}