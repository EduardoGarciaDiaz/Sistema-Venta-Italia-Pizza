using ItaliaPizza_Contratos.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class PedidoDAO
    {
        public PedidoDAO() { }

        public int GuardarPedido(Pedido pedido)
        {
            int pedidoNuevoId = 0;
            try
            {
                using (var contex = new ItaliaPizzaEntities())
                {
                    Pedidos pedidos = new Pedidos()
                    {
                        FechaPedido = pedido.Fecha,
                        CantidadProductos = pedido.CantidadProductos,
                        TotalParaPagar = pedido.Total,
                        IdEstadoPedido = pedido.IdEstadoPedido,
                        IdTipoServicio = pedido.TipoServicio.Id,
                    };
                    contex.Pedidos.Add(pedidos);
                    contex.SaveChanges();
                    pedidoNuevoId = pedidos.NumeroPedido;
                    foreach (ProductoVentaPedidos producto in pedido.ProductosIncluidos.Keys)
                    {
                        PedidosProductosVenta pedidosProductosVenta = new PedidosProductosVenta()
                        {
                            NumeroPedido = pedidos.NumeroPedido,
                            CodigoProducto = producto.Codigo,
                            CantidadProducto = pedido.ProductosIncluidos[producto]
                        };
                        pedidos.PedidosProductosVenta.Add(pedidosProductosVenta);
                        contex.SaveChanges();
                    }
                    UsuariosPedidos usuariosPedidos = new UsuariosPedidos()
                    {
                        IdCliente = pedido.IdCliente,
                        NumeroPedido = pedidos.NumeroPedido,
                        NombreUsuario = pedido.NombreUsuarioCajero
                    };
                    pedidos.UsuariosPedidos.Add(usuariosPedidos);
                    contex.SaveChanges();
                }
            }
            catch (EntityException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
                pedidoNuevoId = -1;
            }
            catch (SqlException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
                pedidoNuevoId = -1;
            }
            catch (Exception ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
                pedidoNuevoId = -1;
            }

            return pedidoNuevoId;
        }
    }
}
