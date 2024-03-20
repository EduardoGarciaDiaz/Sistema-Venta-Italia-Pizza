using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace ItaliaPizza_DataAccess
{
    public static class OrdenDeCompraDAO
    {

        public static int GuardarOrdenDeCompra(OrdenesCompra ordenesCompra)
        {
            int idOrdenNueva = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var estado = context.EstadosOrdenCompra.FirstOrDefault(ord => ord.Nombre.Equals("Borrador"));
                    ordenesCompra.IdEstadoOrdenCompra = estado.IdEstadoOrdenCompra;
                    ordenesCompra.EstadosOrdenCompra = estado;
                    context.OrdenesCompra.Add(ordenesCompra);
                    context.SaveChanges();
                    idOrdenNueva = ordenesCompra.IdOrdenCompra;
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
            return idOrdenNueva;
        }

        public static int GuardarElementoInsumoDeOrdenDeCompra(List<OrdenesCompraInsumos> elementoInsumoOrdenesCompra)
        {
            int idOrdenNueva = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.OrdenesCompraInsumos.AddRange(elementoInsumoOrdenesCompra);
                    idOrdenNueva = context.SaveChanges();                     
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
            return idOrdenNueva;
        }

        public static bool ValidarOrdenDeCompraExiste(int idOrdenCompra)
        {
            bool idOrdenNueva = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.OrdenesCompraInsumos.Any(orden => orden.IdOrdenCompra == idOrdenCompra);
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
            return idOrdenNueva;
        }

        public static OrdenesCompra RecuperarOrdenDeCompra(int idOrdenCompra)
        {
            OrdenesCompra ordenesCompra = new OrdenesCompra();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.OrdenesCompra.Include(ord => ord.Proveedores).Include(ord => ord.OrdenesCompraInsumos).FirstOrDefault(ord => ord.IdOrdenCompra == idOrdenCompra);
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
            return ordenesCompra;
        }

        public static int CambiarEstadoOrdenCompraAEnviado(OrdenesCompra ordenesCompra)
        {
            int idOrdenNueva = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var estado = context.EstadosOrdenCompra.FirstOrDefault(ord => ord.Nombre.Equals("Enviado"));
                    ordenesCompra.IdEstadoOrdenCompra = estado.IdEstadoOrdenCompra;
                    ordenesCompra.EstadosOrdenCompra = estado;
                    context.OrdenesCompra.AddOrUpdate(ordenesCompra);                    
                    idOrdenNueva = context.SaveChanges(); 
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
            return idOrdenNueva;
        }

    }
}



  
