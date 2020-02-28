using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace SisWeb.Models.Common
{
    public class CampoConsultaViewModel
    {
        [Required]
        public string campo_busqueda { get; set; }
        [Required]
        public string text_busqueda{ get; set; }
    }
}
