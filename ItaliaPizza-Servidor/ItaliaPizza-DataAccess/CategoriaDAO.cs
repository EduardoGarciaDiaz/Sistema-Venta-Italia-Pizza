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
    public class CategoriaDAO
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

                return categoriasInsumo;
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

        public List<CategoriasProductoVenta> RecuperarCategoriasProductoVenta()
        {
            List<CategoriasProductoVenta> categoriasProductoVenta = new List<CategoriasProductoVenta>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    categoriasProductoVenta = context.CategoriasProductoVenta.ToList();
                }

                return categoriasProductoVenta;
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
