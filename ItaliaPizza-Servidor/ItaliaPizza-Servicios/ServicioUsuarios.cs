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
                    clienteNuevo.IdDireccion = exito;
                    Usuarios usuariosNuevo = AuxiliarConversorDTOADAO.ConvertirUsuarioDtoAUsuarios(clienteNuevo);
                    exito = UsuarioDAO.GuardarUsuarioNuevoBD(usuariosNuevo);
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
                    empleadoNuevo.Usuario.IdDireccion = exito;
                    Usuarios usuariosNuevo = AuxiliarConversorDTOADAO.ConvertirUsuarioDtoAUsuarios(empleadoNuevo.Usuario);
                    exito = UsuarioDAO.GuardarUsuarioNuevoBD(usuariosNuevo);
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
            if (!String.IsNullOrEmpty(correo))
            {
                esUnico = UsuarioDAO.CorreoEsUnico(correo);
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
    }
}
