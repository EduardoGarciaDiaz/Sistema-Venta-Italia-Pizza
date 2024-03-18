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
    public interface IServicioProductos
    {
        [OperationContract]
        void OperacionProductosEjemplo();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<Categoria> RecuperarCategorias();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<UnidadMedida> RecuperarUnidadesMedida();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ValidarCodigoProducto(string codigoProducto);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int GuardarProducto(Producto producto);

        [OperationContract]
        List<ProductoSinReceta> RecuperarProductosSinReceta();

        [OperationContract]
        List<InsumoRegistroReceta> RecuperarInsumos();

        [OperationContract]
        List<Categoria> RecuperarCategoriasProductoVenta();

        [OperationContract]
        List<ProductoVentaPedidos> RecuperarProductosVenta();

        [OperationContract]
        bool ValidarDisponibilidadDeProducto(string codigoProducto, int cantidadProductos);
    }
}
