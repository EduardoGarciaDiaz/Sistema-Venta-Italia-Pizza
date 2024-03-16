using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public static class EmpleadoDAO
    {
        public static List<TiposEmpleado> RecuperarTiposEmpleadoBD()
        {
            List<TiposEmpleado> resultadoOperacion = new List<TiposEmpleado>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    resultadoOperacion = context.TiposEmpleado.ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return resultadoOperacion;
        }

        public static int GuardarEmpleadoNuevoBD(Empleados empeladoNuevo)
        {
            int resultadoOperacion = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    context.Empleados.Add(empeladoNuevo);
                    resultadoOperacion = context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
            return resultadoOperacion;
        }

        public static bool NombreUsuarioEsUnico(String nombreUsuario)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.Empleados.Any(empleado => empleado.NombreUsuario.Equals(nombreUsuario));
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
