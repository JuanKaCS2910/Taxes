﻿namespace Taxes.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Tax
    {
        [Key]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 7,
            ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        public int Stratum { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(0, 1,
            ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        //P2: Porcentual a 2 decimales
        public float Rate { get; set; }
    }
}