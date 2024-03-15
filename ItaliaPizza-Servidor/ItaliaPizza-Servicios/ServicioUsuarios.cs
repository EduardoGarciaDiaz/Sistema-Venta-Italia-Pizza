using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioUsuarios
    {
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
    }
}
