using ItaliaPizza_Contratos;
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
        Object _lock = new Object();

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
            InsumoDAO insumoDAO = new InsumoDAO();


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
                            insumoDAO.GuardarInsumo(insumoNuevo);
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
            try
            {
                categoriasProductoVenta.AddRange(
               new CategoriaDAO().RecuperarCategoriasProductoVenta().Select(categoriaProductoVenta =>
                   new Categoria
                   {
                       Id = categoriaProductoVenta.IdCategoriaProductoVenta,
                       Nombre = categoriaProductoVenta.Nombre
                   }));

            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }

            return categoriasProductoVenta;
        }

        public List<ProductoVentaPedidos> RecuperarProductosVenta()
        {
            List<ProductoVentaPedidos> productosVenta = new List<ProductoVentaPedidos> ();
            ProductoDAO productoDAO = new ProductoDAO();
            try
            {
                productosVenta = MapeadorProductosAProductoVenta
                                    .MapearProductosAProductosVenta(
                                        productoDAO.RecuperarProductosParaVenta(),
                                        productoDAO.RecuperarProductos());
                                                return productosVenta;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        
        public bool ValidarDisponibilidadDeProducto(string codigoProducto, int cantidadProductos)
        {
            //lock(_lock)
            {
                ProductoDAO productoDAO = new ProductoDAO();
                InsumoDAO insumoDAO = new InsumoDAO();
                RecetaDAO recetaDAO = new RecetaDAO();

                try
                {
                    if (productoDAO.ValidarSiProductoEnVentaEsInventariado(codigoProducto))
                    {
                        return ValidarDisponibilidadInsumoProducto(codigoProducto, cantidadProductos, insumoDAO);
                    }
                    else
                    {
                        return ValidarDisponibilidadInsumoReceta(codigoProducto, cantidadProductos, recetaDAO, insumoDAO);
                    }
                }
                catch (ExcepcionDataAccess e)
                {
                    throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
                }
            //
            }
        }

        private bool ValidarDisponibilidadInsumoProducto(string codigoProducto, int cantidadProductos, InsumoDAO insumoDAO)
        {
            if (insumoDAO.ValidarDisponibilidadInsumo(codigoProducto, cantidadProductos))
            {
                insumoDAO.ApartarCantidadInsumo(codigoProducto, cantidadProductos);
                return true;
            }
            return false;
        }

        private bool ValidarDisponibilidadInsumoReceta(string codigoProducto, int cantidadProductos, RecetaDAO recetaDAO, InsumoDAO insumoDAO)
        {
            List<RecetasInsumos> insumosEnReceta = recetaDAO.RecuperarInsumosEnReceta(codigoProducto);

            foreach (RecetasInsumos insumo in insumosEnReceta)
            {
                if (!insumoDAO.ValidarDisponibilidadInsumo(insumo.CodigoProducto, ((double)insumo.CantidadInsumo * cantidadProductos)))
                {
                    return false;
                }
            }

            foreach (RecetasInsumos insumo in insumosEnReceta)
            {
                insumoDAO.ApartarCantidadInsumo(insumo.CodigoProducto, ((double)insumo.CantidadInsumo * cantidadProductos));
            }
            return true;
        }

        public bool DisminuirCantidadInsumoPorProducto(string codigoProducto, int cantidadRequerida)
        {
            bool insumosDisminuidos = false;
            RecetaDAO recetaTemporalDAO = new RecetaDAO();
            InsumoDAO insumoDAO = new InsumoDAO();

            try
            {
                List<RecetasInsumos> insumosEnReceta = recetaTemporalDAO.RecuperarInsumosEnReceta(codigoProducto);

                foreach (RecetasInsumos insumo in insumosEnReceta)
                {
                    bool insumoDisminuido =
                        insumoDAO.DisminuirCantidadInsumo(insumo.CodigoProducto, ((double)insumo.CantidadInsumo * cantidadRequerida));
                    if (!insumoDisminuido)
                    {
                        insumosDisminuidos = false;
                        break;
                    }
                }
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return insumosDisminuidos;
        }

        public List<InsumoOrdenCompraDto> RecuperarInsumosActivos()
        {
            try 
                { 
                List<InsumoOrdenCompraDto> insumoOrdenCompras = new List<InsumoOrdenCompraDto>();
                InsumoDAO insumoDAO = new InsumoDAO();
                var insumosActivos = insumoDAO.RecuperarInsumosActivos();
                foreach (var item in insumosActivos)
                {
                    insumoOrdenCompras.Add(AuxiliarConversorDTOADAO.ConvertirInsumosAInsumoOrdenCompraDto(item));
                }
                return insumoOrdenCompras;            
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public bool DesapartarInsumosDeProducto (string codigoProducto, int cantidadParaDesapartar)
        {
            ProductoDAO productoDAO = new ProductoDAO();
            InsumoDAO insumoDAO = new InsumoDAO();
            RecetaDAO recetaDAO = new RecetaDAO();

            try
            {
                if (productoDAO.ValidarSiProductoEnVentaEsInventariado(codigoProducto))
                {
                    return insumoDAO.DesapartarCantidadInsumo(codigoProducto, cantidadParaDesapartar);
                }
                else
                {
                    return DesapartarInsumosDeProductoVenta(codigoProducto, cantidadParaDesapartar, recetaDAO, insumoDAO);
                }
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        private bool DesapartarInsumosDeProductoVenta(string codigoProducto, int cantidadProductos, RecetaDAO recetaDAO, InsumoDAO insumoDAO)
        {
            //TODO: Try-catch
            List<RecetasInsumos> insumosEnReceta = recetaDAO.RecuperarInsumosEnReceta(codigoProducto);

            foreach (RecetasInsumos insumo in insumosEnReceta)
            {
                insumoDAO.DesapartarCantidadInsumo(insumo.CodigoProducto, (double)insumo.CantidadInsumo * cantidadProductos);
            }
            return true;
        }

        public List<Producto> RecuperarProductosTipoInsumo()
        {
            ProductoDAO productoDAO = new ProductoDAO();

            try
            {
                List<Productos> productos = productoDAO.RecuperarProductos();

                List<Productos> productosTemporales = new List<Productos>();
                productosTemporales.AddRange(productos.Where(producto => producto.Insumos != null && producto.ProductosVenta == null));

                List<Producto> productosConvertidos = new List<Producto>();
                foreach (Productos producto in productosTemporales)
                {
                    productosConvertidos.Add(AuxiliarPreparacionDatos.ConvertirProductosAProducto(producto, EnumTiposProducto.Insumo));
                }

                return productosConvertidos;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public List<Producto> RecuperarProductosTipoVenta()
        {
            ProductoDAO productoDAO = new ProductoDAO();
            try
            {
                List<Productos> productos = productoDAO.RecuperarProductos();

                List<Productos> productosTemporales = new List<Productos>();
                productosTemporales.AddRange(productos.Where(producto => producto.ProductosVenta != null));

                List<Producto> productosConvertidos = new List<Producto>();
                foreach (Productos producto in productosTemporales)
                {
                    productosConvertidos.Add(AuxiliarPreparacionDatos.ConvertirProductosAProducto(producto, EnumTiposProducto.Venta));
                }

                return productosConvertidos;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public List<Categoria> RecuperarCategoriasInsumo()
        {
            List<Categoria> categoriasInsumo = new List<Categoria>();

            try
            {
                categoriasInsumo.AddRange(
               new CategoriaDAO().RecuperarCategoriasInsumo().Select(categoriaInsumo =>
                   new Categoria
                   {
                       Id = categoriaInsumo.IdCategoriaInsumo,
                       Nombre = categoriaInsumo.Nombre
                   }));

            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }

            return categoriasInsumo;
        }

        public bool ValidarDesactivacion(string codigoProducto)
        {
            bool esDesactivacionValida = false;
            int numeroPedido = 0;
            PedidoDAO pedidoDAO = new PedidoDAO();

            try
            {
                numeroPedido = pedidoDAO.ValidarDesactivacionProducto(codigoProducto);

                if (numeroPedido == 0)
                {
                    esDesactivacionValida = true;
                }

                return esDesactivacionValida;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public bool ValidarDesactivacionInsumo(string codigoProducto)
        {
            bool esDesactivacionValida = false;
            InsumoDAO insumoDAO = new InsumoDAO();

            try
            {
                esDesactivacionValida = insumoDAO.ValidarDesactivacionInsumo(codigoProducto);

                return esDesactivacionValida;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public int DesactivarProducto(string codigoProducto)
        {
            int filasAfectadas = -1;
            ProductoDAO productoDAO = new ProductoDAO();

            try
            {
                filasAfectadas = productoDAO.DesactivarProducto(codigoProducto);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }

            return filasAfectadas;
        }

        public int ActivarProducto(string codigoProducto)
        {
            int filasAfectadas = -1;
            ProductoDAO productoDAO = new ProductoDAO();

            try
            {
                filasAfectadas = productoDAO.ActivarProducto(codigoProducto);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }

            return filasAfectadas;
        }

        public List<Producto> RecuperarProductosInventariados()
        {
            ProductoDAO productoDAO = new ProductoDAO();

            try
            {
                List<Productos> productos = productoDAO.RecuperarProductos();

                List<Productos> productosTemporales = new List<Productos>();
                productosTemporales.AddRange(productos.Where(producto => producto.Insumos != null && producto.EsInventariado == true));

                List<Producto> productosConvertidos = new List<Producto>();
                foreach (Productos producto in productosTemporales)
                {
                    productosConvertidos.Add(AuxiliarPreparacionDatos.ConvertirProductosAProductoInventariado(producto));
                }

                return productosConvertidos;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public int ActualizarProducto(Producto producto)
        {
            ProductoDAO productoDAO= new ProductoDAO();
            try
            {
                return productoDAO.ActualizarProducto(producto);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }
    }
}
