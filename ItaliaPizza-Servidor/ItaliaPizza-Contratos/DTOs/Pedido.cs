using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class Pedido
    {
        [DataMember]
        public int NumeroPedido { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public int CantidadProductos { get; set; }

        [DataMember]
        public double Total { get; set; }

        [DataMember]
        public int IdEstadoPedido { get; set; }

        [DataMember]
        public TipoServicio TipoServicio { get; set; }

        [DataMember]
        public int IdCliente { get; set; }

        [DataMember]
        public string NombreUsuarioCajero { get; set; }

        [DataMember]
        public Dictionary<ProductoVentaPedidos, int> ProductosIncluidos { get; set; }

    }
}
