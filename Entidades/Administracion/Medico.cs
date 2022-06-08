using API.Entity.Monitoreo;
using API.Entity.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace API.Entity.Administracion
{
    public class Medico
    {
        public int Id_medico { get; set; }
        public string Nombre{ get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM  { get; set; }
        public int DNI { get; set; }
        public int? idusuario { get; set; }

        [ForeignKey("idusuario")]
        public User user { get; set; }
        public ICollection<PacienteMedico> pacienteMedicos { get; set; }
    }
}
