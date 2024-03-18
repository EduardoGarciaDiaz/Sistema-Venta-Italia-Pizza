using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using ItaliaPizza_Servicios.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ProductoDAO gestionProducto = new ProductoDAO();

            List<CategoriasInsumo> categoriasInsumo = gestionProducto.RecuperarCategoriasInsumo();
            List<CategoriasProductoVenta> categoriasProductoVenta = gestionProducto.RecuperarCategoriasProductoVenta();

            List<Categoria> categorias = AuxiliarPreparacionDatos.PrepararListaCategorias(categoriasProductoVenta, categoriasInsumo);

            return categorias;
        }

        public List<UnidadMedida> RecuperarUnidadesMedida()
        {
            List<UnidadMedida> listaUnidades = new List<UnidadMedida>();
            ProductoDAO gestionProducto = new ProductoDAO();
            List<UnidadesMedida> unidadesMedida = gestionProducto.RecuperarUnidadesMedida();

            listaUnidades.AddRange(unidadesMedida.Select(unidad => new UnidadMedida
            {
                Id = unidad.IdUnidadMedida,
                Nombre = unidad.Nombre
            }));

            return listaUnidades;
        }


        public bool ValidarCodigoProducto(string codigoProducto)
        {
            bool esCodigoUnico = false;

            ProductoDAO gestionProducto = new ProductoDAO();
            bool existeProducto = gestionProducto.ValidarCodigoProducto(codigoProducto);
            
            if (!existeProducto)
            {
                esCodigoUnico = true;
            }

            return esCodigoUnico;
        }

        public int GuardarProducto(Producto producto)
        {
            ProductoDAO gestionProducto = new ProductoDAO();

            Insumo insumo = producto.Insumo;
            ProductoVenta productoVenta = producto.ProductoVenta;

            Productos productoNuevo = AuxiliarConversorDTOADAO.ConvertirProductoAProductos(producto);

            int filasAfectadas = gestionProducto.GuardarProducto(productoNuevo);

            if (filasAfectadas > 0)
            {
                if (insumo != null)
                {
                    Insumos insumoNuevo = AuxiliarConversorDTOADAO.ConvertirInsumoAInsumos(insumo);
                    gestionProducto.GuardarInsumo(insumoNuevo);
                }

                if (productoVenta != null)
                {
                    ProductosVenta productoVentaNuevo = AuxiliarConversorDTOADAO.ConvertirProductoVentaAProductosVenta(productoVenta);
                    gestionProducto.GuardarProductoVenta(productoVentaNuevo);
                }
            }

            return filasAfectadas;
        }

        public List<ProductoSinReceta> RecuperarProductosSinReceta()
        {
            ProductoDAO productoDAO = new ProductoDAO();
            List<ProductoSinReceta> productosSinReceta = productoDAO.RecuperarProductosSinReceta();

            return productosSinReceta;
        }

        public List<InsumoRegistroReceta> RecuperarInsumos()
        {
            InsumoDAO insumoDAO = new InsumoDAO();
            List<InsumoRegistroReceta> insumos = insumoDAO.RecuperarInsumos();

            return insumos;
        }
        
        public List<Categoria> RecuperarCategoriasProductoVenta ()
        {
            List<Categoria> categoriasProductoVenta = new List<Categoria>();
            categoriasProductoVenta.AddRange(
                new ProductoDAO().RecuperarCategoriasProductoVenta().Select(categoriaProductoVenta => 
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
