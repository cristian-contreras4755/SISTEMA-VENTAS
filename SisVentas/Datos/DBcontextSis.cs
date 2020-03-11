using Datos.Almacen;
using Datos.usuario;
using Datos.Ventas;
using Datos.Ventas.TipoDocumento;
using Entidad.Almacen;
using Entidad.StoreProcedure;
using Entidad.Usuarios;
using Entidad.Ventas;
using Entidad.Ventas.TipoDocumento;
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
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Ingreso> Ingreso { get; set; }
        public DbSet<DetalleIngreso> detallesIngreso { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<DetalleVenta> DetallesVentas { get; set; }
        public DbSet<Serie> Serie { get; set; }
        public DbSet<sp_venta_diaria> sp_venta_diaria { get; set; }










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
            modelBuilder.ApplyConfiguration(new PersonaMap());
            modelBuilder.ApplyConfiguration(new IngresoMap());
            modelBuilder.ApplyConfiguration(new DetalleIngresoMap());
            modelBuilder.ApplyConfiguration(new VentaMap());
            modelBuilder.ApplyConfiguration(new VentaDetalleMap());
            modelBuilder.ApplyConfiguration(new SerieMap());
        }






    }
}
