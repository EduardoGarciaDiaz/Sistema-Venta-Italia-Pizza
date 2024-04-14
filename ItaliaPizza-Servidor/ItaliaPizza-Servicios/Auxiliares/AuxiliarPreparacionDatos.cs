using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class AuxiliarPreparacionDatos
    {
        public static List<Categoria> PrepararListaCategorias(
            List<CategoriasProductoVenta> categoriasProductoVenta,
            List<CategoriasInsumo> categoriasInsumo)
        {
            List<Categoria> categorias = new List<Categoria>();

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

        public static Producto ConvertirProductosAProducto(Productos productos, EnumTiposProducto tipoProducto)
        {
            Producto producto = new Producto()
            {
                Codigo = productos.CodigoProducto,
                Nombre = productos.Nombre,
                Descripcion = productos.Descripcion,
                EsInventariado = (bool)productos.EsInventariado,
                EsActivo = (bool)productos.EsActivo,
            };

            if (tipoProducto == EnumTiposProducto.Insumo)
            {
                producto.Insumo = AsignarInsumo(productos.Insumos);
            }
            if (tipoProducto == EnumTiposProducto.Venta)
            {
                producto.ProductoVenta = AsignarProductoVenta(productos.ProductosVenta);
                if (producto.EsInventariado)
                {
                    producto.Insumo = AsignarInsumo(productos.Insumos);
                }
            }

            return producto;
        }

        private static Insumo AsignarInsumo(Insumos insumoProducto)
        {
            Insumo insumo = new Insumo()
            {
                Codigo = insumoProducto.CodigoProducto,
                Cantidad = (float)insumoProducto.Cantidad,
                UnidadMedida = new UnidadMedida()
                {
                    Id = (int)insumoProducto.IdUnidadMedida,
                    Nombre = insumoProducto.UnidadesMedida.Nombre
                },
                CostoUnitario = (float)insumoProducto.Costo,
                Restriccion = insumoProducto.Restricciones,
                Categoria = new Categoria()
                {
                    Id = (int)insumoProducto.IdCategoriaInsumo,
                    Nombre = insumoProducto.CategoriasInsumo.Nombre
                }
            };

            return insumo;
        }

        private static ProductoVenta AsignarProductoVenta(ProductosVenta productoVenta)
        {
            ProductoVenta producto = new ProductoVenta()
            {
                Codigo = productoVenta.CodigoProducto,
                Precio = (float)productoVenta.Precio,
                Foto = productoVenta.Foto,
                Categoria = new Categoria()
                {
                    Id = (int)productoVenta.IdCategoriaProductoVenta,
                    Nombre = productoVenta.CategoriasProductoVenta.Nombre
                }
            };

            return producto;
        }
    }
}
