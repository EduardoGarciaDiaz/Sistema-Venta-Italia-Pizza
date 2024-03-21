using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using ItaliaPizza_Contratos.DTOs;

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

        public static int GuardarInsumoOrdenDeCompra(List<OrdenesCompraInsumos> elementoInsumoOrdenesCompra)
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

        

        public static OrdenesCompra RecuperarOrdenDeCompra(int idOrdenCompra)
        {
            OrdenesCompra ordenCompra = new OrdenesCompra();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    ordenCompra = context.OrdenesCompra.Include(ord => ord.Proveedores).Include(ord => ord.EstadosOrdenCompra).Include(ord => ord.OrdenesCompraInsumos).FirstOrDefault(ord => ord.IdOrdenCompra == idOrdenCompra);

                    if (ordenCompra != null)
                    {
                        var insumosRelacionados = ordenCompra.OrdenesCompraInsumos.Select(oci => oci.Insumos).ToList();                          
                        for (int i = 0; i < insumosRelacionados.Count; i++)
                        {
                            ordenCompra.OrdenesCompraInsumos.ToArray()[i].Insumos = insumosRelacionados.FirstOrDefault(ins => ins.CodigoProducto.Equals(ordenCompra.OrdenesCompraInsumos.ToArray()[i].CodigoProducto));
                        }
                        var productos = insumosRelacionados.Select(insumo => insumo.Productos).Distinct().ToList();
                        for (int i = 0; i < productos.Count; i++)
                        {
                            ordenCompra.OrdenesCompraInsumos.ToArray()[i].Insumos.Productos = productos.FirstOrDefault(ins => ins.CodigoProducto.Equals(ordenCompra.OrdenesCompraInsumos.ToArray()[i].CodigoProducto));
                        }

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
            return ordenCompra;
        }

        public static int CambiarEstadoOrdenCompraAEnviado(OrdenesCompra ordenesCompra)
        {
            int idOrdenNueva = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var estado = context.EstadosOrdenCompra.FirstOrDefault(ord => ord.Nombre.Equals("Enviada"));
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



  
