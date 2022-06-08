using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Monitoreo.Wearable
{
    public class WearableViewModel
    {
        public int idWearable { get; set; }
        public string idw { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public int? idPaciente { get; set;}
        public string nombre { get; set; }
        public string apellidoM { get; set; }
        public string apellidoP { get; set; }
        public int? DNI { get; set; }
        public int? edad { get; set; }
    }
}
