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
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<ProductoSinReceta> RecuperarProductosSinReceta();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<InsumoRegistroReceta> RecuperarInsumos();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<Categoria> RecuperarCategoriasProductoVenta();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<ProductoVentaPedidos> RecuperarProductosVenta();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ValidarDisponibilidadDeProducto(string codigoProducto, int cantidadProductos);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<InsumoOrdenCompraDto> RecuperarInsumosActivos();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool DesapartarInsumosDeProducto(string codigoProducto, int cantidadParaDesapartar);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<Producto> RecuperarProductosTipoInsumo();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<Producto> RecuperarProductosTipoVenta();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<Categoria> RecuperarCategoriasInsumo();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ValidarDesactivacion(string codigoProducto);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int DesactivarProducto(string codigoProducto);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int ActivarProducto(string codigoProducto);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<Producto> RecuperarProductosInventariados();


        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int ActualizarProducto(Producto producto);


    }
}
