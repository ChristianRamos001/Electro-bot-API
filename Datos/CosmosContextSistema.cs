using API.Entity.Monitoreo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Data
{
    public class CosmosContextSistema : DbContext
    {
        public DbSet<RecordECG> RecordsECG { get; set; }

        public CosmosContextSistema(DbContextOptions options) : base(options)
        {
            Database.EnsureCreatedAsync();
        }

        public CosmosContextSistema()
        {
            Database.EnsureCreatedAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecordECG>().ToContainer("Records-ECG").HasPartitionKey("userID").HasNoDiscriminator().HasKey(c => c.id);
            modelBuilder.Entity<RecordECG>().OwnsMany(p => p.data);
            //modelBuilder.Entity<DataECG>().OwnsOne(p => p.dataECG);
        }
    }
}
