using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrito.Shared.Modedls.DTOBack
{
    public class ProductoDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public float Precio { get; set; }

        public string Descripcion { get; set; }
    }
}
