using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class GestionProducto
    {
        public List<CategoriasInsumo> RecuperarCategoriasInsumo()
        {
            List<CategoriasInsumo> categoriasInsumo = new List<CategoriasInsumo>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    categoriasInsumo = context.CategoriasInsumo.ToList();                    
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

            return categoriasInsumo;
        }

        public List<CategoriasProductoVenta> RecuperarCategoriasProductoVenta()
        {
            List<CategoriasProductoVenta> categoriasProductoVenta = new List<CategoriasProductoVenta>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    categoriasProductoVenta = context.CategoriasProductoVenta.ToList();
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

            return categoriasProductoVenta;
        }
    }
}
