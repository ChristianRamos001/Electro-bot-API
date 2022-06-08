using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entity.Administracion;
using API.Smart_Heart.Models.Administracion.CentroSalud;

namespace API.Smart_Heart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentrosSaludController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public CentrosSaludController(DBContextSistema context)
        {
            _context = context;
        }

        // GET: api/CentrosSalud
        [HttpGet("[action]")]
        public async Task<ActionResult<CentroViewModel>> Visualizar()
        {
            var centro = await _context.CentrosSalud.FirstOrDefaultAsync(c=> c.IDcentroSalud == 1);

            return new CentroViewModel
            {
                NombreCS = "Hospital Nacional Guillermo Almenara Irigoyen",
                direccion = "Jirón García Naranjo 840, La Victoria 13",
                latitud = -12.059627, 
                longitud = -77.022390,
                telefono = "01-3242983",
                TipoCS = centro.TipoCS,
                urlPhoto = "https://portal.andina.pe/EDPfotografia/Thumbnail/2015/04/18/000290151W.jpg"
            };
        }

        // PUT: api/CentrosSalud/5
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] CentroSaludUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cs = await _context.CentrosSalud
                .FirstOrDefaultAsync(u => u.IDcentroSalud == model.IdcentroSalud);

            if (cs == null)
            {
                return Ok(new { message = "No se ha encontrado el centro de salud en la DB" });
            }

            cs.NombreCS = model.NombreCS;
            cs.RUC = (int) model.RUC;
            cs.TipoCS = model.TipoCS;
            cs.direccion = model.direccion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

      
        private bool CentroSaludExists(int id)
        {
            return _context.CentrosSalud.Any(e => e.IDcentroSalud == id);
        }
    }
}
