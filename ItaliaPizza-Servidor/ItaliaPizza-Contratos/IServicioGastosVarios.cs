using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_Contratos.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos
{
    [ServiceContract]
    public interface IServicioGastosVarios
    {
        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int GuardarGastoVario(GastoVario gastoVario);


    }
}
