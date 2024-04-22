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
    public interface IServicioCorteCaja
    {
        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        CorteCaja RecuperarCorteCaja(DateTime fecha);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int GuardarCorteCaja(CorteCaja corteCaja);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int ActualizarCorteCaja(CorteCaja corteCaja);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ExisteCorteCaja(DateTime fecha);
    }
}
