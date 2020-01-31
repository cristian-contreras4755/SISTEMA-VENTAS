using Entidad.Almacen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Almacen
{
    public class IngresoMap : IEntityTypeConfiguration<Ingreso>
    {
        public void Configure(EntityTypeBuilder<Ingreso> builder)
        {
            builder.ToTable("ingreso").HasKey(I => I.idingreso);
            builder.HasOne(i => i.persona).
                WithMany(p => p.ingresos).
                HasForeignKey(I => I.idproveedor);
        }

    }
}
