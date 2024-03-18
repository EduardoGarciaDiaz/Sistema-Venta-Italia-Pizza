using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioPedidos
    {
        public int GuardarPedido(Pedido pedido)
        {
            int numeroPedido = -1;
            PedidoDAO pedidoDAO = new PedidoDAO();
            ProductoDAO productoDAO = new ProductoDAO();
            InsumoDAO insumoDAO = new InsumoDAO();
            numeroPedido = pedidoDAO.GuardarPedido(pedido);

            if (numeroPedido > 0)
            {
                foreach (ProductoVentaPedidos productosVenta in pedido.ProductosIncluidos.Keys)
                {
                    bool insumoDisminuido;
                    if (productoDAO.ValidarSiProductoEnVentaEsInventariado(productosVenta.Codigo))
                    {
                        insumoDisminuido = insumoDAO.DisminuirCantidadInsumo(productosVenta.Codigo, pedido.ProductosIncluidos[productosVenta]);
                    }
                    else
                    {
                        insumoDisminuido = DisminuirCantidadInsumoPorProducto(productosVenta.Codigo, pedido.ProductosIncluidos[productosVenta]);
                    }
                    if (!insumoDisminuido)
                    {
                        break;
                    }
                }
            }

            return numeroPedido;
        }

        public List<TipoServicio> RecuperarTiposServicio()
        {
            List<TipoServicio> tipoServicios = new List<TipoServicio>();
            TipoServicioDAO tipoServicioDAO = new TipoServicioDAO();
            tipoServicios = tipoServicioDAO.RecuperarTiposServicio().ConvertAll(tipoServicio =>
                new TipoServicio
                {
                    Id = tipoServicio.IdTipoServicio,
                    Nombre = tipoServicio.Nombre
                }    
            );
            return tipoServicios;
        }
    }
}
