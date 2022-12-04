using Carrito.Shared.Modedls.DTOBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrito.Shared.Models.DTOBack
{
    public class DatosCarritoDTO
    {
        public int IdCarrito { get; set; }

        public List<ProductoDto> ListaItems { get; set; }

        public float PrecioTotal { get; set; }

    }
}
