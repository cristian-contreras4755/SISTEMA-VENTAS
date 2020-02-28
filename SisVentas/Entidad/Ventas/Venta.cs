using Entidad.Usuarios;
using Entidad.Ventas.TipoDocumento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entidad.Ventas
{
  public  class Venta
    {

        public int idventa { get; set; }
        [Required]
        public int idcliente { get; set; }
        [Required]
        public int idusuario { get; set; }

        [Required]
        public int Id_Serie { get; set; }

        [Required]
        public string tipo_comprobante { get; set; }
        public string serie_comprobante { get; set; }
        [Required]
        public string num_comprobante { get; set; }
        [Required]
        public DateTime fecha_hora { get; set; }
        [Required]
        public decimal impuesto { get; set; }
        [Required]
        public decimal total { get; set; }
        [Required]
        public string estado { get; set; }

        public ICollection<DetalleVenta> detalles { get; set; }
        public Usuario usuario { get; set; }
        public Persona persona { get; set; }
        public Serie serie { get; set; }

    }
}
