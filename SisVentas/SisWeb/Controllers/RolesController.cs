using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidad.Usuarios;
using SisWeb.Models.Usuarios.Rol;
using Microsoft.AspNetCore.Authorization;

namespace SisWeb.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DBcontextSis _context;

        public RolesController(DBcontextSis context)
        {
            _context = context;
        }


        // GET: api/Categoria
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolViewModel>> Listar()
        {
            var rol = await _context.Roles.ToListAsync();


            return rol.Select(r => new RolViewModel
            {
                idrol = r.idrol,
                nombre = r.nombre,
                descripcion = r.descripcion,
                condicion = r.condicion
            });
        }



        // GET: api/Categoria
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> Select()
        {
            var rol = await _context.Roles.Where(a => a.condicion == true).ToListAsync();

            return rol.Select(c => new SelectViewModel
            {
                idrol = c.idrol,
                nombre = c.nombre,
            });
        }








        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.idrol == id);
        }
    }
}
