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
        Categoria RecuperarCategorias();

        [OperationContract]
        bool ValidarCodigoProducto(string codigoProducto);

        [OperationContract]
        int GuardarProducto(Producto producto);
    }
}
