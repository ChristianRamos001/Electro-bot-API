using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion.Paciente
{
    public class PacienteFViewModel
    {
        public int Id_paciente { get; set; }
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int DNI { get; set; }
        public int edad { get; set; }
    }
}
