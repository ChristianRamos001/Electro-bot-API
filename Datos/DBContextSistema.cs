using API.Data.Administracion;
using API.Data.Monitoreo;
using API.Data.Seguridad;
using API.Entity.Administracion;
using API.Entity.Monitoreo;
using API.Entity.Seguridad;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Data
{
    public class DBContextSistema : DbContext
    {
        public DbSet<User> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Wearable> Wearables { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<PacienteMedico> PacienteMedicos { get; set; }
        public DbSet<CentroSalud> CentrosSalud { get; set; }


        public DBContextSistema(DbContextOptions<DBContextSistema> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RolMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new WearableMap());
            modelBuilder.ApplyConfiguration(new MedicoMap());
            modelBuilder.ApplyConfiguration(new PacienteMap());
            modelBuilder.ApplyConfiguration(new PacienteMedicoMap());
            modelBuilder.ApplyConfiguration(new CentroSaludMap());
        }
    }
}
