using Datos.Almacen;
using Entidad.Almacen;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
  public  class DBcontextSis : DbContext
    {

        public DbSet<Categoria> categorias { get; set; }

        public DBcontextSis(DbContextOptions<DBcontextSis> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoriaMap());
        }



    }
}
