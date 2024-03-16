using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class RecetaTemporalDAO
    {
        public RecetaTemporalDAO() { }

        public List<RecetasInsumos> RecuperarInsumosEnReceta(string codigoProducto)
        {
            List<RecetasInsumos> insumosDeReceta = new List<RecetasInsumos>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    insumosDeReceta = context.Recetas.FirstOrDefault(r => r.CodigoProducto == codigoProducto).RecetasInsumos.ToList();
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

            return insumosDeReceta;
        }
    }
}
