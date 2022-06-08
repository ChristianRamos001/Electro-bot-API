using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion.Paciente
{
    public class AdicionarWearableModel
    {
        [Required(ErrorMessage = "idPaciente is required")]
        public int idPaciente { get; set; }
        [Required(ErrorMessage = "idWearable is required")]
        public int idWearable { get; set; }


    }
}
