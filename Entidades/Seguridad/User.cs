using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entity.Seguridad
{
    public class User
    {
        public int idusuario { get; set; }
        [Required]
        public int idrol { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public byte[] password_hash { get; set; }
        [Required]
        public byte[] password_salt { get; set; }
        public bool condicion { get; set; }

        [ForeignKey("idrol")]
        public Rol Rol { get; set; }

    }
}
