using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class UsuarioDAO
    {

        public UsuarioDAO() { }

        public List<Usuarios> RecuperarClientesPorNombre(string nombre)
        {
            List<Usuarios> clientes = new List<Usuarios>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    clientes = context.Usuarios.Where(usuario =>
                    usuario.NombreCompleto.Contains(nombre)
                    && usuario.EsActivo == true
                    && context.Empleados.FirstOrDefault(empleado =>
                        empleado.IdUsuario == usuario.IdUsuario) == null)
                        .ToList();
                }
            }
            catch (EntityException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (SqlException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }

            return clientes;
        }
    }
}
