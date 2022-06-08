using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion
{
    public class MedicoUpdateModel
    {
        public int Id_medico { get; set; }
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public int DNI { get; set; }
    }
}
