using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{

    [DataContract]
    public class CorteCaja
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public double dineroEnCaja { get; set; }

        [DataMember]
        public double fondo {  get; set; }

        [DataMember]
        public double salidasRegistradas { get; set; }

        [DataMember]
        public double ingresosRegistrados { get; set; }

        [DataMember]
        public double diferencia { get; set; }

        [DataMember]
        public DateTime fecha { get; set; }

        [DataMember]
        public string nombreUsuario { get; set; }   

    }
}
