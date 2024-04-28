using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class Reporte
    {
        [DataMember]
        public byte[] ContenidoReporte { get; set; }
    }
}
