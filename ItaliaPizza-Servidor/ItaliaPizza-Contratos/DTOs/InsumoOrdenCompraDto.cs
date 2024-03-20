using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public  class InsumoOrdenCompraDto
    {
        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Codigo { get; set; }


        [DataMember]
        public String UnidadMedida { get; set; }

        [DataMember]
        public float Existencia { get; set; }

        [DataMember]
        public float CostoUnitario { get; set; }

    }
}
