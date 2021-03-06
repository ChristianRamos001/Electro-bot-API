using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Smart_Heart.Models.Seguridad.Usuario
{
    public class ActualizarViewModel
    {
        [Required]
        public int idusuario { get; set; }
        [Required]
        public int idrol { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3
            , ErrorMessage = "El nombre debe ser mayor a 3 caracteres y menor a 100")]
        public string nombreUsuario { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }
        public bool act_password { get; set; }
 
    }
}
