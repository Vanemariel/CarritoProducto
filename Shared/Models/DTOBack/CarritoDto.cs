using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrito.Shared.Modedls.DTOBack
{
    public class CarritoDto // sn uso aun
    {
        public float PrecioTotal { get; set; }

        public List<ProductoDto> Productos { get; set; }
    }
}
