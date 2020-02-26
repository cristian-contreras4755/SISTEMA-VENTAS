using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidad.Almacen;
using SisWeb.Models.Almacen.Articulo;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace SisWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly DBcontextSis _context;

        public ArticulosController(DBcontextSis context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador,Almacenero")]
        // GET: api/Articulos
        [HttpGet("[action]")]
        public async Task<IEnumerable<ArticuloViewModel>> Listar()
        {
            var articulo = await _context.Articulos.Include(a=>a.categoria).ToListAsync();


            return articulo.Select(a => new ArticuloViewModel
            {
                idarticulo = a.idarticulo,
                idcategoria = a.idcategoria,
                categoria = a.categoria.nombre,
                codigo = a.codigo,
                nombre = a.nombre,
                stock = a.stock,
                precio_venta = a.precio_venta,
               descripcion = a.descripcion,
                condicion = a.condicion

            });
        }



        [Authorize(Roles = "Administrador,Almacenero")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarIngreso([FromRoute] string texto)
        {
            var articulo = await _context.Articulos.Include(a => a.categoria)
                .Where(n=>n.nombre.Contains(texto))
                .Where(n => n.condicion==true)
                .ToListAsync();


            return articulo.Select(a => new ArticuloViewModel
            {
                idarticulo = a.idarticulo,
                idcategoria = a.idcategoria,
                categoria = a.categoria.nombre,
                codigo = a.codigo,
                nombre = a.nombre,
                stock = a.stock,
                precio_venta = a.precio_venta,
                descripcion = a.descripcion,
                condicion = a.condicion

            });
        }



        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarVenta([FromRoute] string texto)
        {
            var articulo = await _context.Articulos.Include(a => a.categoria)
                .Where(n => n.nombre.Contains(texto)).Where(a=>a.stock>0)
                .Where(n => n.condicion == true)
                .ToListAsync();


            return articulo.Select(a => new ArticuloViewModel
            {
                idarticulo = a.idarticulo,
                idcategoria = a.idcategoria,
                categoria = a.categoria.nombre,
                codigo = a.codigo,
                nombre = a.nombre,
                stock = a.stock,
                precio_venta = a.precio_venta,
                descripcion = a.descripcion,
                condicion = a.condicion

            });
        }




        [Authorize(Roles = "Administrador,Almacenero")]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> Mostrar([FromRoute] int id)
        {
            var articulo = await _context.Articulos.Include(a=>a.categoria).SingleOrDefaultAsync(a=>a.idarticulo==id);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre,
                descripcion = articulo.descripcion,
                stock = articulo.stock,
                precio_venta = articulo.precio_venta,
                condicion = articulo.condicion
            });
        }


        [Authorize(Roles = "Administrador,Almacenero")]
        [HttpGet("[action]/{codigo}")]
        public async Task<ActionResult> BuscarCodigoIngreso([FromRoute] string codigo)
        {
            var articulo = await _context.Articulos.Include(a => a.categoria).
                Where(a=>a.condicion==true).
                SingleOrDefaultAsync(a => a.codigo == codigo);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre,
                descripcion = articulo.descripcion,
                stock = articulo.stock,
                precio_venta = articulo.precio_venta,
                condicion = articulo.condicion
            });
        }

        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{codigo}")]
        public async Task<ActionResult> BuscarCodigoVenta([FromRoute] string codigo)
        {
            var articulo = await _context.Articulos.Include(a => a.categoria).
                Where(a => a.condicion == true).Where(A=>A.stock>0).
                SingleOrDefaultAsync(a => a.codigo == codigo);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre,
                descripcion = articulo.descripcion,
                stock = articulo.stock,
                precio_venta = articulo.precio_venta,
                condicion = articulo.condicion
            });
        }






        [Authorize(Roles = "Administrador,Almacenero")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Model.idcategoria <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(c => c.idarticulo == Model.idarticulo);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.nombre = Model.nombre;
            articulo.idcategoria = Model.idcategoria;
            articulo.codigo = Model.codigo;
            articulo.nombre = Model.nombre;
            articulo.precio_venta = Model.precio_venta;
            articulo.stock = Model.stock;
            articulo.descripcion = Model.descripcion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        [Authorize(Roles = "Administrador,Almacenero")]
        // POST: api/Categoria
        [HttpPost("[action]")]
        [EnableCors("Todos")]
        public async Task<ActionResult> Crear([FromBody]CrearViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Articulo articulo = new Articulo
            {
                idcategoria = model.idcategoria,
                codigo = model.codigo,
                nombre = model.nombre,
                precio_venta = model.precio_venta,
                stock = model.stock,
                descripcion = model.descripcion,
                condicion = true

            };

            _context.Articulos.Add(articulo);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        [Authorize(Roles = "Administrador,Almacenero")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(c => c.idarticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.condicion = false;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        [Authorize(Roles = "Administrador,Almacenero")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(c => c.idarticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.condicion = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }




        private bool ArticuloExists(int id)
        {
            return _context.Articulos.Any(e => e.idarticulo == id);
        }
    }
}
