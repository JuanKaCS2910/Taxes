using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taxes.Models
{
    [NotMapped]
    public class TaxPaerView : TaxPaer
    {
        public List<Property> PropertyList { get; set; }
    }
}