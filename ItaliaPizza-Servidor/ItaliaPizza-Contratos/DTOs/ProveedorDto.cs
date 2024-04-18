using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class ProveedorDto
    {
        [DataMember]
        public int IdProveedor { get; set; }

        [DataMember]
        public string NombreCompleto { get; set; }

        [DataMember]
        public string RFC { get; set; }

        [DataMember]
        public string CorreoElectronico { get; set; }

        [DataMember]
        public string NumeroTelefono { get; set; }

        [DataMember]
        public int IdDireccion { get; set; }

        [DataMember]
        public bool EsActivo { get; set; }

        [DataMember]
        public DireccionDto Direccion { get; set; }

    }
}
