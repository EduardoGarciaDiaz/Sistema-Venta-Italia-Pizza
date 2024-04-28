using ItaliaPizza_Contratos.DTOs;
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
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioPedidos
    {
        public int GuardarPedido(Pedido pedido)
        {
            int numeroPedido;
            PedidoDAO pedidoDAO = new PedidoDAO();
            ProductoDAO productoDAO = new ProductoDAO();
            InsumoDAO insumoDAO = new InsumoDAO();
            try
            {
                numeroPedido = pedidoDAO.GuardarPedido(pedido);

                if (numeroPedido > 0)
                {
                    foreach (ProductoVentaPedidos productosVenta in pedido.ProductosIncluidos.Keys)
                    {
                        DesapartarInsumosDeProducto(productosVenta.Codigo, pedido.ProductosIncluidos[productosVenta]);
                        if (productoDAO.ValidarSiProductoEnVentaEsInventariado(productosVenta.Codigo))
                        {
                            insumoDAO.DisminuirCantidadInsumo(productosVenta.Codigo, pedido.ProductosIncluidos[productosVenta]);
                        }
                        else
                        {
                            DisminuirCantidadInsumoPorProducto(productosVenta.Codigo, pedido.ProductosIncluidos[productosVenta]);
                        }
                    }
                }
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return numeroPedido;
        }

        public Pedido RecuperarPedido(int numeroPedido)
        {
            PedidoDAO pedidoDAO = new PedidoDAO();
            Pedido pedido = new Pedido();
            try
            {
                pedido = pedidoDAO.RecuperarPedido(numeroPedido);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return pedido;
        }

        public List<PedidoConsultaDTO> RecuperarPedidos()
        {
            PedidoDAO pedidoDAO = new PedidoDAO();
            try
            {
                return pedidoDAO.RecuperarPedidosParaConsulta();
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public List<TipoServicio> RecuperarTiposServicio()
        {
            List<TipoServicio> tipoServicios = new List<TipoServicio>();
            TipoServicioDAO tipoServicioDAO = new TipoServicioDAO();
            try
            {
                tipoServicios = tipoServicioDAO.RecuperarTiposServicio().ConvertAll(tipoServicio =>
                    new TipoServicio
                    {
                        Id = tipoServicio.IdTipoServicio,
                        Nombre = tipoServicio.Nombre
                    }
);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return tipoServicios;
        }

        public int ActualizarEstadoPedido(int numeroPedido, int idEstadoPedido)
        {
            PedidoDAO pedidoDAO = new PedidoDAO();
            try
            {
                return pedidoDAO.ActualizarEstadoPedido(numeroPedido, idEstadoPedido);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public List<PedidoConsultaDTO> RecuperarPedidosEnProceso()
        {
            PedidoDAO pedidoDAO = new PedidoDAO();
            List<PedidoConsultaDTO> pedidos = new List<PedidoConsultaDTO>();
            try
            {
                EstadoPedido estado = pedidoDAO.RecuperarEstadosPedido().FirstOrDefault(e => 
                e.Nombre.ToLower().Contains("en proceso"));
                pedidos = pedidoDAO.RecuperarPedidosPorEstado(estado.IdEstadoPedido);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return pedidos;
        }

        public List<PedidoConsultaDTO> RecuperarPedidosPreparados()
        {
            try
            {
                PedidoDAO pedidoDAO = new PedidoDAO();
                List<PedidoConsultaDTO> pedidos = new List<PedidoConsultaDTO>();
                EstadoPedido estado = pedidoDAO.RecuperarEstadosPedido().FirstOrDefault(e =>
                e.Nombre.ToLower().Contains("preparado"));
                pedidos = pedidoDAO.RecuperarPedidosPorEstado(estado.IdEstadoPedido);
                return pedidos;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public double RecuperarIngresosDePedidosPorFecha(DateTime fecha)
        {
            PedidoDAO pedidoDAO = new PedidoDAO();
            try
            {
                return pedidoDAO.RecuperarIngresosDePedidosPorFecha(fecha);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }
    }
}
