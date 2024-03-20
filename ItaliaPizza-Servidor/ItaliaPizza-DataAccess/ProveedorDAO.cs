using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return proveedores;
        }

    }
}
