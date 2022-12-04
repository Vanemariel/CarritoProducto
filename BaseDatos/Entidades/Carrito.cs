using BDCarrito.Comun;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDCarrito.Entidades
{
    public class Carrito : BaseEntity
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public float PrecioTotal { get; set; }

        public int DniUsuario { get; set; }

        [InverseProperty("Carrito")]
        public List<Producto> Productos { get; set; }


        [ForeignKey("DniUsuario")]
        public Usuario Usuario { get; set; }    
    }
}

