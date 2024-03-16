using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class ProductoVentaPedidos
    {

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public byte[] Foto { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public double Precio { get; set; }

        [DataMember]
        public int IdCategoria { get; set; }
    }
}
