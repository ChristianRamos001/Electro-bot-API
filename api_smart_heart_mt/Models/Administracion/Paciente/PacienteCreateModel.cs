using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion.Paciente
{
    public class PacienteCreateModel
    {
        [Required(ErrorMessage = "Nombre is required")] 
        public string Nombre { get; set; }
        [Required(ErrorMessage = "ApellidoP is required")]
        public string ApellidoP { get; set; }
        [Required(ErrorMessage = "ApellidoM is required")]
        public string ApellidoM { get; set; }
        [Required(ErrorMessage = "fechaNacimiento is required")]
        public DateTime fechaNacimiento { get; set; }
        [Required(ErrorMessage = "DNI is required")] 
        public int? DNI { get; set; }
        [Required(ErrorMessage = "email is required")]
        public string email { get; set; }
        public int? id_wearable { get; set; }
        public List<int>? listMedicos { get; set; }

    }
}
