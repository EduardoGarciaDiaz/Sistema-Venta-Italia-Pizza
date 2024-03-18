using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class AuxiliarConversorDTOADAO
    {
        public static Productos ConvertirProductoAProductos(Producto producto)
        {
            Productos productos = new Productos()
            {
                CodigoProducto = producto.Codigo,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                EsInventariado = producto.EsInventariado,
                EsActivo = producto.EsActivo
            };

            return productos;
        }

        public static Insumos ConvertirInsumoAInsumos(Insumo insumo)
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

        public static ProductosVenta ConvertirProductoVentaAProductosVenta(ProductoVenta productoVenta)
        {
            ProductosVenta productosVenta = new ProductosVenta()
            {
                CodigoProducto = productoVenta.Codigo,
                Precio = productoVenta.Precio,
                IdCategoriaProductoVenta = productoVenta.Categoria.Id,
                Foto = productoVenta.Foto
            };

            return productosVenta;
        }

        public static Recetas ConvertirRecetaProductoARecetas(RecetaProducto receta)
        {
            Recetas recetas = new Recetas()
            {
                CodigoProducto = receta.CodigoProducto
            };

            return recetas;
        }

        public static RecetasInsumos ConvertirInsumoRecetaARecetasInsumos(InsumoReceta insumoReceta, int idReceta)
        {
            RecetasInsumos recetasInsumos = new RecetasInsumos()
            {
                CantidadInsumo = insumoReceta.Cantidad,
                IdReceta = idReceta,
                CodigoProducto = insumoReceta.Codigo
            };

            return recetasInsumos;
        }
    }
}
