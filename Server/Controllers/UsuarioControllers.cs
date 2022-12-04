using BDCarrito;
using BDCarrito.Entidades;
using Carrito.Shared.Modedls.DTOBack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carrito.Server.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class UsuarioControllers : ControllerBase
    {
        private readonly BDContext context;

        public UsuarioControllers(BDContext context)
        {
            this.context = context;
        }

        [HttpGet("GetAllUsuario")]

        public async Task<ActionResult<List<UsuarioDto>>> GetAll()
        {
            List<Usuario> Usuarios = await this.context.TablaUsuario.ToListAsync();

            List<UsuarioDto> ListUserMapper = new List<UsuarioDto> { };

            Usuarios.ForEach(Usuario =>
            {
                ListUserMapper.Add(new UsuarioDto
                {
                    Id = Usuario.Id,
                    DNI = Usuario.DNI,
                    Vip = Usuario.Vip
                    
                });
            });
            return Ok(ListUserMapper);
        }

        [HttpPost]
        //verbo es el http post, pero a la base de datos ingresa como un insert
        public async Task<ActionResult<string>> AddUser(Usuario usuario)
        {
            try
            {
                context.TablaUsuario.Add(usuario);
                await context.SaveChangesAsync();
                return("se ha creado un usuario correctamente");
            }
            catch (Exception e)
            {
                return BadRequest("Ocurrio un error al crear el usuario. Intentelo nuevamente");
            }

        }
    }
}
