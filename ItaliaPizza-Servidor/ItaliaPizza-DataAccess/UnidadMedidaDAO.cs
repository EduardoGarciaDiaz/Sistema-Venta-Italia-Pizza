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
    public class UnidadMedidaDAO
    {
        public List<UnidadesMedida> RecuperarUnidadesMedida()
        {
            List<UnidadesMedida> unidadesMedida = new List<UnidadesMedida>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    unidadesMedida = context.UnidadesMedida.ToList();
                }

                return unidadesMedida;
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
