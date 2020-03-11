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
using SisWeb.Models.Utilitarios;
using System.Data.SqlClient;
using System.Data;
using Entidad.StoreProcedure;
using SisWeb.Models.Reporte;
using System.Data.Common;

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
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                //cliente
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento = v.persona.num_documento,
                direccion = v.persona.direccion,

                total_text = Util.NumeroALetras(v.total.ToString()),

 
                //comprobante
               nom_comprobante = v.tipo_comprobante,
                num_serie_comprobante = v.serie_comprobante + "-" + v.num_comprobante,

                fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,

                igv = v.igv,
                bim = v.bim,

                total = v.total,
                estado = v.estado

            }) ; 
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
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                //cliente
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento = v.persona.num_documento,
                direccion = v.persona.direccion,

                //comprobante
                nom_comprobante = v.tipo_comprobante,
                num_serie_comprobante = v.serie_comprobante + "-" + v.num_comprobante,
                total_text = Util.NumeroALetras(v.total.ToString()),

                fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,

                igv = v.igv,
                bim = v.bim,

                total = v.total,
                estado = v.estado

                /*
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                num_documento = v.persona.num_documento,
                direccion = v.persona.direccion,
                nom_comprobante = v.tipo_comprobante,
                num_serie_comprobante = v.serie_comprobante + "-" + v.num_comprobante,
                fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,
                total = v.total,
                estado = v.estado*/
            });
        }

    //    [Authorize(Roles = "Administrador,Vendedor")]
        [HttpGet("[action]/{id}")]
        public async Task<VentaDocModel> VentaConsUn([FromRoute] int id)
        {
            var venta = await _context.Venta
          .Include(v => v.usuario)
           .Include(v => v.persona)
           .Where(v => v.idventa== id)
           .FirstAsync();

            if (venta==null) {
                return new VentaDocModel();
            }


            VentaDocModel ventaDocModel = new VentaDocModel();
            List<DetalleViewModel> listaDet = new List<DetalleViewModel>();


                ventaDocModel.idventa = venta.idventa;
            ventaDocModel.idusuario = venta.idusuario;
            ventaDocModel.usuario = venta.usuario.nombre;
            //cliente
            ventaDocModel.idcliente = venta.idcliente;
            ventaDocModel.cliente = venta.persona.nombre;
            ventaDocModel.num_documento = venta.persona.num_documento;
            ventaDocModel.direccion = venta.persona.direccion;

            //comprobante
            ventaDocModel.nom_comprobante = venta.tipo_comprobante;
            ventaDocModel.num_serie_comprobante = venta.serie_comprobante + "-" + venta.num_comprobante;
            ventaDocModel.total_text = Util.NumeroALetras(venta.total.ToString());

            ventaDocModel.fecha_hora = venta.fecha_hora;
            ventaDocModel.impuesto = venta.impuesto;

            ventaDocModel.igv = venta.igv;
            ventaDocModel.bim = venta.bim;

            ventaDocModel.total = venta.total;
            ventaDocModel.estado = venta.estado;


            var ventaDet = await _context.DetallesVentas
            .Include(v => v.articulo)
            .Where(v => v.idventa == id)
            .ToListAsync();


            List<DetalleViewModel> detalleViewModelgen = new List<DetalleViewModel>();
            foreach (var item in ventaDet)
            {
                DetalleViewModel detalleViewModel1 = new DetalleViewModel();
                detalleViewModel1.articulo = item.articulo.descripcion;
                detalleViewModel1.cantidad = item.cantidad;
                detalleViewModel1.precio = item.precio;
                detalleViewModel1.precio = item.precio;
                detalleViewModelgen.Add(detalleViewModel1);
            }
            ventaDocModel.Detalle = detalleViewModelgen;


            return ventaDocModel;
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
                igv = model.igv,
                bim = model.bim,
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
            return Ok(venta.idventa);
        }


        // GET: api/Articulos
        [HttpGet("[action]")]
        public  List<sp_venta_diaria>VentaDiaria([FromBody]  model_consulta _model)
        {
            List<sp_venta_diaria> lista = new List<sp_venta_diaria>();
            try
            {
                                var param = new SqlParameter[] {
                                        new SqlParameter() {
                                            ParameterName = "@FechaInicio",
                                            SqlDbType =  System.Data.SqlDbType.DateTime,
                                            Direction = System.Data.ParameterDirection.Input,
                                            Value =_model.f_inicial
                                        },
                                        new SqlParameter() {
                                            ParameterName = "@FechaFin",
                                            SqlDbType =  System.Data.SqlDbType.DateTime,
                                            Direction = System.Data.ParameterDirection.Input,
                                                Value = _model.f_final
                                            }};

                SqlConnection sqlConnection = new SqlConnection("data source=DESKTOP-699F64C;initial catalog=db_ventasudemy; user id=sa; Password=cristian261.;persist security info=True;" /* _context.Database.GetDbConnection().ConnectionString*/);
                                     
                                            var cmm = sqlConnection.CreateCommand();
                                            cmm.CommandType = System.Data.CommandType.StoredProcedure;
                                            cmm.CommandText = "sp_venta_diaria";
                                            cmm.Parameters.AddRange(param);
                                            cmm.Connection = sqlConnection;
                                               sqlConnection.Open();
                                            var reader = cmm.ExecuteReader();

                                                while (reader.Read())
                                                {
                                                    sp_venta_diaria modelo = new sp_venta_diaria();
                                                    modelo.fecha = Convert.ToDateTime(reader["fecha"]);
                                                    modelo.total = Convert.ToDecimal(reader["total"]);
                                                    modelo.cantidad = Convert.ToInt32(reader["cantidad"]);
                                                    lista.Add(modelo);
                                                }

            }
            catch (Exception ex)
            {
                string s=ex.Message;

                return lista;
            }
            return lista;

        }


        // GET: api/Articulos
        [HttpGet("[action]")]
        public List<VentaMensualViewModel> VentaMensual([FromBody]  model_consulta _model)
        {
            /*async Task<IEnumerable<sp_venta_diaria>>*/
            // IEnumerable<sp_venta_diaria> s= new IEnumerable<sp_venta_diaria>();
            List<VentaMensualViewModel> lista = new List<VentaMensualViewModel>();
            try
            {


                var param = new SqlParameter[] {
                                        new SqlParameter() {
                                            ParameterName = "@FechaInicio",
                                            SqlDbType =  System.Data.SqlDbType.DateTime,
                                            Direction = System.Data.ParameterDirection.Input,
                                            Value =_model.f_inicial
                                        },
                                        new SqlParameter() {
                                            ParameterName = "@FechaFin",
                                            SqlDbType =  System.Data.SqlDbType.DateTime,
                                            Direction = System.Data.ParameterDirection.Input,
                                                Value = _model.f_final
                                            }};


                SqlConnection sqlConnection = new SqlConnection("data source=DESKTOP-699F64C;initial catalog=db_ventasudemy; user id=sa; Password=cristian261.;persist security info=True;" /* _context.Database.GetDbConnection().ConnectionString*/);



                    var cmm = sqlConnection.CreateCommand();
                    cmm.CommandType = System.Data.CommandType.StoredProcedure;
                    cmm.CommandText = "sp_venta_mensual";
                    cmm.Parameters.AddRange(param);
                    cmm.Connection = sqlConnection;
                   sqlConnection.Open();
                    var reader = cmm.ExecuteReader();
                
                while (reader.Read())
                    {
                        VentaMensualViewModel modelo = new VentaMensualViewModel();
                        modelo.anio = Convert.ToString(reader["anio"]);
                        modelo.mes= Convert.ToString(reader["mes"]);
                        modelo.total = Convert.ToDecimal(reader["total"]);
                        modelo.cantidad = Convert.ToInt32(reader["cantidad"]);
                        lista.Add(modelo);
                    }


            }
            catch (Exception ex)
            {

                return lista; 
            }

            return lista;
        }








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
