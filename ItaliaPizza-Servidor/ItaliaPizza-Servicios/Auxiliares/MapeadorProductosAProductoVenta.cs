using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class MapeadorProductosAProductoVenta
    {

        public static List<ProductoVentaPedidos> MapearProductosAProductosVenta(List<ProductosVenta> productosVenta, List<Productos> productos)
        {
            List<ProductoVentaPedidos> productosEnVenta = new List<ProductoVentaPedidos>();
            
            foreach (var productoVenta in productosVenta)
            {
                var producto = productos.FirstOrDefault(p => p.CodigoProducto == productoVenta.CodigoProducto);
                if (producto != null)
                {
                    productosEnVenta.Add(new ProductoVentaPedidos()
                    {
                        Codigo = productoVenta.CodigoProducto,
                        Foto = productoVenta.Foto,
                        Nombre = producto.Nombre,
                        Descripcion = producto.Descripcion,
                        Precio = (double) productoVenta.Precio,
                        IdCategoria = (int) productoVenta.IdCategoriaProductoVenta
                    });
                }
            }

            return productosEnVenta;
        }
    }
}
