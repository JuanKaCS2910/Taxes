namespace Taxes.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class TaxProperty
    {
        [Key]
        public int TaxPropertyId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int Year { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public DateTime DateGenerated { get; set; }

        public DateTime? DatePay { get; set; }

        public bool IsPay { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 9999999999,
            ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        //C2: Currency con 2decimales
        public decimal Value { get; set; }

        public virtual Property Property { get; set; }

    }
}