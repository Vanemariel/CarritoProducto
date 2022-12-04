using BDCarrito;
using BDCarrito.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carrito.Server.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class CarritoControllers : ControllerBase
    {
        private readonly BDContext context;

        public CarritoControllers(BDContext context)
        {
            this.context = context;
        }


      
        [HttpPost]
        public async Task<ActionResult<int>> CreateCarrito(int DNIUsuario)
        {
            try
            {
                BDCarrito.Entidades.Carrito nuevoCarrito = new BDCarrito.Entidades.Carrito
                {
                    PrecioTotal = 0,
                    DniUsuario = DNIUsuario,
                    Productos = new List<Producto>()
                };

                context.TablaCarrito.Add(nuevoCarrito);
                await context.SaveChangesAsync();


                List<BDCarrito.Entidades.Carrito> listadoCarritos = await context.TablaCarrito
                    .Where(carrito => carrito.DniUsuario == DNIUsuario)
                    .ToListAsync();

                return listadoCarritos.Last().Id;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{idCarrito:int}")] 
        public async Task<ActionResult> Delete(int idCarrito)
        {

            try
            {
                if (idCarrito <= 0)
                {
                    throw new Exception("No es correcto");
                }

                BDCarrito.Entidades.Carrito Carro = await context.TablaCarrito.Where(x => x.Id == idCarrito).FirstOrDefaultAsync();

                if (Carro == null)
                {
                    throw new Exception($"No existe el Producto con id igual a {idCarrito}.");
                }

                context.TablaCarrito.Remove(Carro);
                await context.SaveChangesAsync();

                return Ok($"El Carrito ha sido borrado.");
            }
            catch (Exception e) //se captura la excepcion del try
            {
                return BadRequest(e.Message);
            }
        }

    }
}
