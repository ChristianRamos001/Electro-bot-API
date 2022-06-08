using API.Entity.Monitoreo;
using API.Entity.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace API.Entity.Administracion
{
    public class Paciente
    {
        public int Id_paciente { get; set; }
        public string Nombre{ get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM  { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int DNI { get; set; }
        public int? id_wearable { get; set; }
        public int? idusuario { get; set; }

        [ForeignKey("idusuario")] 
        public User? User { get; set; }
        [ForeignKey("id_wearable")]
        public Wearable? Wearable { get; set; }
        public ICollection<PacienteMedico>? PacienteMedicos { get; set; }

    }
}
