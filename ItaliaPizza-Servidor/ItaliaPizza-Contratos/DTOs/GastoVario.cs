using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class GastoVario
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public double Monto { get; set; }

        [DataMember]
        public string nombreUsuario { get; set; }
    }
}
