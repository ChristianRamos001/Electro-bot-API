using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entity.Administracion;
using API.Smart_Heart.Models.Administracion.Paciente;
using Microsoft.Extensions.Configuration;
using API.Entity.Seguridad;
using Microsoft.AspNetCore.Authorization;
using API.Smart_Heart.Models;

namespace API.Smart_Heart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly DBContextSistema _context;
        private readonly IConfiguration _config;

        public PacientesController(DBContextSistema context, IConfiguration config)
        {
            _config = config;
            _context = context;

        }

        // GET: api/Pacientes
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IEnumerable<PacienteViewModel>> Listar()
        {
            var pacientes = await _context.Pacientes
                                            .Include(o => o.Wearable)
                                            .Include(o => o.PacienteMedicos)
                                            .ToListAsync();

            return pacientes.Select(o => new PacienteViewModel
            {
                Id_paciente = o.Id_paciente,
                Nombre = o.Nombre,
                ApellidoP = o.ApellidoP,
                ApellidoM = o.ApellidoM,
                fechaNacimiento = o.fechaNacimiento,
                DNI = o.DNI,
                Wearable = (o.Wearable == null) ? "Ninguno" : o.Wearable.idw,
                idWearable = (o.Wearable == null) ? 0 : o.Wearable.id_wearable,
                NumMedicos = o.PacienteMedicos.Count(),
                edad = DateTime.Today.AddTicks(-o.fechaNacimiento.Ticks).Year - 1
            });
        }

        // GET: api/Pacientes/ListarFaltantesW

        [HttpGet("[action]")]
        [Authorize(Roles = "Administrador")]
        public async Task<IEnumerable<PacienteFViewModel>> ListarFaltantesW()
        {
            var pacientes = await _context.Pacientes
                                            .Include(o => o.Wearable)
                                            .Where(o => o.Wearable == null)
                                            .ToListAsync();

            return pacientes.Select(o => new PacienteFViewModel
            {
                Id_paciente = o.Id_paciente,
                Nombre = o.Nombre,
                ApellidoP = o.ApellidoP,
                ApellidoM = o.ApellidoM,
                fechaNacimiento = o.fechaNacimiento,
                DNI = o.DNI,
                edad = DateTime.Today.AddTicks(-o.fechaNacimiento.Ticks).Year - 1
            });
        }

        // GET: api/Pacientes
        [Authorize(Roles = "Administrador,Médico")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteViewModel>> Visualizar([FromRoute] int id)
        {
            var paciente = await _context.Pacientes
                                        .Include(i => i.Wearable)
                                        .Include(i => i.PacienteMedicos)
                                        .FirstOrDefaultAsync(i => i.Id_paciente == id);

            if (paciente == null)
            {
                return BadRequest(new { message = "No existe el paciente consultado" });
            }

            return new PacienteViewModel
            {
                Id_paciente = paciente.Id_paciente,
                Nombre = paciente.Nombre,
                ApellidoP = paciente.ApellidoP,
                ApellidoM = paciente.ApellidoM,
                fechaNacimiento = paciente.fechaNacimiento,
                DNI = paciente.DNI,
                Wearable = (paciente.Wearable == null) ? "Ninguno" : paciente.Wearable.idw,
                idWearable = (paciente.Wearable == null) ? 0 : paciente.Wearable.id_wearable,
                NumMedicos = (paciente.PacienteMedicos == null) ? 0 : paciente.PacienteMedicos.Count(),
                edad = DateTime.Today.AddTicks(-paciente.fechaNacimiento.Ticks).Year - 1
            };
        }


        // GET: api/Pacientes/VisualizarDoctores
        [HttpGet("[action]/{id}")]
        public async Task<IEnumerable<PDoctoresViewModel>> VisualizarDoctores([FromRoute] int id)
        {
            var pacienteMedicos = await _context.PacienteMedicos
                                                .Where(p => p.Id_paciente == id)
                                                .Include(p => p.Medico)
                                                .ToListAsync();

            return pacienteMedicos.Select(o => new PDoctoresViewModel
            {
                idMedico = o.Id_medico,
                nombre = o.Medico.Nombre,
                apellidoP = o.Medico.ApellidoP,
                apellidoM = o.Medico.ApellidoM,
                DNI = o.Medico.DNI
            });

        }

        // GET: api/Pacientes/VisualizarDoctores
        [HttpGet("mobile/[action]")]
        [Authorize(Roles = "Paciente")]
        public async Task<IEnumerable<PDoctoresViewModelMobile>> VizualizeDoctors()
        {
            var idusuario = HelperController.readToken(HttpContext, _config["Jwt:Key"]);

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(c => c.idusuario == idusuario);

            var pacienteMedicos = await _context.PacienteMedicos
                                                .Where(p => p.Id_paciente == paciente.Id_paciente)
                                                .Include(p => p.Medico)
                                                .ToListAsync();

            return pacienteMedicos.Select(o => new PDoctoresViewModelMobile
            {
                idMedico = o.Id_medico,
                nombre = o.Medico.Nombre,
                apellidoP = o.Medico.ApellidoP,
                apellidoM = o.Medico.ApellidoM,
                DNI = o.Medico.DNI,
                urlIcon = "https://cdn-icons-png.flaticon.com/512/3143/3143629.png"

            });

        }


        // PUT: api/Pacientes/AdicionarWearable
        [HttpPut("[action]")]
        public async Task<IActionResult> AdicionarWearable([FromBody] AdicionarWearableModel model)
        {
            var paciente = await _context.Pacientes
               .FirstOrDefaultAsync(u => u.Id_paciente == model.idPaciente);

            if (paciente == null)
            {
                return Ok(new { message = "No se ha encontrado el paciente en la DB" });
            }

            paciente.id_wearable = model.idWearable;

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
        public async Task<IActionResult> AdicionarMedicos([FromBody] AdicionarMedicosModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paciente = await _context.Pacientes.FindAsync(model.idPaciente);

            var pacienteMedicos = await _context.PacienteMedicos
                .Where(dc => dc.Id_paciente == paciente.Id_paciente)
                .ToListAsync();

            if (paciente == null)
            {
                return Ok(new { message = "No se ha encontrado el paciente en la DB" });
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
                foreach (var idmedico in model.listIDMedicos)
                {
                    PacienteMedico pacienteMedico = new PacienteMedico()
                    {
                        Id_medico = idmedico,
                        Id_paciente = paciente.Id_paciente
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

        // PUT: api/Pacientes
        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] PacienteUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paciente = await _context.Pacientes.FindAsync(model.idPaciente);
            
            if (paciente == null)
            {
                return NotFound();
            }

            paciente.Nombre = model.Nombre;
            paciente.ApellidoM = model.ApellidoM;
            paciente.ApellidoP = model.ApellidoP;
            paciente.DNI = (int)model.DNI;
            paciente.fechaNacimiento = model.fechaNacimiento;
            paciente.id_wearable = model.id_wearable;

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


        // Delete: api/Pacientes/AdicionarMedicos
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            var user = await _context.Usuarios.FindAsync(paciente.idusuario);

            var pacienteMedicos = await _context.PacienteMedicos
                .Where(dc => dc.Id_paciente == paciente.Id_paciente)
                .ToListAsync();

            if (paciente == null)
            {
                return Ok(new { message = "No se ha encontrado el paciente en la DB" });
            }

            //Eiminar Usuario
            if (paciente != null)
            {
                try
                {
                    _context.Usuarios.Remove(user);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
              
            }

            ///Eliminar PacientesMedicos
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
                
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        // POST: api/Pacientes
        [HttpPost]
        public async Task<IActionResult> CrearPaciente([FromBody] PacienteCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var idusuario = await CrearUsuario(model.email);

            Paciente paciente = new Paciente()
            {
                Nombre = model.Nombre,
                ApellidoM = model.ApellidoM,
                ApellidoP = model.ApellidoP,
                DNI = (int)model.DNI,
                fechaNacimiento = model.fechaNacimiento,
                idusuario = idusuario,
                id_wearable = model.id_wearable,

            };

            _context.Pacientes.Add(paciente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            try
            {
                if (model.listMedicos != null)
                {
                    if (model.listMedicos.Count() > 0)
                    {
                        foreach (var idmedico in model.listMedicos)
                        {
                            PacienteMedico pacienteMedico = new PacienteMedico()
                            {
                                Id_medico = idmedico,
                                Id_paciente = paciente.Id_paciente
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

        private async Task<IActionResult> EliminaMedicos(Paciente paciente, List<int> listidmedicos)
        {

            var pacienteMedicos = await _context.PacienteMedicos
                .Where(dc => dc.Id_paciente == paciente.Id_paciente)
                .ToListAsync();

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
                idrol = 1,
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

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id_paciente == id);
        }
    }
}
