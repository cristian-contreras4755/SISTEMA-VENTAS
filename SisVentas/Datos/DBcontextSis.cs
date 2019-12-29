using Datos.Almacen;
using Datos.usuario;
using Entidad.Almacen;
using Entidad.Usuario;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
  public  class DBcontextSis : DbContext
    {

        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DBcontextSis(DbContextOptions<DBcontextSis> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ArcticuloMap());
            modelBuilder.ApplyConfiguration(new RolMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
        }



    }
}
