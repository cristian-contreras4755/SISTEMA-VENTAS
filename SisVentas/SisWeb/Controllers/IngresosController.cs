using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidad.Almacen;
using Microsoft.AspNetCore.Authorization;
using SisWeb.Models.Almacen.Ingreso;

namespace SisWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresosController : ControllerBase
    {
        private readonly DBcontextSis _context;

        public IngresosController(DBcontextSis context)
        {
            _context = context;
        }


        [Authorize(Roles = "Administrador,Almacenero")]
        // GET: api/Articulos
        [HttpGet("[action]")]
        public async Task<IEnumerable<IngresoViewModel>> Listar()
        {
           
            var ingreso = await _context.Ingreso
                            .Include(i => i.usuario)
                            .Include(i => i.persona)
                            .OrderByDescending(i => i.idingreso)
                            .Take(100)
                            .ToListAsync();

            return ingreso.Select(i => new IngresoViewModel
            {
                idingreso = i.idingreso,
                idproveedor = i.idproveedor,
                proveedor = i.persona.nombre,
                idusuario = i.idusuario,
                usuario = i.usuario.nombre,
                tipo_comprobante = i.tipo_comprobante,
                serie_comprobante = i.serie_comprobante,
                num_comprobante = i.num_comprobante,
                fecha_hora = i.fecha_hora,
                impuesto = i.impuesto,
                total = i.total,
                estado = i.estado
            });
        }




        [Authorize(Roles = "Administrador,Almacenero")]
        [HttpGet("[action]/{idingreso}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles( int idingreso)
        {

            var detalle = await _context.detallesIngreso
                               .Include(i => i.articulo)
                                .Where(di=>di.idingreso==idingreso)
                               .ToListAsync();

            return detalle.Select(i => new DetalleViewModel
            {
                idarticulo = i.idarticulo,
                articulo = i.articulo.nombre,
                cantidad = i.cantidad,
                precio = i.precio
            });
        }




        // POST: api/Ingresos/Crear
        // [Authorize(Roles = "Administrador,Almacenero")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fechaHora = DateTime.Now;

            Ingreso ingreso = new Ingreso
            {
                idproveedor = model.idproveedor,
                idusuario = model.idusuario,
                tipo_comprobante = model.tipo_comprobante,
                serie_comprobante = model.serie_comprobante,
                num_comprobante = model.num_comprobante,
                fecha_hora = fechaHora,
                impuesto = model.impuesto,
                total = model.total,
                estado = "Aceptado"
            };


            try
            {
                _context.Ingreso.Add(ingreso);
                await _context.SaveChangesAsync();

                var id = ingreso.idingreso;
                foreach (var det in model.detalles)
                {
                    DetalleIngreso detalle = new DetalleIngreso
                    {
                        idingreso = id,
                        idarticulo = det.idarticulo,
                        cantidad = det.cantidad,
                        precio = det.precio
                    };
                    _context.detallesIngreso.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }





        private bool IngresoExists(int id)
        {
            return _context.Ingreso.Any(e => e.idingreso == id);
        }
    }
}
