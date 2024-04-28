using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_Contratos.Excepciones;
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
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<ProveedorDto> RecuperarProveedores();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<ProveedorDto> RecuperarProveedoresActivos();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool GuardarProveedorNuevo(ProveedorDto proveedorNuevo);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ActualizarInformacionProveedor(ProveedorDto proveedor);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ValidarRfcUnicoProveedor(string rfc);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ValidarCorreoUnicoProveedor(string correo);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ValidarRfcUnicoProveedorEditado(string rfc, int idProveedor);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ValidarCorreoUnicoProveedorEditado(string correo, int idProveedor);


        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool CambiarEstadoProveedor(bool estaActivo, int idProveedor);
    }
}
