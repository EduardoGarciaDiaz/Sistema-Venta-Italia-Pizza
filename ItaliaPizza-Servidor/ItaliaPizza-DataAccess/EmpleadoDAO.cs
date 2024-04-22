using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza_DataAccess.Excepciones;
using System.Data.Entity.Migrations;
using System.Runtime.Remoting.Contexts;

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
                    if (empeladoNuevo.IdTipoEmpleado == context.TiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals("Mesero")).IdTipoEmpleado)
                    {
                        int idUsuario = (int)empeladoNuevo.IdUsuario;
                        if (!context.Empleados.Any(emp => emp.IdTipoEmpleado == context.TiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals("Mesero")).IdTipoEmpleado))
                        {
                            empeladoNuevo.IdUsuario = null;
                            context.Empleados.AddOrUpdate(empeladoNuevo);
                        }
                        context.Meseros.Add(new Meseros() {IdMesero = 0, IdUsuario = idUsuario, NombreUsuario = empeladoNuevo.NombreUsuario});
                    }
                    else
                    {
                        context.Empleados.Add(empeladoNuevo);
                    }
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
                     empleados = context.Empleados.Include(u => u.Usuarios.Direcciones).Include(e=> e.Usuarios).Include(e => e.TiposEmpleado).Where(empl => empl.IdTipoEmpleado != null && empl.IdUsuario != null).ToList();
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

        public static List<Meseros> RecuperarMeserosBD()
        {
            List<Meseros> meseros = new List<Meseros>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    meseros = context.Meseros.Include(u => u.Usuarios.Direcciones).Include(e => e.Usuarios).Include(e => e.Empleados).Include(e => e.Empleados.TiposEmpleado).ToList();
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
            return meseros;
        }

        public static Empleados RecuperarEmpleadoProNombreUsuarioBD(string nombreUsuario)
        {

            Empleados empleados = new Empleados();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    if (nombreUsuario.Equals("mesero"))
                    {
                       var  mesero = context.Meseros.Include(u => u.Usuarios.Direcciones).Include(e => e.Usuarios).Include(e => e.Empleados).Include(e => e.Empleados.TiposEmpleado).First();
                        
                    }
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

        public static Meseros RecuperarMeseroPorNombreUsuarioBD()
        {
            Meseros meseros = new Meseros();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    meseros = context.Meseros.Include(u => u.Usuarios.Direcciones).Include(e => e.Usuarios).Include(e => e.Empleados).Include(e => e.Empleados.TiposEmpleado).FirstOrDefault();
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
            return meseros;
        }

        public static int ValidarCredencialesBD(string nombreUsuario, string contraseña)
        {
            int resultadoOperacion = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    bool existe = context.Empleados.Any(emp => emp.NombreUsuario.Equals(nombreUsuario) && emp.Contraseña.Equals(contraseña));  
                    if (existe)
                    {
                        if (!nombreUsuario.Equals("mesero")) {
                            if ((bool)context.Empleados.FirstOrDefault(emp => emp.NombreUsuario.Equals(nombreUsuario)).Usuarios.EsActivo)
                            {
                                resultadoOperacion = 1;
                            }
                            else
                            {
                                resultadoOperacion = 2;
                            }
                        }
                        else
                        {
                            resultadoOperacion = 1;
                        }
                    }
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

        public static bool ValidarActualizacionNombreDeUsuarioUnico(string nuevoNombreUsuario, int idUsuarioModificar)
        {
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    return !context.Empleados.Any(empleado => empleado.NombreUsuario.Equals(nuevoNombreUsuario) && empleado.IdUsuario != idUsuarioModificar);
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
        }

        public static int ActualizarEmpleado(Empleados empleadoEdicion )
        {
            int filasAfectadas = -1;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var empleado = context.Empleados.Find(empleadoEdicion.IdUsuario);
                    if (empleado != null)
                    {
                        context.Entry(empleado).CurrentValues.SetValues(empleadoEdicion);
                        filasAfectadas = context.SaveChanges();
                    }

                    return filasAfectadas;
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
        }
    }
}
