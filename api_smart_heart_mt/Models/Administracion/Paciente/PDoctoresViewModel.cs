using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Administracion.Paciente
{
    public class PDoctoresViewModel
    {
        public int idMedico { get;  set; }
        public string nombre { get;  set; }
        public string apellidoP { get;  set; }
        public string apellidoM { get;  set; }
        public int DNI { get;  set; }
    }

    public class PDoctoresViewModelMobile
    {
        public int idMedico { get; set; }
        public string nombre { get; set; }
        public string apellidoP { get; set; }
        public string apellidoM { get; set; }
        public int DNI { get; set; }
        public string urlIcon { get; set; }

    }
}
