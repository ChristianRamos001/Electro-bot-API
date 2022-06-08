using API.Entity.Administracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Data.Administracion
{
    public class CentroSaludMap : IEntityTypeConfiguration<CentroSalud>
    {
        public void Configure(EntityTypeBuilder<CentroSalud> builder)
        {
            builder.ToTable("CentrosSalud")
                .HasKey(c => c.IDcentroSalud);
        }

    }
}
