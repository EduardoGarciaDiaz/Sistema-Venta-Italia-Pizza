using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess.Excepciones;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class PedidoDAO
    {
        public PedidoDAO() { }


        public bool ValidarClienteTienePedidosPendientes(int idUsuario)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.UsuariosPedidos.Any(pedido => pedido.IdCliente == idUsuario &&
                                                              context.Pedidos.FirstOrDefault(ped => ped.NumeroPedido == pedido.NumeroPedido).IdEstadoPedido ==
                                                              context.EstadosPedido.FirstOrDefault(estado => estado.Nombre.Equals("En proceso")).IdEstadoPedido);
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return resultadoOperacion;
        }

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
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }

            return pedidoNuevoId;
        }

        public List<PedidoConsultaDTO> RecuperarPedidosParaConsulta()
        {
            List<PedidoConsultaDTO> pedidos = new List<PedidoConsultaDTO>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    pedidos = context.Pedidos.ToList().ConvertAll(p =>
                    new PedidoConsultaDTO
                    {
                        NumeroPedido = p.NumeroPedido,
                        Fecha = (DateTime) p.FechaPedido,
                        estadoPedido = new EstadoPedido()
                        {
                            IdEstadoPedido = p.EstadosPedido.IdEstadoPedido,
                            Nombre = p.EstadosPedido.Nombre
                        },
                        CantidadProductos = (int) p.CantidadProductos,
                        Total = (double) p.TotalParaPagar,
                        NombreCliente = p.UsuariosPedidos.FirstOrDefault().Usuarios.NombreCompleto
                    });
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }

            return pedidos;
        }

        public List<PedidoConsultaDTO> RecuperarPedidosPorEstado(int estado)
        {
            List<PedidoConsultaDTO> pedidos = new List<PedidoConsultaDTO>();

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    pedidos = context.Pedidos.Where(pedido => pedido.IdEstadoPedido == estado).ToList().ConvertAll(p =>
                    new PedidoConsultaDTO
                    {
                        NumeroPedido = p.NumeroPedido,
                        Fecha = (DateTime)p.FechaPedido,
                        estadoPedido = new EstadoPedido()
                        {
                            IdEstadoPedido = p.EstadosPedido.IdEstadoPedido,
                            Nombre = p.EstadosPedido.Nombre
                        },
                        CantidadProductos = (int)p.CantidadProductos,
                        Total = (double)p.TotalParaPagar,
                        NombreCliente = p.UsuariosPedidos.FirstOrDefault().Usuarios.NombreCompleto
                    });
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }

            return pedidos;
        }

        public Pedido RecuperarPedido(int numeroPedido)
        {
            Pedido pedido = new Pedido();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    Pedidos pedidoConsulta = context.Pedidos.FirstOrDefault(p => p.NumeroPedido == numeroPedido);
                    if (pedidoConsulta != null)
                    {

                        pedido = new Pedido()
                        {
                            NumeroPedido = pedidoConsulta.NumeroPedido,
                            Fecha = (DateTime)pedidoConsulta.FechaPedido,
                            CantidadProductos = (int)pedidoConsulta.CantidadProductos,
                            Total = (double)pedidoConsulta.TotalParaPagar,
                            IdEstadoPedido = (int)pedidoConsulta.IdEstadoPedido,
                            TipoServicio = new TipoServicio()
                            {
                                Id = pedidoConsulta.TiposServicio.IdTipoServicio,
                                Nombre = pedidoConsulta.TiposServicio.Nombre
                            },
                            IdCliente = (int)pedidoConsulta.UsuariosPedidos.FirstOrDefault().IdCliente,
                            ProductosIncluidos = new Dictionary<ProductoVentaPedidos, int>(
                                pedidoConsulta.PedidosProductosVenta.ToDictionary(
                                        p => new ProductoVentaPedidos
                                        {
                                            Codigo = p.CodigoProducto,
                                            Descripcion = p.ProductosVenta.Productos.Descripcion,
                                            Precio = (double)p.ProductosVenta.Precio,
                                            IdCategoria = p.ProductosVenta.CategoriasProductoVenta.IdCategoriaProductoVenta,
                                            Nombre = p.ProductosVenta.Productos.Nombre
                                        },
                                        p => p.CantidadProducto ?? 0
                                    )
                                )
                        };
                    }
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return pedido;
        }

        public int ActualizarEstadoPedido(int numeroPedido, int idEstado)
        {
            int resultado = -1;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.Pedidos.FirstOrDefault(p => p.NumeroPedido == numeroPedido).IdEstadoPedido = idEstado;
                    resultado = context.SaveChanges();
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return resultado;
        }

        public List<EstadoPedido> RecuperarEstadosPedido()
        {
            List<EstadoPedido> estados = new List<EstadoPedido>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    estados = context.EstadosPedido.ToList().ConvertAll(ep => new EstadoPedido
                    {
                        Nombre = ep.Nombre,
                        IdEstadoPedido = ep.IdEstadoPedido
                    });
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return estados;
        }
    }
}
