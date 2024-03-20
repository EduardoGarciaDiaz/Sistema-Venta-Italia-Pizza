using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza_DataAccess.Excepciones;

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
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
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
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
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
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return resultadoOperacion;
        }


        public static  List<Empleados> RecuperarEmpleadosBD()
        {

            List<Empleados> empleados = new List<Empleados>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                     empleados = context.Empleados.Include(u => u.Usuarios.Direcciones).Include(e=> e.Usuarios).Include(e => e.TiposEmpleado).ToList();

                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return empleados;
        }

        public static Empleados RecuperarEmpleadoProNombreUsuarioBD(string nombreUsuario)
        {

            Empleados empleados = new Empleados();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    empleados = context.Empleados.Include(e => e.Usuarios).Include(u => u.Usuarios.Direcciones).Include(e => e.TiposEmpleado).FirstOrDefault(empl => empl.NombreUsuario.Equals(nombreUsuario));
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return empleados;
        }

        public static bool ValidarCredencialesBD(string nombreUsuario, string contraseña)
        {
            bool resultadoOperacion = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    resultadoOperacion = context.Empleados.Any(emp => emp.NombreUsuario.Equals(nombreUsuario) && emp.Contraseña.Equals(contraseña));                   
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return resultadoOperacion;
        }
    }
}
