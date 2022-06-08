using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entity.Administracion;
using API.Smart_Heart.Models.Administracion;
using API.Smart_Heart.Models.Administracion.Medico;
using API.Entity.Seguridad;
using API.Smart_Heart.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Smart_Heart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly DBContextSistema _context;
        private readonly IConfiguration _config;
        public MedicosController(DBContextSistema context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        // GET: api/Medicos
        [HttpGet]
        public async Task<IEnumerable<MedicoViewModel>> Listar()
        {
            var medicos = await _context.Medicos
                                        .Include(c => c.pacienteMedicos)
                                        .ToListAsync();

            return medicos.Select(c => new MedicoViewModel
            {
                Id_medico = c.Id_medico,
                Nombre = c.Nombre,
                ApellidoM = c.ApellidoM,
                ApellidoP = c.ApellidoP,
                DNI = c.DNI,
                PacientesAsig = c.pacienteMedicos.Count()
            });
        }

        // GET: api/Medicos/VisualizarPacientes
        [HttpGet("[action]")]
        [Authorize(Roles = "Médico")]
        public async Task<IEnumerable<MPacienteViewModel>> VisualizarPacientes()
        {
            var idusuario = HelperController.readToken(HttpContext, _config["Jwt:Key"]);

            var medico = await _context.Medicos.FirstOrDefaultAsync(c => c.idusuario == idusuario);

            if (medico == null)
            {
                return Enumerable.Empty<MPacienteViewModel>();
            }

            var pacienteMedicos = await _context.PacienteMedicos
                                                .Where(p => p.Id_medico == medico.Id_medico)
                                                .Include(p => p.Paciente)
                                                .ToListAsync();  

            return pacienteMedicos.Select(o => new MPacienteViewModel
            {
                idPaciente = o.Id_paciente,
                nombre = o.Paciente.Nombre,
                apellidoP = o.Paciente.ApellidoP,
                apellidoM = o.Paciente.ApellidoM,
                DNI = o.Paciente.DNI,
                edad = DateTime.Today.AddTicks(-o.Paciente.fechaNacimiento.Ticks).Year - 1
            });

        }
        
        // POST: api/Medicos/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] MedicoCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var idusuario = await CrearUsuario(model.email);

            if (idusuario == 0)
            {
                return BadRequest("El email ya existe");
            }

            if (idusuario == -1)
            {
                return BadRequest("Error");
            }

            Medico medico = new Medico
            {
                Nombre = model.Nombre,
                ApellidoM = model.ApellidoM,
                ApellidoP = model.ApellidoP,
                DNI = (int)model.DNI,
                idusuario = idusuario
            };

            _context.Medicos.Add(medico);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            ///Pacientes
            try
            {
                if (model.listPacientes != null)
                {
                    if (model.listPacientes.Count() > 0)
                    {
                        foreach (var idpaciente in model.listPacientes)
                        {
                            PacienteMedico pacienteMedico = new PacienteMedico()
                            {
                                Id_medico = medico.Id_medico,
                                Id_paciente = idpaciente
                            };

                            _context.PacienteMedicos.Add(pacienteMedico);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();

        }

        // PUT: api/Medicos/Actualizar
        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] MedicoUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medico = await _context.Medicos
                .FirstOrDefaultAsync(u => u.Id_medico == model.Id_medico);

            if (medico == null)
            {
                return NotFound();
            }

            medico.Nombre = model.Nombre;
            medico.ApellidoP = model.ApellidoP;
            medico.ApellidoM = model.ApellidoM;
            medico.DNI = model.DNI;

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

        // PUT: api/Pacientes/AdicionarMedicos
        [HttpPut("[action]")]
        public async Task<IActionResult> AdicionarPacientes([FromBody] AdicionarPacientesModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medico = await _context.Medicos.FindAsync(model.idMedico);

            var pacienteMedicos = await _context.PacienteMedicos
                .Where(dc => dc.Id_medico == medico.Id_medico)
                .ToListAsync();

            if (medico == null)
            {
                return Ok(new { message = "No se ha encontrado el medico consultado" });
            }


            ///Eliminar
            try
            {
                foreach (var obj in pacienteMedicos)
                {
                    _context.PacienteMedicos.Remove(obj);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            ///

            try
            {
                foreach (var idpaciente in model.listIDPacientes)
                {
                    PacienteMedico pacienteMedico = new PacienteMedico()
                    {
                        Id_medico = medico.Id_medico,
                        Id_paciente = idpaciente
                    };

                    _context.PacienteMedicos.Add(pacienteMedico);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();

        }

        // DELETE: api/Medicos/Eliminar/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            var medicos = await _context.Medicos.Where(m => m.Id_medico == id)
                .ToListAsync();

            var pacientemedicos = await _context.PacienteMedicos.Where(m => m.Id_medico == id)
                .ToListAsync();

            if (medicos == null)
            {
                return NotFound();
            }

            _context.PacienteMedicos.RemoveRange(pacientemedicos);
            _context.Medicos.RemoveRange(medicos);

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

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        }

        private async Task<int> CrearUsuario(string modelemail)
        {
            var emailC = modelemail.ToLower();

            if (await _context.Usuarios.AnyAsync(u => u.email == emailC))
            {
                return 0;
            }

            CrearPasswordHash("demo", out byte[] passwordHash, out byte[] passwordSalt);

            User usuario = new User
            {
                idrol = 2,
                email = emailC,
                password_hash = passwordHash,
                password_salt = passwordSalt,
                condicion = true
            };

            _context.Usuarios.Add(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }

            return usuario.idusuario;
        }


        private bool MedicoExists(int id)
        {
            return _context.Medicos.Any(e => e.Id_medico == id);
        }
    }
}
