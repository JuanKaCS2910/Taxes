
namespace Taxes.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    //Clase NotMapped: No se crea a nivel de BD.

    [NotMapped]
    public class DepartmentView : Department
    {
        public List<Municipality> MunicipalityList { get; set; }
    }
}