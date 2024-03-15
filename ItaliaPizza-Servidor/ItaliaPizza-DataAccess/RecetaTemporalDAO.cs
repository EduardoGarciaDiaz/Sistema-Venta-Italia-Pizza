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

        public Recetas RecuperarRecetaDeProducto(string codigoProducto)
        {
            Recetas receta = new Recetas();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    receta = context.Recetas.Where(recetaConsulta =>
                        receta.CodigoProducto == codigoProducto).FirstOrDefault();
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

            return receta;
        }
    }
}
