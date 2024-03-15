using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class ProductoVenta
    {
        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public float Precio { get; set; }

        [DataMember]
        public Byte[] Foto { get; set; }

        [DataMember]
        public Categoria Categoria { get; set; }
    }
}
