using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entity.Seguridad;
using API.Smart_Heart.Models.Seguridad.Rol;

namespace API.Smart_Heart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly DBContextSistema _context;

        public RolesController(DBContextSistema context)
        {
            _context = context;
        }

        // GET: api/Roles/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolViewModel>> Listar()
        {
            var roles = await _context.Roles.ToListAsync();

            return roles.Select(c => new RolViewModel
            {
                idrol = c.idrol,
                condicion = c.condicion,
                descripcion = c.descripcion,
                nombre = c.nombre,
            });

        }

        // GET: api/Roles/Select
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolSelectViewModel>> Select()
        {
            var roles = await _context.Roles
                .Where(r => r.condicion == true)
                .ToListAsync();

            return roles.Select(r => new RolSelectViewModel
            {
                idrol = r.idrol,
                nombre = r.nombre,
            });

        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.idrol == id);
        }
    }
}
