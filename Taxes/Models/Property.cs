namespace Taxes.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int TaxPaerId { get; set; }

        [StringLength(20,
            ErrorMessage ="The field {0} can contain maximun {1} and minium {2} characters",
            MinimumLength =7)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int MunicipalityId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(100,
            ErrorMessage = "The field {0} can contain maximun {1} and minium {2} characters",
            MinimumLength = 10)]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1,7, 
            ErrorMessage ="The field {0} must be contain values between {1} and {2}")]
        public int Stratum { get; set; }
        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 9999999,
            ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        public float Area { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 9999999999,
            ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode =false)]
        //C2: Currency con 2decimales
        public decimal Value { get; set; }

        public virtual TaxPaer TaxPaer { get; set; }
        public virtual Department Department { get; set; }
        public virtual Municipality Municipality { get; set; }
        public virtual PropertyType PropertyType { get; set; }

        public virtual ICollection<TaxProperty> TaxProperties { get; set; }

    }
}