using ItaliaPizza_DataAccess.Excepciones;
using System;
using System.Collections.Generic;
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
    }
}
