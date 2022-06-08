using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entity.Monitoreo;
using API.Smart_Heart.Models.Monitoreo.Wearable;
using System.Collections;

namespace API.Smart_Heart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WearablesController : Controller
    {
        private readonly DBContextSistema _context;

        public WearablesController(DBContextSistema context)
        {
            _context = context;
        }

        // GET: api/Wearables/Listar
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<WearableViewModel>>> Listar()
        {
            var wearables = await (from wearable in _context.Wearables
                                   join _paciente in _context.Pacientes on wearable.id_wearable equals _paciente.id_wearable into wearable_paciente
                                   from paciente in wearable_paciente.DefaultIfEmpty()
                                       //where paciente == null
                                   select new
                                   {
                                       wearable.id_wearable,
                                       wearable.idw,
                                       wearable.marca,
                                       wearable.modelo,
                                       paciente.Id_paciente,
                                       paciente.Nombre,
                                       paciente.ApellidoP,
                                       paciente.ApellidoM,
                                       paciente.fechaNacimiento,
                                       paciente.DNI
                                   }).ToListAsync();


            //var wearable = await _context.Pacientes
            //                              .Include(o => o.Wearable)    //.Include(v => v.Rol)
            //                            .ToListAsync();

            return wearables.Select(w => new WearableViewModel
            {
                idWearable = w.id_wearable,
                idw = w.idw,
                marca = w.marca,
                modelo = w.modelo,
                idPaciente = w.Id_paciente,
                nombre = w.Nombre,
                apellidoP = w.ApellidoP,
                apellidoM = w.ApellidoM,
                DNI = w.DNI,
                edad = DateTime.Today.AddTicks(-w.fechaNacimiento.Ticks).Year - 1
            }).ToList();

        }

        // POST: api/Wearables/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] WearableCreateModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Wearable wearable = new Wearable
            {
                idw = model.idw,
                marca = model.marca,
                modelo = model.modelo
            };

            _context.Wearables.Add(wearable);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();


        }

        // PUT: api/Wearables/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] WearableUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wearable = await _context.Wearables
                .FirstOrDefaultAsync(u => u.id_wearable == model.idWearable);

            if (wearable == null)
            {
                return NotFound();
            }

            wearable.idw = model.idw;
            wearable.marca = model.marca;
            wearable.modelo = model.modelo;

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


        // DELETE: api/Wearables/Eliminar
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            var wearable = await _context.Wearables.FindAsync(id);

            if (wearable == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.id_wearable == id);
            paciente.id_wearable = null;
            paciente.Wearable = null;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            _context.Wearables.Remove(wearable);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        // DELETE: api/Wearables/Visualizar/id
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Wearable>> Visualizar([FromRoute] int id)
        {
            var wearable = await _context.Wearables.FindAsync(id);

            if (wearable == null)
            {
                return Ok(new { message = "No existe el wearable" });
            }

            return wearable;

        }

        // GET: api/Wearables/Disponibles
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<WearableDViewModel>>> Disponibles()
        {
            var wearables = await (from wearable in _context.Wearables
                                   join _paciente in _context.Pacientes on wearable.id_wearable equals _paciente.id_wearable into wearable_paciente
                                   from paciente in wearable_paciente.DefaultIfEmpty()
                                   where paciente == null
                                   select new
                                   {
                                       wearable.id_wearable,
                                       wearable.idw,
                                       wearable.marca,
                                       wearable.modelo
                                   }).ToListAsync();


            if (wearables == null)
            {
                return Ok(new { message = "No existen wearables disponibles" });
            }

            return wearables.Select(c => new WearableDViewModel
            {
                idWearable = c.id_wearable,
                idw = c.idw,
                modelo = c.modelo,
                marca = c.marca
            }).ToList();
        }



        private bool WearableExists(int id)
        {
            return _context.Wearables.Any(e => e.id_wearable == id);
        }
    }
}
