using BDCarrito;
using BDCarrito.Entidades;
using Carrito.Shared.Modedls.DTOBack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carrito.Server.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class ProductoControllers : ControllerBase
    {
        private readonly BDContext context;

        public ProductoControllers(BDContext context)
        {
            this.context = context;
        }



        [HttpGet("GetAllProducto")]

        public async Task<ActionResult<List<ProductoDto>>> GetAll()
        {
            List<Producto> productos = await this.context.TablaProductos.ToListAsync();

            List<ProductoDto> ListUserMapper = new List<ProductoDto> { };

            productos.ForEach(productos =>
            {
                ListUserMapper.Add(new ProductoDto
                {
                    Nombre = productos.Nombre,
                    Precio = productos.Precio
                });
            });
            return Ok(ListUserMapper);
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddProducto(Producto prod)
        {
            try
            {
                context.TablaProductos.Add(prod);
                await context.SaveChangesAsync();
                return "Se ha insetado correctamente";
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProducto(int id, [FromBody] Producto Producto)
        {
           
            try
            {
                Producto productoencontrado = await context.TablaProductos.Where(x => x.Id == id).FirstOrDefaultAsync();
              
                if (productoencontrado == null)
                {
                    throw new Exception("No existe el Producto a modificar");
                }

                productoencontrado.Nombre = Producto.Nombre;
                productoencontrado.Precio = Producto.Precio;
                productoencontrado.Descripcion = Producto.Descripcion;

                context.TablaProductos.Update(productoencontrado);
                await context.SaveChangesAsync();
                return Ok("Los datos han sido cambiados");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProducto(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception("No es correcto");
                }

                Producto? prod = await context.TablaProductos.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (prod == null)
                {
                    throw new Exception(("No existe el Producto con id igual a {id}.");
                }
                context.TablaProductos.Remove(prod);
                await context.SaveChangesAsync();
                return Ok($"El producto {prod} ha sido borrado.");
            }
            catch (Exception e) 
            {
                return BadRequest(e.Message);
            }
        }
    }
}
