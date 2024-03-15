using ItaliaPizza_Contratos.DTOs;
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
        List<Categoria> RecuperarCategorias();

        [OperationContract]
        List<UnidadMedida> RecuperarUnidadesMedida();

        [OperationContract]
        bool ValidarCodigoProducto(string codigoProducto);

        [OperationContract]
        int GuardarProducto(Producto producto);

        [OperationContract]
        List<Categoria> RecuperarCategoriasProductoVenta();

        [OperationContract]
        List<ProductoVentaPedidos> RecuperarProductosVenta();

        [OperationContract]
        bool ValidarDisponibilidadDeProducto(string codigoProducto);
    }
}
