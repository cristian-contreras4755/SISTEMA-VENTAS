using Entidad.Ventas.TipoDocumento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Ventas.TipoDocumento
{
  public  class SerieMap : IEntityTypeConfiguration<Serie>
    {
        public void Configure(EntityTypeBuilder<Serie> builder)
        {
            builder.ToTable("Serie")
               .HasKey(p => p.Id_Serie);
        }

    }
}
