using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidad.Ventas;
using Microsoft.AspNetCore.Authorization;
using SisWeb.Models.Ventas.Venta;

namespace SisWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly DBcontextSis _context;

        public VentasController(DBcontextSis context)
        {
            _context = context;
        }


        [Authorize(Roles = "Administrador,Vendedor")]
        // GET: api/Articulos
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> Listar()
        {

            var venta = await _context.Venta
                            .Include(v => v.usuario)
                            .Include(v => v.persona)
                            .OrderByDescending(v => v.idventa)
                            .Take(100)
                            .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                tipo_comprobante = v.tipo_comprobante,
                serie_comprobante = v.serie_comprobante,
                num_comprobante = v.num_comprobante,
                fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,
                total = v.total,
                estado = v.estado

            });
        }


        [Authorize(Roles = "Administrador,Vendedor")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<VentaViewModel>> ListarFiltro([FromRoute] string texto)
        {

            var venta = await _context.Venta
                 .Include(v => v.usuario)
                 .Include(v => v.persona)
                 .Where(v => v.num_comprobante.Contains(texto))
                 .OrderByDescending(v => v.idventa)
                 .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                tipo_comprobante = v.tipo_comprobante,
                serie_comprobante = v.serie_comprobante,
                num_comprobante = v.num_comprobante,
                fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,
                total = v.total,
                estado = v.estado
            });
        }


        [Authorize(Roles = "Administrador,Vendedor")]
        [HttpGet("[action]/{idventa}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles(int idventa)
        {

            var detalle = await _context.DetallesVentas
              .Include(a => a.articulo)
              .Where(d => d.idventa == idventa)
              .ToListAsync();

            return detalle.Select(d => new DetalleViewModel
            {
                idarticulo = d.idarticulo,
                articulo = d.articulo.nombre,
                cantidad = d.cantidad,
                precio = d.precio,
                descuento = d.descuento
            });

        }




        // POST: api/Ventas/Crear
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fechaHora = DateTime.Now;

            // var generar serie 

            var serie = await _context.Serie.Where(a => a.Cd_TD == model.tipo_comprobante).Where(a => a.IB_Estado == true).FirstOrDefaultAsync();
            int correlativo;
            string nroDoc="";
            if (serie!=null)
            {
             var    ventas = await _context.Venta.Where(a => a.tipo_comprobante == model.tipo_comprobante).Where(a => a.Id_Serie== serie.Id_Serie).CountAsync();

                int caracteres = ventas.ToString().Length;
                string text = "00000000";
                 correlativo = ventas + 1;
                 nroDoc = text.Remove(0, correlativo.ToString().Length) + correlativo;
            }
            else
            {
                return BadRequest(new { error="no se pudo generar correlativo"});
            }


            Venta venta = new Venta
            {
                idcliente = model.idcliente,
                idusuario = model.idusuario,
                Id_Serie = serie.Id_Serie,
                tipo_comprobante = model.tipo_comprobante,
                serie_comprobante = serie.NroSerie,
                num_comprobante = nroDoc,
                fecha_hora = fechaHora,
                impuesto = model.impuesto,
                total = model.total,
                estado = "Aceptado"
            };


            try
            {
                _context.Venta.Add(venta);
                await _context.SaveChangesAsync();

                var id = venta.idventa;
                foreach (var det in model.detalles)
                {
                    DetalleVenta detalle = new DetalleVenta
                    {
                        idventa = id,
                        idarticulo = det.idarticulo,
                        cantidad = det.cantidad,
                        precio = det.precio,
                        descuento = det.descuento
                    };
                    _context.DetallesVentas.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                string Error = ex.Message;
                return BadRequest();
            }

            return Ok();
        }


        /*
        // PUT: api/Ventas/Anular/1
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var venta = await _context.Venta.FirstOrDefaultAsync(v => v.idventa == id);

            if (venta == null)
            {
                return NotFound();
            }

            venta.estado = "Anulado";

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

        */


        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {



            if (id <= 0)

            {

                return BadRequest();

            }



            var venta = await _context.Venta.FirstOrDefaultAsync(v => v.idventa == id);



            if (venta == null)

            {

                return NotFound();

            }



            venta.estado = "Anulado";



            try

            {

                await _context.SaveChangesAsync();

                // Inicio de código para devolver stock

                // 1. Obtenemos los detalles

                var detalle = await _context.DetallesVentas.Include(a => a.articulo).Where(d => d.idventa == id).ToListAsync();

                //2. Recorremos los detalles

                foreach (var det in detalle)

                {

                    //Obtenemos el artículo del detalle actual

                    var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.idarticulo == det.articulo.idarticulo);

                    //actualizamos el stock

                    articulo.stock = det.articulo.stock + det.cantidad;

                    //Guardamos los cambios

                    await _context.SaveChangesAsync();

                }

                // Fin del código para devolver stock

            }

            catch (DbUpdateConcurrencyException)

            {

                // Guardar Excepción

                return BadRequest();

            }



            return Ok();

        }





        private bool VentaExists(int id)
        {
            return _context.Venta.Any(e => e.idventa == id);
        }
    }
}
