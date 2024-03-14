﻿using System;
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

        public List<UnidadesMedida> RecuperarUnidadesMedida()
        {
            List<UnidadesMedida> unidadesMedida = new List<UnidadesMedida>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    unidadesMedida = context.UnidadesMedida.ToList();
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

            return unidadesMedida;
        }

        public bool ValidarCodigoProducto(string codigoProducto)
        {
            using (var context = new ItaliaPizzaEntities())
            {
                bool existeProducto = context.Productos.Any(p => p.CodigoProducto == codigoProducto);

                return existeProducto;
            }
        }

        public int GuardarProducto(Productos producto)
        {
            int filasAfectadas = -1;

            if (producto != null)
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.Productos.Add(producto);
                    filasAfectadas = context.SaveChanges();
                }
            }

            return filasAfectadas;
        }

        public int GuardarInsumo(Insumos insumo)
        {
            int filasAfectadas = -1;

            if (insumo != null)
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.Insumos.Add(insumo);
                    filasAfectadas = context.SaveChanges();
                }
            }

            return filasAfectadas;
        }

        public int GuardarProductoVenta(ProductosVenta productoVenta)
        {
            int filasAfectadas = -1;

            if (productoVenta != null)
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.ProductosVenta.Add(productoVenta);
                    filasAfectadas = context.SaveChanges();
                }
            }

            return filasAfectadas;
        }
    }
}
