using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion.Medico
{
    public class AdicionarPacientesModel
    {

        [Required(ErrorMessage = "idPaciente is required")]
        public int idMedico { get; set; }
        [Required(ErrorMessage = "listIDMedicos is required")]
        public List<int> listIDPacientes { get; set; }
    }
}
