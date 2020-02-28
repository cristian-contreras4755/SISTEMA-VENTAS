using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SisWeb.Models.Ventas.Venta
{
    public class VentaViewModel
    {
        public int idventa { get; set; }
        public int idcliente { get; set; }
        public string cliente { get; set; }
        public string num_documento { get; set; }
        public string direccion { get; set; }
        public int idusuario { get; set; }
        public string usuario { get; set; }
        public string nom_comprobante { get; set; }
        public string num_serie_comprobante { get; set; }
        public DateTime fecha_hora { get; set; }
        public decimal impuesto { get; set; }
        public decimal total { get; set; }
        public string estado { get; set; }
    }
}
