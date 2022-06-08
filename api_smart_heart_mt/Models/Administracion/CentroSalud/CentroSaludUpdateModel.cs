using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion.CentroSalud
{
    public class CentroSaludUpdateModel
    {
        [Required(ErrorMessage = "IdcentroSalud is required")]
        public int? IdcentroSalud { get; set; }
        [Required(ErrorMessage = "NombreCS is required")]
        public string NombreCS { get; set; }
        [Required(ErrorMessage = "RUC is required")]
        public int? RUC { get; set; }
        [Required(ErrorMessage = "direccion is required")]
        public string direccion { get; set; }
        [Required(ErrorMessage = "TipoCS is required")]
        public string TipoCS { get; set; }
    }
}
