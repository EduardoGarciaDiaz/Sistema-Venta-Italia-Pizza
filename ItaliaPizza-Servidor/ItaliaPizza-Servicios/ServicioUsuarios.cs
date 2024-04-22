using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza_Servicios.Auxiliares;
using ItaliaPizza_DataAccess.Excepciones;
using ItaliaPizza_Contratos.Excepciones;
using System.ServiceModel;

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
                try
                {
                    exito = DireccionDAO.GuardarDireccionNuevaBD(direccionNueva);
                    if (exito > 0)
                    {
                        UsuarioDAO usuarioDAO = new UsuarioDAO();
                        clienteNuevo.IdDireccion = exito;
                        Usuarios usuariosNuevo = AuxiliarConversorDTOADAO.ConvertirUsuarioDtoAUsuarios(clienteNuevo);
                        exito = usuarioDAO.GuardarUsuarioNuevoBD(usuariosNuevo);
                        if (exito > 0)
                        {
                            seGuardoCliente = true;
                        }
                    }
                }
                catch (ExcepcionDataAccess e)
                {
                    throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
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
                try
                {
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
                catch (ExcepcionDataAccess e)
                {
                    throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
                }
            }
            return seGuardoEmpleado;
        }

        public List<ClienteBusqueda> BuscarCliente(string nombre)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            try
            {
                return usuarioDAO.RecuperarClientesPorNombre(nombre).ConvertAll(usuario =>
                    new ClienteBusqueda
                    {
                        IdCliente = usuario.IdUsuario,
                        Nombre = usuario.NombreCompleto,
                        Correo = usuario.CorreoElectronico
                    });
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

                
        public List<TipoEmpleadoDto> RecuperarTiposEmpleado()
        {
            List<TipoEmpleadoDto> tiposEmpleadosLista = new List<TipoEmpleadoDto>();
            try
            {
                List<TiposEmpleado> tiposEmpleadoRecuperados = EmpleadoDAO.RecuperarTiposEmpleadoBD();
                foreach (var item in tiposEmpleadoRecuperados)
                {
                    TipoEmpleadoDto tipoEmpleadoDto = AuxiliarConversorDTOADAO.ConvertirTiposEmpleadoATiposEmpleadoDto(item);
                    tiposEmpleadosLista.Add(tipoEmpleadoDto);
                }
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return tiposEmpleadosLista;
        }

        public bool ValidarCorreoUnico(string correo)
        {
            try
            {
                bool esUnico = false;
                UsuarioDAO usuarioDAO = new UsuarioDAO();
                if (!string.IsNullOrEmpty(correo))
                {
                    try
                    {
                        esUnico = usuarioDAO.CorreoEsUnico(correo);
                    }
                    catch (ExcepcionDataAccess e)
                    {
                        throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
                    }
                }
                return esUnico;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public bool ValidarNombreDeUsuarioUnico(string nombreDeUsuario)
        {
            bool esUnico = false;
            if (!string.IsNullOrEmpty(nombreDeUsuario))
            {
                try
                {
                    esUnico = EmpleadoDAO.NombreUsuarioEsUnico(nombreDeUsuario);
                }
                catch (ExcepcionDataAccess e)
                {
                    throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
                }
            }
            return esUnico;
        }

        public List<UsuarioDto> RecuperarClientes()
        {
            List<UsuarioDto> usuariosDto = new List<UsuarioDto>();
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            try
            {
                var usuariosLista = usuarioDAO.RecuperarClientesBD();
                foreach (var usuario in usuariosLista)
                {
                    usuariosDto.Add(AuxiliarConversorDTOADAO.ConvertirUsuariosAUsuarioDto(usuario, usuario.Direcciones));
                }
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return usuariosDto;
        }



        public List<EmpleadoDto> RecuperarEmpleados()
        {
            List<EmpleadoDto> empleadosLista = new List<EmpleadoDto>();
            try
            {
                var empleados = EmpleadoDAO.RecuperarEmpleadosBD();
                var meseros = EmpleadoDAO.RecuperarMeserosBD();
                foreach (var empleado in empleados)
                {
                    empleadosLista.Add(AuxiliarConversorDTOADAO.ConvertirEmpleadosAEmpleadoDto(empleado, empleado.TiposEmpleado.Nombre, empleado.Usuarios, empleado.Usuarios.Direcciones));
                }
                foreach (var mesero in meseros)
                {
                    empleadosLista.Add(AuxiliarConversorDTOADAO.ConvertirMeserosAEmpleadoDto(mesero));
                }
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return empleadosLista;
        }

        public bool Activar_DesactivarUsuario(int idUsuario, bool esEmpleado, bool esDesactivar)
        {
            bool exitoAccion;
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            PedidoDAO pedidoDAO = new PedidoDAO();
            try
            {
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
                        if (pedidoDAO.ValidarClienteTienePedidosPendientes(idUsuario))
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
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            
            return exitoAccion;
        }
        
        public Cliente RecuperarClientePorId (int idCliente)
        {
            UsuarioDAO usuarioDAO=new UsuarioDAO();
            try
            {
                return usuarioDAO.RecuperarDatosClientePorId(idCliente);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public EmpleadoDto RecuperarEmpleadoPorNombreUsuario(string nombreUsuario)
        {
            EmpleadoDto empleadoDto;
            try
            {
                var empleado = EmpleadoDAO.RecuperarEmpleadoProNombreUsuarioBD(nombreUsuario);               
                if (!nombreUsuario.Equals("mesero"))
                {
                    empleadoDto = RecuperarEmpleadoComun(nombreUsuario);
                }
                else
                {
                    empleadoDto = RecuperarEmpeladoMesero();
                }
                ListaEmpleadoActivos.RegistrarUsuarioEnLista(empleadoDto.IdUsuario, empleado.NombreUsuario);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return empleadoDto;
        }

        private EmpleadoDto RecuperarEmpleadoComun(string nombreUsuario)
        {
            EmpleadoDto empleadoDto;            
            var empleado = EmpleadoDAO.RecuperarEmpleadoProNombreUsuarioBD(nombreUsuario);
            string tipo;
            if (empleado.TiposEmpleado != null)
            {
                tipo = empleado.TiposEmpleado.Nombre ?? string.Empty;
            }
            else
            {
                empleado.IdTipoEmpleado = 0;
                tipo = "Administrador";
            }
            empleadoDto = AuxiliarConversorDTOADAO.ConvertirEmpleadosAEmpleadoDto(empleado, tipo, empleado.Usuarios, empleado.Usuarios.Direcciones);          
            return empleadoDto;
        }

        private EmpleadoDto RecuperarEmpeladoMesero()
        {
            EmpleadoDto empleadoDto;            
            var empleado = EmpleadoDAO.RecuperarMeseroPorNombreUsuarioBD();
            string tipo;
            if (empleado.Empleados.TiposEmpleado != null)
            {
                tipo = empleado.Empleados.TiposEmpleado.Nombre ?? string.Empty;
            }
            else
            {
                empleado.Empleados.IdTipoEmpleado = 0;
                tipo = "Administrador";
            }
            empleadoDto = AuxiliarConversorDTOADAO.ConvertirMeserosAEmpleadoDto(empleado);              
            return empleadoDto;
        }

        public bool ValidarActualizacionNombreDeUsuarioUnico(string nuevoNombreUsuario, int idUsuarioModificar)
        {
            bool esActualizacionValida = false;

            try
            {
                esActualizacionValida = EmpleadoDAO.ValidarActualizacionNombreDeUsuarioUnico(nuevoNombreUsuario, idUsuarioModificar);

                return esActualizacionValida;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public bool ValidarActualizacionCorreoUnico(string nuevoCorreo, int idUsuarioModificar)
        {
            bool esActualizacionValida = false;
            UsuarioDAO usuarioDAO = new UsuarioDAO();

            try
            {
                esActualizacionValida = usuarioDAO.ValidarActualizacionCorreoUnico(nuevoCorreo, idUsuarioModificar);

                return esActualizacionValida;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public bool ActualizarEmpleado(EmpleadoDto empleadoEdicion)
        {
            bool seGuardoEmpleado = false;

            if (empleadoEdicion != null)
            {
                int exito;
                Direcciones direccionEdicion = AuxiliarConversorDTOADAO.ConvertirDireccionDtoADirecciones(empleadoEdicion.Usuario.Direccion);
                try
                {
                    exito = DireccionDAO.ActualizarDireccion(direccionEdicion);
                    if (exito > 0)
                    {
                        UsuarioDAO usuarioDAO = new UsuarioDAO();
                        Usuarios usuariosNuevo = AuxiliarConversorDTOADAO.ConvertirUsuarioDtoAUsuarios(empleadoEdicion.Usuario);
                        exito = usuarioDAO.ActualizarUsuario(usuariosNuevo);
                        if (exito > 0)
                        {
                            Empleados empleado = AuxiliarConversorDTOADAO.ConvertirEmpleadoDtoAEmpleado(empleadoEdicion);
                            exito = EmpleadoDAO.ActualizarEmpleado(empleado);
                            if (exito > 0)
                            {
                                seGuardoEmpleado = true;
                            }
                        }
                    }
                }
                catch (ExcepcionDataAccess e)
                {
                    throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
                }
            }

            return seGuardoEmpleado;

        }

        public bool ActualizarCliente(UsuarioDto usuarioEdicion)
        {
            bool seGuardoCliente = false;
            if (usuarioEdicion != null)
            {
                int exito;
                Direcciones direccionEdicion = AuxiliarConversorDTOADAO.ConvertirDireccionDtoADirecciones(usuarioEdicion.Direccion);
                try
                {
                    exito = DireccionDAO.ActualizarDireccion(direccionEdicion);

                    if (exito > 0)
                    {
                        UsuarioDAO usuarioDAO = new UsuarioDAO();
                        Usuarios usuariosEdicion = AuxiliarConversorDTOADAO.ConvertirUsuarioDtoAUsuarios(usuarioEdicion);
                        exito = usuarioDAO.ActualizarUsuario(usuariosEdicion);
                        if (exito > 0)
                        {
                            seGuardoCliente = true;
                        }
                    }
                }
                catch (ExcepcionDataAccess e)
                {
                    throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
                }
            }
            return seGuardoCliente;
        }
    }
}
