using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace API.Entity.Administracion
{
    public class PacienteMedico
    {
        public int IDPacienteMedico { get; set; }
        public int Id_medico { get; set; }
        public int Id_paciente { get; set; }

        [ForeignKey("Id_medico")]
        public Medico Medico { get; set; }
        [ForeignKey("Id_paciente")]
        public Paciente Paciente { get; set; }

    }
}
