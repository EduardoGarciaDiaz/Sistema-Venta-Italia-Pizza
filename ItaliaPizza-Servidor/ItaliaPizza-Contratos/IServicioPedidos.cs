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
    public interface IServicioPedidos
    {
        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<TipoServicio> RecuperarTiposServicio();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int GuardarPedido(Pedido pedido);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<PedidoConsultaDTO> RecuperarPedidosEnProceso();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<PedidoConsultaDTO> RecuperarPedidosPreparados();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<PedidoConsultaDTO> RecuperarPedidos();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        Pedido RecuperarPedido(int numeroPedido);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int ActualizarEstadoPedido(int numeroPedido, int idEstadoPedido);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        double RecuperarIngresosDePedidosPorFecha(DateTime fecha);
    }
}
