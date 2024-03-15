using System;
﻿using ItaliaPizza_Contratos.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{

    public class ProductoDAO
    {

        public ProductoDAO() { }
    public class ProductoDAO
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

        public List<Productos> RecuperarProductos()
        {
            List<Productos> productos = new List<Productos>();
        public List<UnidadesMedida> RecuperarUnidadesMedida()
        {
            List<UnidadesMedida> unidadesMedida = new List<UnidadesMedida>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    productos = context.Productos.ToList();
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

            return productos;
        }

        public List<ProductosVenta> RecuperarProductosVenta()
        {
            List<ProductosVenta> productosVenta = new List<ProductosVenta>();
            return unidadesMedida;
        }

        public bool ValidarCodigoProducto(string codigoProducto)
        {
            bool existeProducto = true;

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    productosVenta = context.ProductosVenta.ToList();
                    existeProducto = context.Productos.Any(p => p.CodigoProducto == codigoProducto);
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

            return productosVenta;
            return existeProducto;
        }

        public int GuardarProducto(Productos producto)
        {
            int filasAfectadas = -1;

            try
            {
                if (producto != null)
                {
                    using (var context = new ItaliaPizzaEntities())
                    {
                        context.Productos.Add(producto);
                        filasAfectadas = context.SaveChanges();
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

            return filasAfectadas;
        }

        public int GuardarInsumo(Insumos insumo)
        {
            int filasAfectadas = -1;

            try
            {
                if (insumo != null)
                {
                    using (var context = new ItaliaPizzaEntities())
                    {
                        context.Insumos.Add(insumo);
                        filasAfectadas = context.SaveChanges();
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

            return filasAfectadas;
        }

        public int GuardarProductoVenta(ProductosVenta productoVenta)
        {
            int filasAfectadas = -1;

            try
            {
                if (productoVenta != null)
                {
                    using (var context = new ItaliaPizzaEntities())
                    {
                        context.ProductosVenta.Add(productoVenta);
                        filasAfectadas = context.SaveChanges();
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

            return filasAfectadas;
        }
    }
}
