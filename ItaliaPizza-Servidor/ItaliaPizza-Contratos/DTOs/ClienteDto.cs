using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class ClienteDto
    {
        [DataMember]
        public UsuarioDto Usuario { get; set; }

        [DataMember]
        public int IdDireccion { get; set; }

        [DataMember]
        public DireccionDto Direccion { get; set; }
    }
}
