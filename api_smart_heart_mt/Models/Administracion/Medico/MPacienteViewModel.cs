using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion.Medico
{
    public class MPacienteViewModel
    {
        public int idPaciente { get; set; }
        public string nombre { get; set; }
        public string apellidoP { get; set; }
        public string apellidoM { get; set; }
        public int DNI { get; set; }
        public int edad { get; internal set; }
    }
}
