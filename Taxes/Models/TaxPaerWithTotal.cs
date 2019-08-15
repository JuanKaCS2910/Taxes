namespace Taxes.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class TaxPaerWithTotal
    {
        public TaxPaer TaxPaer { get; set; }
        [DisplayFormat(DataFormatString ="{0:C2}",ApplyFormatInEditMode =false)]
        public decimal Total { get; set; }
    }
}