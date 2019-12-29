﻿using Entidad.Ventas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Ventas
{
    public class PersonaMap : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("persona")
               .HasKey(p => p.idpersona);
        }
    }
}
