using Entidad.Ventas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Ventas
{
    class VentaMap : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.ToTable("venta").HasKey(v=> v.idventa);

            builder.HasOne(p => p.persona).WithMany(p => p.ventas).HasForeignKey(v=>v.idcliente);
            builder.HasOne(s => s.serie).WithMany(s => s.ventas).HasForeignKey(v => v.Id_Serie);
        }
    }
}
