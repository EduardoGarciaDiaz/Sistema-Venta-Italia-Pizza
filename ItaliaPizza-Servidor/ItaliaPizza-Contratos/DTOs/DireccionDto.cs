using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class DireccionDto
    {
        [DataMember]
        public int IdDireccion { get; set; }

        [DataMember]
        public string CodigoPostal { get; set; }

        [DataMember]
        public string Calle { get; set; }

        [DataMember]
        public int Numero { get; set; }

        [DataMember]
        public string Ciudad { get; set; }

        [DataMember]
        public string Colonia { get; set; }

    }
}
