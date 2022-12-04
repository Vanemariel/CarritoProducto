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
    public class Usuario : BaseEntity
    {
        [Key]
        public int DNI { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        public bool Vip { get; set; }


        [InverseProperty("Usuario")]
        public List<Carrito> Carritos { get; set; }
    }
}
