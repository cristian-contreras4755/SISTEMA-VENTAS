using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SisWeb.Models.Ventas.TipoDocumento
{
    public class Serie
    {
        [Required]
        public int Id_Serie { get; set; }
        public string NroSerie { get; set; }
        [Required]
        public string Cd_TD { get; set; }
        public int Desde { get; set; }
        public int Hasta { get; set; }
        public bool IB_Estado { get; set; }

    }
}
