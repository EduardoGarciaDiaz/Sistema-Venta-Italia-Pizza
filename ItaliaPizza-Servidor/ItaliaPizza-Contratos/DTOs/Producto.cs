using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class Producto
    {
        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public bool EsInventariado { get; set; }

        [DataMember]
        public Insumo Insumo { get; set; }

        [DataMember]
        public ProductoVenta ProductoVenta { get; set; }

        [DataMember]
        public bool EsActivo { get; set; }
    }
}
