using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class RecetaProducto
    {
        [DataMember]
        public int IdReceta { get; set; }

        [DataMember]
        public string CodigoProducto { get; set; }

        [DataMember]
        public InsumoReceta[] InsumosReceta { get; set; }
    }
}
