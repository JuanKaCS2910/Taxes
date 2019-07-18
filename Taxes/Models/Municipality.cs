namespace Taxes.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Municipality
    {
        [Key]
        public int MunicipalityId { get; set; }
        public int DeparmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Index("Municipality_Name_Index", IsUnique = true)]
        [StringLength(30, ErrorMessage = "The field {0} can contain maximun {1} and minimun {2} characters", MinimumLength = 1)]
        [Display(Name = "Municipality name")]
        public string Name { get; set; }
        public virtual Department Department { get; set; }
    }
}