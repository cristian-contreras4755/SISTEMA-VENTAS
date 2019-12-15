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



        // GET: api/Articulos/5
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
        




        private bool ArticuloExists(int id)
        {
            return _context.Articulos.Any(e => e.idarticulo == id);
        }
    }
}
