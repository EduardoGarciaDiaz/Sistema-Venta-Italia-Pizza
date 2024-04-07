using ItaliaPizza_Contratos.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos
{
    [ServiceContract]
    public interface IServicioProveedores
    {
        [OperationContract]
        List<ProveedorDto> RecuperarProveedores();

        [OperationContract]
        bool GuardarProveedorNuevo(ProveedorDto proveedorNuevo);

        [OperationContract]
        bool ActualizarInformacionProveedor(ProveedorDto proveedor);

        [OperationContract]
        bool ValidarRfcUnicoProveedor(string rfc);

        [OperationContract]
        bool ValidarCorreoUnicoProveedor(string correo);

        [OperationContract]
        bool ValidarRfcUnicoProveedorEditado(string rfc, int idProveedor);

        [OperationContract]
        bool ValidarCorreoUnicoProveedorEditado(string correo, int idProveedor);
    }
}
