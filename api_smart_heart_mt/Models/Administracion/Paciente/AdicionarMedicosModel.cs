using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion.Paciente
{
    public class AdicionarMedicosModel
    {
        [Required(ErrorMessage = "idPaciente is required")]
        public int idPaciente { get; set; }
        [Required(ErrorMessage = "listIDMedicos is required")]
        public List<int> listIDMedicos { get; set; }
    }
}
