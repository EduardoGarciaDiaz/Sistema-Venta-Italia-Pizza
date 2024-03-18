using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class Insumo
    {
        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public float Cantidad { get; set; }

        [DataMember]
        public UnidadMedida UnidadMedida { get; set; }

        [DataMember]
        public float CostoUnitario { get; set; }

        [DataMember]
        public string Restriccion { get; set; }

        [DataMember]
        public Categoria Categoria { get; set; }
    }
}
