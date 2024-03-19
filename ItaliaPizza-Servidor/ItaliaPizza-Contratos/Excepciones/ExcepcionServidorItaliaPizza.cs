using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.Excepciones
{
    [DataContract]
    public class ExcepcionServidorItaliaPizza
    {
        [DataMember]
        public string Mensaje { get; set; }

        [DataMember]
        public string StackTrace { get; set; }
    }
}
