using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos
{
    [ServiceContract]
    public interface IServicioPedidos
    {
        [OperationContract]
        void OperacionPedidosEjemplo();
    }
}
