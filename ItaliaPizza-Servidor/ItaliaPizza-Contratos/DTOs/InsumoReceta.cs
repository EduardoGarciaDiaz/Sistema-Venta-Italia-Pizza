using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class InsumoReceta
    {
        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public double Cantidad { get; set; }

        [DataMember]
        public UnidadMedida UnidadMedida { get; set; }
    }
}
