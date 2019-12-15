using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidad.Almacen;
using SisWeb.Models.Almacen.Categoria;
using Microsoft.AspNetCore.Cors;

namespace SisWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly DBcontextSis _context;

        public CategoriaController(DBcontextSis context)
        {
            _context = context;
        }

        // GET: api/Categoria
        [HttpGet("[action]")]
        public async Task<IEnumerable<CategoriaViewModel>> Listar()
        {
            var categoria= await _context.categorias.ToListAsync();


            return categoria.Select(c => new CategoriaViewModel
            {
                idcategoria = c.idcategoria,
                 nombre = c.nombre,
                 descripcion = c.descripcion,
                 condicion = c.condicion
            }) ; 
        }




        // GET: api/Categoria/5
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Categoria>> Mostrar( [FromRoute] int id)
        {
            var categoria = await _context.categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(   new CategoriaViewModel
            {
                idcategoria = categoria.idcategoria,
                nombre = categoria.nombre,
                descripcion = categoria.descripcion,
                condicion = categoria.condicion
            });
        }

        // PUT: api/Categoria/5
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

            var categoria = await _context.categorias.FirstOrDefaultAsync(c => c.idcategoria == Model.idcategoria);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.nombre = Model.nombre;
            categoria.descripcion = Model.descripcion;

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

        // POST: api/Categoria
        [HttpPost("[action]")]
        [EnableCors("Todos")]
        public async Task<ActionResult> Crear([FromBody]CrearViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categoria categoria = new Categoria
            {
                nombre = model.nombre,
                descripcion = model.descripcion,
                condicion = true
            };

            _context.categorias.Add(categoria);
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

        // DELETE: api/Categoria/5
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<Categoria>> Eliminar([FromRoute] int id)
        {
            var categoria = await _context.categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            _context.categorias.Remove(categoria);
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

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
           
            if (id <= 0)
            {
                return BadRequest();
            }

            var categoria = await _context.categorias.FirstOrDefaultAsync(c => c.idcategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.condicion = false;
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

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
        
            if (id <= 0)
            {
                return BadRequest();
            }

            var categoria = await _context.categorias.FirstOrDefaultAsync(c => c.idcategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.condicion = true;
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

    }
}
