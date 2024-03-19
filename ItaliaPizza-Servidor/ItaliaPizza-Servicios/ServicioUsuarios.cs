using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza_Servicios.Auxiliares;

namespace ItaliaPizza_Servicios
{
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioUsuarios
    {
        public bool GuardarCliente(UsuarioDto clienteNuevo)
        {
            bool seGuardoCliente = false;
            if(clienteNuevo != null)
            {
                int exito;
                Direcciones direccionNueva = AuxiliarConversorDTOADAO.ConvertirDireccionDtoADirecciones(clienteNuevo.Direccion);
                 exito= DireccionDAO.GuardarDireccionNuevaBD(direccionNueva);
                if(exito > 0)
                {
                    UsuarioDAO usuarioDAO = new UsuarioDAO();
                    clienteNuevo.IdDireccion = exito;
                    Usuarios usuariosNuevo = AuxiliarConversorDTOADAO.ConvertirUsuarioDtoAUsuarios(clienteNuevo);
                    exito = usuarioDAO.GuardarUsuarioNuevoBD(usuariosNuevo);
                    if(exito > 0)
                    {
                        seGuardoCliente = true;
                    }
                }
            }
            return seGuardoCliente;
        }

        public bool GuardarEmpleado(EmpleadoDto empleadoNuevo)
        {
            bool seGuardoEmpleado = false;
            if (empleadoNuevo != null)
            {
                int exito;
                Direcciones direccionNueva = AuxiliarConversorDTOADAO.ConvertirDireccionDtoADirecciones(empleadoNuevo.Usuario.Direccion);
                exito = DireccionDAO.GuardarDireccionNuevaBD(direccionNueva);
                if (exito > 0)
                {
                    UsuarioDAO usuarioDAO = new UsuarioDAO();
                    empleadoNuevo.Usuario.IdDireccion = exito;
                    Usuarios usuariosNuevo = AuxiliarConversorDTOADAO.ConvertirUsuarioDtoAUsuarios(empleadoNuevo.Usuario);
                    exito = usuarioDAO.GuardarUsuarioNuevoBD(usuariosNuevo);
                    if (exito > 0)
                    {
                        empleadoNuevo.IdUsuario = exito;
                        Empleados empleado = AuxiliarConversorDTOADAO.ConvertirEmpleadoDtoAEmpleado(empleadoNuevo);
                        exito = EmpleadoDAO.GuardarEmpleadoNuevoBD(empleado);
                        if (exito > 0)
                        {
                            seGuardoEmpleado = true;
                        }
                    }
                }
            }
            return seGuardoEmpleado;
        }

        public void OperacionUsuariosEjemplo()
        {
            throw new NotImplementedException();
        }

        public List<ClienteBusqueda> BuscarCliente(string nombre)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            return usuarioDAO.RecuperarClientesPorNombre(nombre).ConvertAll(usuario =>
                new ClienteBusqueda
                {
                    IdCliente = usuario.IdUsuario,
                    Nombre = usuario.NombreCompleto,
                    Correo = usuario.CorreoElectronico
                });
        }

                
        public List<TipoEmpleadoDto> RecuperarTiposEmpleado()
        {
            List<TipoEmpleadoDto> tiposEmpleadosLista = new List<TipoEmpleadoDto>();
            List<TiposEmpleado> tiposEmpleadoRecuperados = EmpleadoDAO.RecuperarTiposEmpleadoBD();
            foreach (var item in tiposEmpleadoRecuperados)
            {
                TipoEmpleadoDto tipoEmpleadoDto = AuxiliarConversorDTOADAO.ConvertirTiposEmpleadoATiposEmpleadoDto(item);
                tiposEmpleadosLista.Add(tipoEmpleadoDto);
            }
            return tiposEmpleadosLista;
        }

        public bool ValidarCorreoUnico(string correo)
        {
            bool esUnico = false;
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            if (!String.IsNullOrEmpty(correo))
            {
                esUnico = usuarioDAO.CorreoEsUnico(correo);
            }
            return esUnico;
        }

        public bool ValidarNombreDeUsuarioUnico(string nombreDeUsuario)
        {
            bool esUnico = false;
            if (!String.IsNullOrEmpty(nombreDeUsuario))
            {
                esUnico = EmpleadoDAO.NombreUsuarioEsUnico(nombreDeUsuario);
            }
            return esUnico;
        }

        public List<UsuarioDto> RecuperarClientes()
        {
            List<UsuarioDto> usuariosDto = new List<UsuarioDto>();
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            var usuariosLista = usuarioDAO.RecuperarClientesBD();
            foreach (var usuario in usuariosLista)
            {
                usuariosDto.Add(AuxiliarConversorDTOADAO.ConvertirUsuariosAUsuarioDto(usuario,usuario.Direcciones));
            }
            return usuariosDto;
        }



        public List<EmpleadoDto> RecuperarEmpleados()
        {
            List<EmpleadoDto> empleadosLista = new List<EmpleadoDto>();
            var empelados = EmpleadoDAO.RecuperarEmpleadoBD();
            foreach (var empleado in empelados)
            {
                empleadosLista.Add(AuxiliarConversorDTOADAO.ConvertirEmpleadosAEmpleadoDto(empleado, empleado.TiposEmpleado.Nombre, empleado.Usuarios, empleado.Usuarios.Direcciones));
            }
            return empleadosLista;
        }

        public bool Activar_DesactivarUsuario(int idUsuario, bool esEmpleado, bool esDesactivar)
        {
            bool exitoAccion;
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            if (esDesactivar)
            {
                if (esEmpleado)
                {
                    if (ListaEmpleadoActivos.EsEmpleadoNoActivo(idUsuario))
                    {
                        exitoAccion = usuarioDAO.DesactivarUsuario(idUsuario);
                    }
                    else
                    {
                        exitoAccion = false;
                    }
                }
                else
                {
                    if (usuarioDAO.ValidarClienteTienePedidosPendientes(idUsuario))
                    {
                        exitoAccion = usuarioDAO.DesactivarUsuario(idUsuario);
                    }
                    else
                    {
                        exitoAccion = false;
                    }
                }
            }
            else
            {
                exitoAccion = usuarioDAO.ActivarUsuario(idUsuario);
            }
            return exitoAccion;
        }
        
        public Cliente RecuperarClientePorId (int idCliente)
        {
            UsuarioDAO usuarioDAO=new UsuarioDAO();
            return usuarioDAO.RecuperarDatosClientePorId(idCliente);
        }

    }
}
