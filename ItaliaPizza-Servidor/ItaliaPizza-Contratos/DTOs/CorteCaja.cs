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
        public double DineroEnCaja { get; set; }

        [DataMember]
        public double Fondo {  get; set; }

        [DataMember]
        public double SalidasRegistradas { get; set; }

        [DataMember]
        public double IngresosRegistrados { get; set; }

        [DataMember]
        public double Diferencia { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string NombreUsuario { get; set; }   

    }
}
