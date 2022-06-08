using API.Entity.Monitoreo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Data.Monitoreo
{
    public class WearableMap : IEntityTypeConfiguration<Wearable>
    {
        public void Configure(EntityTypeBuilder<Wearable> builder)
        {
            builder.ToTable("Wearable")
                .HasKey(c => c.id_wearable);
        }

    }
}
