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
    public interface IServicioUsuarios
    {
        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<ClienteBusqueda> BuscarCliente(string nombre);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool GuardarEmpleado(EmpleadoDto empleadoNuevo);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool GuardarCliente(UsuarioDto clienteNuevo);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<TipoEmpleadoDto> RecuperarTiposEmpleado();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ValidarNombreDeUsuarioUnico(String nombreDeUsuario);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool ValidarCorreoUnico(String correo);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<UsuarioDto> RecuperarClientes();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<EmpleadoDto> RecuperarEmpleados();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool Activar_DesactivarUsuario(int idUsuario, bool esEmpleado, bool esDesactivar);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        Cliente RecuperarClientePorId(int idCliente);

    }

    

     
}
