using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
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

        public List<UnidadMedida> RecuperarUnidadesMedida()
        {
            List<UnidadMedida> listaUnidades = new List<UnidadMedida>();
            GestionProducto gestionProducto = new GestionProducto();
            List<UnidadesMedida> unidadesMedida = new List<UnidadesMedida>();

            unidadesMedida = gestionProducto.RecuperarUnidadesMedida();

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

            GestionProducto gestionProducto = new GestionProducto();
            bool existeProducto = gestionProducto.ValidarCodigoProducto(codigoProducto);
            
            if (!existeProducto)
            {
                esCodigoUnico = true;
            }

            return esCodigoUnico;
        }

        public int GuardarProducto(Producto producto)
        {
            GestionProducto gestionProducto = new GestionProducto();

            Insumo insumo = producto.Insumo;
            ProductoVenta productoVenta = producto.ProductoVenta;

            Productos productoNuevo = CrearProductoEntityFramework(producto);

            int filasAfectadas = gestionProducto.GuardarProducto(productoNuevo);

            if (filasAfectadas > 0)
            {
                if (insumo != null)
                {
                    Insumos insumoNuevo = CrearInsumoEntityFramework(insumo);
                    gestionProducto.GuardarInsumo(insumoNuevo);
                }

                if (productoVenta != null)
                {
                    ProductosVenta productoVentaNuevo = CrearProductoVentaEntityFramework(productoVenta);
                    gestionProducto.GuardarProductoVenta(productoVentaNuevo);
                }
            }

            return filasAfectadas;
        }

        private Productos CrearProductoEntityFramework(Producto producto)
        {
            Productos productos = new Productos()
            {
                CodigoProducto = producto.Codigo,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                EsInventariado = producto.EsInventariado,
                EsActivo = producto.esActivo
            };

            return productos;
        }

        private Insumos CrearInsumoEntityFramework(Insumo insumo)
        {
            Insumos insumos = new Insumos()
            {
                CodigoProducto = insumo.Codigo,
                Cantidad = insumo.Cantidad,
                Costo = insumo.CostoUnitario,
                Restricciones = insumo.Restriccion,
                IdUnidadMedida = insumo.UnidadMedida.Id,
                IdCategoriaInsumo = insumo.Categoria.Id
            };

            return insumos;
        }

        private ProductosVenta CrearProductoVentaEntityFramework(ProductoVenta productoVenta)
        {
            ProductosVenta productosVenta = new ProductosVenta()
            {
                CodigoProducto = productoVenta.Codigo,
                Precio = productoVenta.Precio,
                IdCategoriaProductoVenta = productoVenta.Categoria.Id // ,
                // Foto = [productoVenta.Foto
            };

            return productosVenta;
        }




    }
}
