using BDCarrito.Comun;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDCarrito.Entidades
{
    public class Producto : BaseEntity
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public float Precio { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Descripcion { get; set; }   

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int CarritoId { get; set; }

        [ForeignKey("CarritoId")]
        public Carrito Carrito { get; set; } 
    }
}
 