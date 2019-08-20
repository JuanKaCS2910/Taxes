namespace Taxes.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class MunicipalitiesView
    {

        public string Department { get; set; }

        public string Municipality { get; set; }

        public List<Municipality> Municipalities { get; set; }

    }
}