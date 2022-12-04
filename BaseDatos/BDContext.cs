using BDCarrito.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BDCarrito
{
    public class BDContext : DbContext
    {
        public DbSet<Carrito> TablaCarrito { get; set; }
        public DbSet<Producto> TablaProductos { get; set; }
        public DbSet<Usuario> TablaUsuario { get; set; }


        public BDContext(DbContextOptions options) : base(options)
        {
        }
    }
}