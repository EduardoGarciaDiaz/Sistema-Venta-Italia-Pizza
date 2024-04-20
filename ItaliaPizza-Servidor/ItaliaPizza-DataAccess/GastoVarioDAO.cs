using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess.Excepciones;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class GastoVarioDAO
    {
        public int GuardarGastoVario(GastosVarios nuevoGastoVario)
        {
            int filasAfectadas = -1;

            try
            {
                if (nuevoGastoVario != null)
                {
                    using (var context = new ItaliaPizzaEntities())
                    {
                        context.GastosVarios.Add(nuevoGastoVario);
                        filasAfectadas = context.SaveChanges();
                    }
                }

                return filasAfectadas;
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

        public double RecuperarSalidasGastosVarios(DateTime fecha)
        {
            double salidasGastosVarios = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    List<GastosVarios> gastosVariosFechaSeleccionada = context.GastosVarios.Where(g => DbFunctions.TruncateTime(g.Fecha) == fecha.Date).ToList();
                    salidasGastosVarios = gastosVariosFechaSeleccionada.Sum(g => g.Total ?? 0);
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

            return salidasGastosVarios;
        }
    }
}
