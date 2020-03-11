using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SisWeb.Models.Reporte
{
    public class VentaMensualViewModel
    {
        public string anio { get; set; }
        public string mes { get; set; }
        public decimal total { get; set; }
        public int cantidad { get; set; }
    }
}
