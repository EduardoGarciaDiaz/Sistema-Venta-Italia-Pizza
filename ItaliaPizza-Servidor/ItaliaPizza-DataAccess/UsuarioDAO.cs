using ItaliaPizza_Contratos.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public static class UsuarioDAO
    {
        public static int GuardarUsuarioNuevoBD(Usuarios usuarioNuevo)
        {
            int resultadoOperacion = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    context.Usuarios.Add(usuarioNuevo);
                    context.SaveChanges();
                    resultadoOperacion = usuarioNuevo.IdUsuario;
                }
            }
            catch (Exception ex)
            {

            }
            return resultadoOperacion;
        }

        public static bool CorreoEsUnico(String correo)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.Usuarios.Any(usuario => usuario.CorreoElectronico.Equals(correo));
                }
            }
            catch (Exception ex)
            {
                resultadoOperacion = false;
            }
            return resultadoOperacion;
        }



    }
}
