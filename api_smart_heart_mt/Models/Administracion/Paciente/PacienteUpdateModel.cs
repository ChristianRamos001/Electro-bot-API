using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion.Paciente
{
    public class PacienteUpdateModel
    {
        public int idPaciente { get; set; }
        public string Nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int id_wearable { get; set; }
        public string ApellidoM { get; internal set; }
        public string ApellidoP { get; internal set; }
        public int DNI { get; internal set; }
    }
}
