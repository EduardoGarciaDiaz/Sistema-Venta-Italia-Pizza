﻿using ItaliaPizza_Contratos.DTOs;
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
        void OperacionUsuariosEjemplo();

        [OperationContract]
        List<ClienteBusqueda> BuscarCliente(string nombre);

        [OperationContract]
        bool GuardarEmpleado(EmpleadoDto empleadoNuevo);

        [OperationContract]
        bool GuardarCliente(UsuarioDto clienteNuevo);

        [OperationContract]
        List<TipoEmpleadoDto> RecuperarTiposEmpleado();

        [OperationContract]
        bool ValidarNombreDeUsuarioUnico(String nombreDeUsuario);

        [OperationContract]
        bool ValidarCorreoUnico(String correo);

        [OperationContract]
        List<UsuarioDto> RecuperarClientes();

        [OperationContract]
        List<EmpleadoDto> RecuperarEmpleados();

        [OperationContract]
        bool Activar_DesactivarUsuario(int idUsuario, bool esEmpleado, bool esDesactivar);

        Cliente RecuperarClientePorId(int idCliente);

    }

    

     
}
