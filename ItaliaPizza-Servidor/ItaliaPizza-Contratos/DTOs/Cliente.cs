using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class Cliente
    {
        [DataMember]
        public int IdCliente { get; set; }

        [DataMember]
        public string NombreCliente { get; set; }

        [DataMember]
        public string NumeroTelefonoCliente { get; set; }

        [DataMember]
        public string CorreoElectronicoCliente { get; set; }

        [DataMember]
        public string DireccionCliente { get; set; }
    }
}
