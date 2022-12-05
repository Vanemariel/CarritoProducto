using BDCarrito;
using BDCarrito.Entidades;
using Carrito.Shared.Modedls.DTOBack;
using Carrito.Shared.Models.DTOBack;
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



        [HttpPost("CreateCarrito")]
        public async Task<ActionResult<int>> CreateCarrito(int DNIUsuario)
        {
            try
            {
                BDCarrito.Entidades.Carrito nuevoCarrito = new BDCarrito.Entidades.Carrito
                {
                    PrecioTotal = 0,
                    DniUsuario = DNIUsuario,
                    ListaProductos = new List<Producto>()
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
        public async Task<ActionResult> DeleteCarrito(int idCarrito)
        {

            try
            {
                if (idCarrito <= 0)
                {
                    throw new Exception("No es correcto");
                }

                BDCarrito.Entidades.Carrito? Carro = await context.TablaCarrito
                    .Where(x => x.Id == idCarrito)
                    .FirstOrDefaultAsync();

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



        [HttpPost("AddProductoToCarrito")]
        public async Task<ActionResult<DatosCarritoDTO>> AddProductoToCarrito( int idCarrito, int idProducto)
        {
            try
            {

                BDCarrito.Entidades.Carrito? carrito = await context.TablaCarrito
                    .Where(carrito => carrito.Id == idCarrito)
                    .Include(Carrito => Carrito.ListaProductos)
                    .FirstOrDefaultAsync();

                if (carrito == null)
                {
                    throw new Exception("No existe el carrito");
                }

                Producto? producto = await context.TablaProductos
                    .Where(producto => producto.Id == idProducto)
                    .FirstOrDefaultAsync();

                if (producto == null)
                {
                    throw new Exception("No existe el producto");
                }

                carrito.ListaProductos.Add(producto);
                carrito.PrecioTotal = carrito.PrecioTotal + producto.Precio;
                //carrito.PrecioTotal += producto.Precio;

                context.TablaCarrito.Update(carrito);
                await context.SaveChangesAsync();

                List<ProductoDto> ListaItems = new List<ProductoDto>();

                foreach (var item in carrito.ListaProductos)
                {
                    ListaItems.Add(new ProductoDto
                    {
                        Nombre = item.Nombre,
                        Precio = item.Precio,
                        Descripcion = item.Descripcion,
                        Id = item.Id
                    });
                }

                DatosCarritoDTO DatosCarritoResponse = new DatosCarritoDTO
                {
                    IdCarrito = carrito.Id,
                    PrecioTotal = carrito.PrecioTotal,
                    ListaItems = ListaItems
                };

                return Ok(DatosCarritoResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("{idCarrito:int}/{idProducto:int}")]
        public async Task<ActionResult<DatosCarritoDTO>> DeleteProductoToCarrito(int idCarrito, int idProducto)
        {
            try
            {

                BDCarrito.Entidades.Carrito? carrito = await context.TablaCarrito
                    .Where(carrito => carrito.Id == idCarrito)
                    .Include(Carrito => Carrito.ListaProductos)
                    .FirstOrDefaultAsync();

                if (carrito == null)
                {
                    throw new Exception("No existe el carrito");
                }

                Producto? producto = await context.TablaProductos
                    .Where(producto => producto.Id == idProducto)
                    .FirstOrDefaultAsync();

                if (producto == null)
                {
                    throw new Exception("No existe el producto");
                }


                carrito.ListaProductos.Remove(producto);
                carrito.PrecioTotal = carrito.PrecioTotal - producto.Precio;
                //carrito.PrecioTotal -= producto.Precio;

                context.TablaCarrito.Update(carrito);
                await context.SaveChangesAsync();

                List<ProductoDto> ListaItems = new List<ProductoDto>();

                foreach (var item in carrito.ListaProductos)
                {
                    ListaItems.Add(new ProductoDto
                    {
                        Nombre = item.Nombre,
                        Precio = item.Precio,
                        Descripcion = item.Descripcion,
                        Id = item.Id
                    });
                }

                DatosCarritoDTO DatosCarritoResponse = new DatosCarritoDTO
                {
                    IdCarrito = carrito.Id,
                    PrecioTotal = carrito.PrecioTotal,
                    ListaItems = ListaItems
                };

                return Ok(DatosCarritoResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{idCarrito:int}")]
        public async Task<ActionResult<EstadoCarritoDTO>> GetStatusCarrito(int idCarrito, bool fechaEspecial)
        {
            try
            {
                BDCarrito.Entidades.Carrito? carrito = await context.TablaCarrito
                   .Where(carrito => carrito.Id == idCarrito)
                   .Include(Carrito => Carrito.ListaProductos)
                   .FirstOrDefaultAsync();

                if (carrito == null)
                {
                    throw new Exception("No existe el carrito");
                }

                Usuario? usuario = await context.TablaUsuario
                   .Where(usuario => usuario.DNI == carrito.DniUsuario)
                   .FirstOrDefaultAsync();


                if (carrito.ListaProductos.Count == 5)
                {
                    carrito.PrecioTotal = Convert.ToInt32(carrito.PrecioTotal * 0.80);
                }

                // Carrito fecha especial
                if (carrito.ListaProductos.Count > 10 & usuario.Vip == false && fechaEspecial == true)
                {
                    carrito.PrecioTotal = carrito.PrecioTotal - 2500;
                }

                // Carrito comun
                if (carrito.ListaProductos.Count > 10 & usuario.Vip == false && fechaEspecial == false)
                {
                    carrito.PrecioTotal = carrito.PrecioTotal - 1000;
                }

                // Carrito VIP
                if (carrito.ListaProductos.Count > 10 & usuario.Vip == true)
                {
                    carrito.ListaProductos.OrderBy(producto => producto.Precio).ToList();

                    carrito.PrecioTotal = carrito.PrecioTotal - carrito.ListaProductos[0].Precio - 3000;
                }

                EstadoCarritoDTO estadoCarritoDTO = new EstadoCarritoDTO {
                    TotalPagar = carrito.PrecioTotal
                };

                return Ok(estadoCarritoDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet("{dniCliente:int}")]
        public async Task<ActionResult<List<ProductoDto>>> ObtenerProductosMasCaros(int dniCliente)
        {
            try
            {
                Usuario? usuario = await context.TablaUsuario
                    .Where(usuario => usuario.DNI == dniCliente)
                    .Include(usuario => usuario.ListaCarritos)
                    .FirstOrDefaultAsync();

                List<Producto> listaProductos = new List<Producto>();

                foreach (BDCarrito.Entidades.Carrito carrito in usuario.ListaCarritos)
                {
                    foreach (Producto producto in carrito.ListaProductos)
                    {
                        listaProductos.Add(producto);
                    }
                }

                listaProductos.OrderByDescending(producto => producto.Precio).ToList();

                List<ProductoDto> listaProductosDTO = new List<ProductoDto>();
                for (int i = 0; i < 4; i++)
                {
                    listaProductosDTO.Add( new ProductoDto
                    {
                        Descripcion = listaProductos[i].Descripcion,
                        Id = listaProductos[i].Id,
                        Nombre = listaProductos[i].Nombre,
                        Precio = listaProductos[i].Precio
                    });
                }

                //List<ProductoDto> listaProductosDTO = new List<ProductoDto>
                //{
                //    new ProductoDto
                //    {
                //        Descripcion = listaProductos[0].Descripcion,
                //        Id =listaProductos[0].Id,
                //        Nombre = listaProductos[0].Nombre,
                //        Precio = listaProductos[0].Precio
                //    },
                //    new ProductoDto
                //    {
                //        Descripcion = listaProductos[1].Descripcion,
                //        Id =listaProductos[1].Id,
                //        Nombre = listaProductos[1].Nombre,
                //        Precio = listaProductos[1].Precio
                //    },
                //    new ProductoDto
                //    {
                //        Descripcion = listaProductos[2].Descripcion,
                //        Id =listaProductos[2].Id,
                //        Nombre = listaProductos[2].Nombre,
                //        Precio = listaProductos[2].Precio
                //    },
                //    new ProductoDto
                //    {
                //        Descripcion = listaProductos[3].Descripcion,
                //        Id =listaProductos[3].Id,
                //        Nombre = listaProductos[3].Nombre,
                //        Precio = listaProductos[3].Precio
                //    },
                //};

                return Ok(listaProductosDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
