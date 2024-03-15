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
            List<Categoria> categorias = new List<Categoria>();
            GestionProducto gestionProducto = new GestionProducto();

            List<CategoriasInsumo> categoriasInsumo = new List<CategoriasInsumo>();
            List<CategoriasProductoVenta> categoriasProductoVenta = new List<CategoriasProductoVenta>();

            categoriasInsumo = gestionProducto.RecuperarCategoriasInsumo();
            categoriasProductoVenta = gestionProducto.RecuperarCategoriasProductoVenta();

            categorias = PrepararListaCategorias(categorias, categoriasProductoVenta, categoriasInsumo);

            return categorias;
        }

        private List<Categoria> PrepararListaCategorias(List<Categoria> categorias,
            List<CategoriasProductoVenta> categoriasProductoVenta,
            List<CategoriasInsumo> categoriasInsumo)
        {
            categorias.AddRange(categoriasProductoVenta.Select(categoriaProductoVenta => new Categoria
            {
                Id = categoriaProductoVenta.IdCategoriaProductoVenta,
                Nombre = categoriaProductoVenta.Nombre
            }));

            categorias.AddRange(categoriasInsumo.Select(categoriaInsumo => new Categoria
            {
                Id = categoriaInsumo.IdCategoriaInsumo,
                Nombre = categoriaInsumo.Nombre
            }));

            return categorias;
        }


        public bool ValidarCodigoProducto(string codigoProducto)
        {
            throw new NotImplementedException();
        }

        public int GuardarProducto(Producto producto)
        {
            throw new NotImplementedException();
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

        public List<ProductoVenta> RecuperarProductosVenta()
        {
            List<ProductoVenta> productosVenta = new List<ProductoVenta> ();
            ProductoDAO productoDAO = new ProductoDAO();
            productosVenta = MapeadorProductosAProductoVenta
                .MapearProductosAProductosVenta(
                    productoDAO.RecuperarProductosVenta(), 
                    productoDAO.RecuperarProductos());
            return productosVenta;
        }

        public bool ValidarDisponibilidadDeProducto(string codigoProducto)
        {
            bool productoDisponible = true;
            RecetaTemporalDAO recetaTemporalDAO = new RecetaTemporalDAO();
            Recetas recetaProducto = recetaTemporalDAO.RecuperarRecetaDeProducto(codigoProducto);
            InsumoDAO insumoDAO = new InsumoDAO();
            foreach(RecetasInsumos insumo in recetaProducto.RecetasInsumos)
            {
                bool insumoDisponible = insumoDAO.ValidarDisponibilidadInsumo(insumo.CodigoProducto, (int)insumo.CantidadInsumo);
                if (!insumoDisponible)
                {
                    productoDisponible = false;
                    break;
                }
            }
            return productoDisponible;
        }
    }
}
