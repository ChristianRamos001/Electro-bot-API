using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Seguridad.Usuario
{
    public class UsuarioViewModel
    {
        public int idusuario { get; set; }
        public int idrol { get; set; }
        public string nombreUsuario { get; set; }
        public string rol { get; set; }
        public string email { get; set; }
        public byte[] password_hash { get; set; }
        public bool condicion { get; set; }
    }
}
