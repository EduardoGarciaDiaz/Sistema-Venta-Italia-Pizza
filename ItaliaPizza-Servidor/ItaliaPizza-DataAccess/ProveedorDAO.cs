using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza_DataAccess.Excepciones;

namespace ItaliaPizza_DataAccess
{
    public static class ProveedorDAO
    {

        public static List<Proveedores> RecuperarProveedoresBD()
        {
            List<Proveedores> proveedores = new List<Proveedores>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    proveedores = context.Proveedores.Include(ins => ins.Direcciones).ToList();

                    return proveedores.ToList();
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

        public static bool GuardarProveedorNuevoBD(Proveedores proveedorNuevo)
        {
            bool exitoOperacion = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.Proveedores.Add(proveedorNuevo);
                    int filasAfectadas = context.SaveChanges();
                    if(filasAfectadas > 0)
                    {
                        exitoOperacion = true;
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
            return exitoOperacion;
        }

        public static  bool ValidarCorreoUnicoProveedorBD(string correo)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.Proveedores.Any(proveedor => proveedor.CorreoElectronico.Equals(correo));
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

        public static bool ValidarRfcUnicoProveedorBD(string rfc)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.Proveedores.Any(proveedor => proveedor.RFC.Equals(rfc));
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
