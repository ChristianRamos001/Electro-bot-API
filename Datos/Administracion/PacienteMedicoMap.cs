using API.Entity.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Data.Administracion
{
    public class PacienteMedicoMap : IEntityTypeConfiguration<PacienteMedico>
    {
        public void Configure(EntityTypeBuilder<PacienteMedico> builder)
        {
            builder.ToTable("PacienteMedico")
                .HasKey(c => c.IDPacienteMedico);
        }

    }
}
