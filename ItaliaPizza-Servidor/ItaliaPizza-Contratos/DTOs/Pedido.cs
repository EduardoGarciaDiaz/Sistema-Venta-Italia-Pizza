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
        public int IdTipoServicio { get; set; }

        [DataMember]
        public int IdCliente { get; set; }

        [DataMember]
        public string nombreUsuarioCajero { get; set; }

        [DataMember]
        public Dictionary<string, int> productosIncluidos { get; set; }

    }
}
