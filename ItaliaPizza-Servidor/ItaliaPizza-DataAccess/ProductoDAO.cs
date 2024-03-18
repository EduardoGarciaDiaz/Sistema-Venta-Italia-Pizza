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
            bool existeProducto = true;

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
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

        public List<ProductoSinReceta> RecuperarProductosSinReceta()
        {
            using (var context = new ItaliaPizzaEntities())
            {
                var productosSinReceta = from p in context.Productos
                                         join pv in context.ProductosVenta on p.CodigoProducto equals pv.CodigoProducto
                                         join r in context.Recetas on pv.CodigoProducto equals r.CodigoProducto into rGroup
                                         from r in rGroup.DefaultIfEmpty()
                                         where r.CodigoProducto == null
                                         && !context.Insumos.Any(i => i.CodigoProducto == p.CodigoProducto)
                                         select new ProductoSinReceta
                                         {
                                             Codigo = p.CodigoProducto,
                                             Nombre = p.Nombre,
                                             Foto = pv.Foto,
                                         };


                return productosSinReceta.ToList();
            }
        }
    }
}
