using System;
using System.Collections.Generic;
using System.Text;

namespace API.Entity.Administracion
{
    public class CentroSalud
    {
        public int IDcentroSalud { get; set; }
        public string NombreCS { get; set; }
        public int RUC { get; set; }
        public string direccion { get; set; }
        public string TipoCS { get; set; }
    }
}
