using System;
using System.Collections.Generic;
using System.Text;

namespace Entidad.StoreProcedure
{
   public class sp_venta_diaria
    {
        public DateTime fecha { get; set; }
        public decimal total { get; set; }
        public int cantidad { get; set; }
    }
}
