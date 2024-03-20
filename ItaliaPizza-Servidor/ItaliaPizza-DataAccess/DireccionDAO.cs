using ItaliaPizza_DataAccess.Excepciones;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public static class DireccionDAO
    {
        public static int GuardarDireccionNuevaBD(Direcciones direccionNueva)
        {
            int resultadoOperacion = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    context.Direcciones.Add(direccionNueva);
                    context.SaveChanges();
                    resultadoOperacion = direccionNueva.IdDireccion;
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

        public static List<Direcciones> RecuperarDireccionesBD(List<int?> idDirecciones)
        {
            List<Direcciones> direcciones = new List<Direcciones>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    direcciones = context.Direcciones.Where(direccion => idDirecciones.Contains(direccion.IdDireccion)).ToList();
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
            return direcciones;        
        }

    }

}
