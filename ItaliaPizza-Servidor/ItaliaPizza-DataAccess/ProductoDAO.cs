using System;
﻿using ItaliaPizza_Contratos.DTOs;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza_DataAccess.Excepciones;
using System.Data.Entity;

namespace ItaliaPizza_DataAccess
{

    public class ProductoDAO
    {
        public ProductoDAO() { }

        public List<Productos> RecuperarProductos()
        {
            List<Productos> productos = new List<Productos>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    productos = context.Productos
                        .Include("ProductosVenta")
                        .Include("ProductosVenta.CategoriasProductoVenta")
                        .Include("Insumos")
                        .Include("Insumos.UnidadesMedida")
                        .Include("Insumos.CategoriasInsumo")
                        .ToList();
                }
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
            return productos;
        }

        public List<ProductosVenta> RecuperarProductosParaVenta()
        {
            List<ProductosVenta> productosVenta = new List<ProductosVenta>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    productosVenta = context.ProductosVenta.Where(pv =>
                    context.Recetas.Any(r => r.CodigoProducto == pv.CodigoProducto)
                    || context.Insumos.Any(i => i.CodigoProducto == pv.CodigoProducto))
                        .ToList();
                }
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
            return productosVenta;
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

                return existeProducto;
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

                return filasAfectadas;
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

                return filasAfectadas;
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

        public List<ProductoSinReceta> RecuperarProductosSinReceta()
        {
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var productosSinReceta = from p in context.Productos
                                             join pv in context.ProductosVenta on p.CodigoProducto equals pv.CodigoProducto
                                             join r in context.Recetas on pv.CodigoProducto equals r.CodigoProducto into rGroup
                                             from r in rGroup.DefaultIfEmpty()
                                             where r.CodigoProducto == null
                                             && !context.Insumos.Any(i => i.CodigoProducto == p.CodigoProducto)
                                             && p.EsActivo == true
                                             select new ProductoSinReceta
                                             {
                                                 Codigo = p.CodigoProducto,
                                                 Nombre = p.Nombre,
                                                 Foto = pv.Foto,
                                             };

                    return productosSinReceta.ToList();
                }
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
        public bool ValidarSiProductoEnVentaEsInventariado(string codigoProducto)
        {
            bool productoEsInventariado = false;

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    Insumos insumo = context.Insumos.FirstOrDefault(i => i.CodigoProducto == codigoProducto);
                    if (insumo != default)
                    {
                        productoEsInventariado = true;
                    }
                }
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

            return productoEsInventariado;
        }

        public int DesactivarProducto(string codigoProducto)
        {
            int filasAfectadas = -1;

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    Productos producto = context.Productos.FirstOrDefault(p => p.CodigoProducto == codigoProducto);
                    if (producto != default)
                    {
                        producto.EsActivo = false;
                        filasAfectadas = context.SaveChanges();
                    }
                }

                return filasAfectadas;
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

        public int ActivarProducto(string codigoProducto)
        {
            int filasAfectadas = -1;

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    Productos producto = context.Productos.FirstOrDefault(p => p.CodigoProducto == codigoProducto);
                    if (producto != default)
                    {
                        producto.EsActivo = true;
                        filasAfectadas = context.SaveChanges();
                    }
                }

                return filasAfectadas;
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

        public List<ProductosVenta> RecuperarProductosVentaPorCategoriaActivos(List<Categoria> categorias)
        {
            List<ProductosVenta> productosVenta = new List<ProductosVenta>();

            try
            {
                List<ProductosVenta> productosVentaRecuperados = new List<ProductosVenta>();
                using (var context = new ItaliaPizzaEntities())
                {
                    productosVentaRecuperados = context.ProductosVenta.Where(cat => (bool) cat.Productos.EsActivo)
                            .Include(pro => pro.Productos)
                            .Include(pro => pro.CategoriasProductoVenta)
                            .Include(pro => pro.Productos.Insumos)
                            .Include(pro => pro.Productos.Insumos.UnidadesMedida)
                            .ToList();
                    foreach (var item in productosVentaRecuperados)
                    {

                        if (categorias.Any(cat => cat.Nombre.Equals(item.CategoriasProductoVenta.Nombre))) productosVenta.Add(item);
                    }
                }
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
            return productosVenta;
        }


        public int ActualizarProducto(Producto producto)
        {
            int filasAfectadas = -1;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    Productos productos = context.Productos.FirstOrDefault(p => p.CodigoProducto.Equals(producto.Codigo));
                    if (productos != default)
                    {
                        productos.Nombre = producto.Nombre;
                        productos.Descripcion = producto.Descripcion;
                    }
                    if (producto.ProductoVenta != null)
                    {
                        ProductosVenta productosVenta = context.ProductosVenta.FirstOrDefault(p => p.CodigoProducto.Equals(producto.Codigo));
                        if ( productosVenta != default)
                        {
                            productosVenta.Precio = producto.ProductoVenta.Precio;
                            productosVenta.Foto = producto.ProductoVenta.Foto;
                            productosVenta.IdCategoriaProductoVenta = producto.ProductoVenta.Categoria.Id;
                        }
                    }
                    if (producto.Insumo != null)
                    {
                        Insumos insumos = context.Insumos.FirstOrDefault(i => i.CodigoProducto.Equals(producto.Codigo));
                        if ( insumos != default)
                        {
                            insumos.Cantidad = producto.Insumo.Cantidad;
                            insumos.IdUnidadMedida = producto.Insumo.UnidadMedida.Id;
                            insumos.Costo = producto.Insumo.CostoUnitario;
                            insumos.IdCategoriaInsumo = producto.Insumo.Categoria.Id;
                            insumos.Restricciones = producto.Insumo.Restriccion;
                        }
                    }
                    filasAfectadas = context.SaveChanges();
                }               
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
                return filasAfectadas;
        }
        

        public List<ProductosVenta> RecuperarTodosProductosVentaPorCategoria(List<Categoria> categorias)
        {
            List<ProductosVenta> productosVenta = new List<ProductosVenta>();

            try
            {
                List<ProductosVenta> productosVentaRecuperados = new List<ProductosVenta>();
                using (var context = new ItaliaPizzaEntities())
                {
                    productosVentaRecuperados = context.ProductosVenta
                            .Include(pro => pro.Productos)
                            .Include(pro => pro.CategoriasProductoVenta)
                            .Include(pro => pro.Productos.Insumos)
                            .Include(pro => pro.Productos.Insumos.UnidadesMedida)
                            .ToList();
                    foreach (var item in productosVentaRecuperados)
                    {

                        if (categorias.Any(cat => cat.Nombre.Equals(item.CategoriasProductoVenta.Nombre))) productosVenta.Add(item);
                    }
                }
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
            return productosVenta;
        }


    } 
}
