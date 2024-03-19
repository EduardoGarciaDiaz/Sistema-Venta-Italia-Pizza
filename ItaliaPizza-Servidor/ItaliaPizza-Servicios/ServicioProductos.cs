using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_Contratos.Excepciones;
using ItaliaPizza_DataAccess;
using ItaliaPizza_DataAccess.Excepciones;
using ItaliaPizza_Servicios.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioProductos
    {
        public void OperacionProductosEjemplo()
        {
            throw new NotImplementedException();
        }

        public List<Categoria> RecuperarCategorias()
        {
            CategoriaDAO categoriaDAO = new CategoriaDAO();

            try
            {
                List<CategoriasInsumo> categoriasInsumo = categoriaDAO.RecuperarCategoriasInsumo();
                List<CategoriasProductoVenta> categoriasProductoVenta = categoriaDAO.RecuperarCategoriasProductoVenta();

                List<Categoria> categorias = AuxiliarPreparacionDatos.PrepararListaCategorias(categoriasProductoVenta, categoriasInsumo);

                return categorias;
            }
            catch (ExcepcionDataAccess ex)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(ex);
            }
        }

        public List<UnidadMedida> RecuperarUnidadesMedida()
        {
            List<UnidadMedida> listaUnidades = new List<UnidadMedida>();
            UnidadMedidaDAO unidadMedidaDAO = new UnidadMedidaDAO();

            try
            {
                List<UnidadesMedida> unidadesMedida = unidadMedidaDAO.RecuperarUnidadesMedida();

                listaUnidades.AddRange(unidadesMedida.Select(unidad => new UnidadMedida
                {
                    Id = unidad.IdUnidadMedida,
                    Nombre = unidad.Nombre
                }));

                return listaUnidades;
            }
            catch (ExcepcionDataAccess ex)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(ex);
            }
        }


        public bool ValidarCodigoProducto(string codigoProducto)
        {
            bool esCodigoUnico = false;
            ProductoDAO productoDAO = new ProductoDAO();

            try
            {
                bool existeProducto = productoDAO.ValidarCodigoProducto(codigoProducto);

                if (!existeProducto)
                {
                    esCodigoUnico = true;
                }

                return esCodigoUnico;
            }
            catch (ExcepcionDataAccess ex)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(ex);
            }
        }

        public int GuardarProducto(Producto producto)
        {
            int filasAfectadas = -1;
            ProductoDAO productoDAO = new ProductoDAO();

            if (producto != null)
            {
                Insumo insumo = producto.Insumo;
                ProductoVenta productoVenta = producto.ProductoVenta;
                Productos productoNuevo = AuxiliarConversorDTOADAO.ConvertirProductoAProductos(producto);

                try
                {
                    filasAfectadas = productoDAO.GuardarProducto(productoNuevo);

                    if (filasAfectadas > 0)
                    {
                        if (insumo != null)
                        {
                            Insumos insumoNuevo = AuxiliarConversorDTOADAO.ConvertirInsumoAInsumos(insumo);
                            productoDAO.GuardarInsumo(insumoNuevo);
                        }

                        if (productoVenta != null)
                        {
                            ProductosVenta productoVentaNuevo = AuxiliarConversorDTOADAO.ConvertirProductoVentaAProductosVenta(productoVenta);
                            productoDAO.GuardarProductoVenta(productoVentaNuevo);
                        }
                    }
                }
                catch (ExcepcionDataAccess ex)
                {
                    throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(ex);
                }
            }

            return filasAfectadas;
        }

        public List<ProductoSinReceta> RecuperarProductosSinReceta()
        {
            ProductoDAO productoDAO = new ProductoDAO();

            try
            {
                List<ProductoSinReceta> productosSinReceta = productoDAO.RecuperarProductosSinReceta();

                return productosSinReceta;
            }
            catch (ExcepcionDataAccess ex)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(ex);
            }
        }

        public List<InsumoRegistroReceta> RecuperarInsumos()
        {
            InsumoDAO insumoDAO = new InsumoDAO();

            try
            {
                List<InsumoRegistroReceta> insumos = insumoDAO.RecuperarInsumos();

                return insumos;
            }
            catch (ExcepcionDataAccess ex)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(ex);
            }
        }
        
        public List<Categoria> RecuperarCategoriasProductoVenta()
        {
            List<Categoria> categoriasProductoVenta = new List<Categoria>();
            categoriasProductoVenta.AddRange(
                new CategoriaDAO().RecuperarCategoriasProductoVenta().Select(categoriaProductoVenta => 
                    new Categoria
                    {
                        Id = categoriaProductoVenta.IdCategoriaProductoVenta,
                        Nombre = categoriaProductoVenta.Nombre
                    }));

            return categoriasProductoVenta;
        }

        public List<ProductoVentaPedidos> RecuperarProductosVenta()
        {
            List<ProductoVentaPedidos> productosVenta = new List<ProductoVentaPedidos> ();
            ProductoDAO productoDAO = new ProductoDAO();
            productosVenta = MapeadorProductosAProductoVenta
                .MapearProductosAProductosVenta(
                    productoDAO.RecuperarProductosParaVenta(), 
                    productoDAO.RecuperarProductos());
            return productosVenta;
        }

        public bool ValidarDisponibilidadDeProducto(string codigoProducto, int cantidadProductos)
        {
            bool productoDisponible = true;
            ProductoDAO productoDAO = new ProductoDAO();
            RecetaTemporalDAO recetaTemporalDAO = new RecetaTemporalDAO();
            InsumoDAO insumoDAO = new InsumoDAO();

            if (productoDAO.ValidarSiProductoEnVentaEsInventariado(codigoProducto))
            {
                productoDisponible = insumoDAO.ValidarDisponibilidadInsumo(codigoProducto, cantidadProductos);

            } 
            else
            {
                List<RecetasInsumos> insumosEnReceta = recetaTemporalDAO.RecuperarInsumosEnReceta(codigoProducto);

                foreach (RecetasInsumos insumo in insumosEnReceta)
                {
                    bool insumoDisponible =
                        insumoDAO.ValidarDisponibilidadInsumo(insumo.CodigoProducto, ((int)insumo.CantidadInsumo * cantidadProductos));
                    if (!insumoDisponible)
                    {
                        productoDisponible = false;
                        break;
                    }
                }
            }
            return productoDisponible;
        }

        public bool DisminuirCantidadInsumoPorProducto(string codigoProducto, int cantidadRequerida)
        {
            bool insumosDisminuidos = false;
            RecetaTemporalDAO recetaTemporalDAO = new RecetaTemporalDAO();
            InsumoDAO insumoDAO = new InsumoDAO();
            List<RecetasInsumos> insumosEnReceta = recetaTemporalDAO.RecuperarInsumosEnReceta(codigoProducto);

            foreach (RecetasInsumos insumo in insumosEnReceta)
            {
                bool insumoDisminuido =
                    insumoDAO.DisminuirCantidadInsumo(insumo.CodigoProducto, ((int)insumo.CantidadInsumo * cantidadRequerida));
                if (!insumoDisminuido)
                {
                    insumosDisminuidos = false;
                    break;
                }
            }
            return insumosDisminuidos;
        }
    }
}
