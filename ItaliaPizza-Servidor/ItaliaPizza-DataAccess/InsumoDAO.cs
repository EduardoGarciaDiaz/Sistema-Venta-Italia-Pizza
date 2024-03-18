using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class InsumoDAO
    {
        public InsumoDAO() { }

        public bool ValidarDisponibilidadInsumo(string codigoInsumo, int cantidadRequerida)
        {
            bool insumoDisponible = false;

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    Insumos insumo = context.Insumos.FirstOrDefault(i => i.CodigoProducto == codigoInsumo);
                    if (insumo != default)
                    {
                        insumoDisponible = insumo.Cantidad >= cantidadRequerida;
                    }
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

            return insumoDisponible;
        }

        public bool DisminuirCantidadInsumo(string codigoInsumo, int cantidadParaDisminuir)
        {
            bool insumoDisminuido = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    Insumos insumo = context.Insumos.FirstOrDefault(i => i.CodigoProducto == codigoInsumo);
                    if (insumo != default)
                    {
                        insumo.Cantidad = insumo.Cantidad - cantidadParaDisminuir;
                    }
                    int registrosAfectados = context.SaveChanges();
                    if (registrosAfectados > 0)
                    {
                        insumoDisminuido = true;
                    }

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

            return insumoDisminuido;
        }

    }
}
