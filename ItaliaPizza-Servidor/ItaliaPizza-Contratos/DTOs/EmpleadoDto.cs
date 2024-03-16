using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class EmpleadoDto
    {
        [DataMember]
        public int IdUsuario { get; set; }

        [DataMember]
        public UsuarioDto Usuario { get;  set; }
        
        [DataMember]
        public string NombreUsuario { get; set; }

        [DataMember]
        public string Contraseña { get; set; }

        [DataMember]
        public int IdTipoEmpleado { get; set; }

        [DataMember]
        public String TipoEmpleado { get; set; }
    }
}
