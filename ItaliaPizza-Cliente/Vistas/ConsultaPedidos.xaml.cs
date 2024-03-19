using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para ConsultaPedidos.xaml
    /// </summary>
    public partial class ConsultaPedidos : Page
    {
        private List<PedidoConsultaDTO> _pedidos;
        SolidColorBrush _colorBrushAmarillo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6B400"));
        SolidColorBrush _colorBrushRojo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD67272"));
        SolidColorBrush _colorBrushNegro = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00000000"));
        SolidColorBrush _colorBrushGris = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF656565"));

        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO = "No existen pedidos para el cliente ingresado";
        private string MENSAJE_CAMPO_VACIO = "Por favor, ingresa algo en la barra para realizar la busqueda.";
        private string MENSAJE_SIN_RESULTADOS_SELECCION_FECHA = "No hay pedidos en la fecha seleccionada.";

        private Pedido _pedidoSeleccionado;

        public ConsultaPedidos()
        {
            InitializeComponent();
            this.BqdClientes.Placeholder.Text = "Ingresa nombre de cliente...";
            this.BqdClientes.ImgBuscarClicked += ImgBuscarPedidoPorCliente;

            ServicioPedidosClient servicioPedidosClient = new ServicioPedidosClient();
            _pedidos = servicioPedidosClient.RecuperarPedidos().ToList();
            MostrarPedidos(_pedidos);
        }

        private void MostrarPedidos(List<PedidoConsultaDTO> pedidos)
        {
            SkpContenedorPedidos.Children.Clear();
            if (pedidos != null)
            {
                foreach (var pedido in pedidos)
                {
                    ElementoConsultaPedido elementoConsultaPedido = new ElementoConsultaPedido();
                    elementoConsultaPedido.LblNumeroPedido.Content = pedido.NumeroPedido;
                    elementoConsultaPedido.LblNombreCliente.Content = pedido.NombreCliente;
                    elementoConsultaPedido.LblCantidadProductos.Content = pedido.CantidadProductos + " productos.";
                    elementoConsultaPedido.LblFecha.Content = pedido.Fecha.ToShortDateString();
                    elementoConsultaPedido.LblTotalPedido.Content = "$" + pedido.Total.ToString("F2");
                    elementoConsultaPedido.LblEstadoPedido.Content = pedido.estadoPedido.Nombre;
                    CambiarColorLabelEstado(pedido.estadoPedido.IdEstadoPedido, elementoConsultaPedido.LblEstadoPedido);
                    elementoConsultaPedido.Click += ElementoConsultaPedidoClick;

                    SkpContenedorPedidos.Children.Add(elementoConsultaPedido);
                }
            }
        }

        private void CambiarColorLabelEstado(int idEstadoPedido, Label lbEstadoPedido)
        {
            if (idEstadoPedido == (int)EnumEstadosPedido.EnProceso)
            {
                lbEstadoPedido.Background = _colorBrushAmarillo;
                lbEstadoPedido.Foreground = new SolidColorBrush(Colors.White);
            }
            if (idEstadoPedido == (int)EnumEstadosPedido.Preparado)
            {
                lbEstadoPedido.Background = new SolidColorBrush(Colors.White);
                lbEstadoPedido.BorderThickness = new Thickness(2);
                lbEstadoPedido.BorderBrush = new SolidColorBrush(Colors.Black); ;
                lbEstadoPedido.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (idEstadoPedido == (int)EnumEstadosPedido.Entregado)
            {
                lbEstadoPedido.Background = _colorBrushNegro;
                lbEstadoPedido.Foreground = new SolidColorBrush(Colors.White);
            }
            if (idEstadoPedido == (int) EnumEstadosPedido.Cancelado)
            {
                lbEstadoPedido.Background = _colorBrushRojo;
                lbEstadoPedido.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void ElementoConsultaPedidoClick(object sender, RoutedEventArgs e)
        {
            ElementoConsultaPedido elementoConsultaPedido = sender as ElementoConsultaPedido;
            ServicioPedidosClient servicioPedidosClient = new ServicioPedidosClient();
            int numeroPedido = int.Parse(elementoConsultaPedido.LblNumeroPedido.Content.ToString());
            Pedido pedido = servicioPedidosClient.RecuperarPedido(numeroPedido);
            if (pedido != null)
            {
                _pedidoSeleccionado = pedido;
                ServicioUsuariosClient servicioUsuarios = new ServicioUsuariosClient();
                Cliente cliente = servicioUsuarios.RecuperarClientePorId(pedido.IdCliente);
                bdrSeleccionaUnPedido.Visibility = Visibility.Collapsed;
                MostrarPedido(pedido, cliente);
            }
        }

        private void MostrarPedido(Pedido pedido, Cliente cliente)
        {
            skpContenedorProductos.Children.Clear();
            lbNumeroPedido.Content = "#" + pedido.NumeroPedido;
            lbNombreCliente.Content = cliente.NombreCliente;
            lbCorreoElectronicoCliente.Content = cliente.CorreoElectronicoCliente;
            lbTipoServicio.Content = pedido.TipoServicio.Nombre;

            foreach (var producto in pedido.ProductosIncluidos.Keys)
            {
                ElementoProductoConsultaPedido consultaPedido = new ElementoProductoConsultaPedido
                {
                    lbNombreProducto = {Content = producto.Nombre},
                    lbDescripcionProducto = { Content =  producto.Descripcion},
                    lbCantidadProductos = {Content = pedido.ProductosIncluidos[producto]},
                    lbPrecioProducto = {Content = "$" + producto.Precio},
                };
                skpContenedorProductos.Children.Add(consultaPedido);
            }
            lbSubtotal.Content = (pedido.Total / 1.16).ToString("F2");
            lbIVA.Content = (pedido.Total - (pedido.Total / 1.16)).ToString("F2");
            lbTotal.Content = pedido.Total.ToString("F2");
            MostrarContenidoDeBoton(pedido.IdEstadoPedido);
        }

        private void MostrarContenidoDeBoton(int idEstadoPedido)
        {
            EmpleadoSingleton empleadoSingleton = EmpleadoSingleton.getInstance();
            empleadoSingleton.TipoEmpleado = new TipoEmpleadoDto
            {
                IdTipoEmpleado = 3,
                Nombre = "X"
            };
            if (idEstadoPedido == (int)EnumEstadosPedido.EnProceso 
                && empleadoSingleton.TipoEmpleado.IdTipoEmpleado == (int) EnumTiposEmpleado.Chef)
            {
                btnActualizarEstadoPedido.Background = _colorBrushAmarillo;
                btnActualizarEstadoPedido.Content = "Marcar como preparado";
                btnActualizarEstadoPedido.Foreground = new SolidColorBrush(Colors.White);
                btnActualizarEstadoPedido.BorderThickness = new Thickness(0);
            }
            if (idEstadoPedido == (int)EnumEstadosPedido.EnProceso
                && empleadoSingleton.TipoEmpleado.IdTipoEmpleado == (int) EnumTiposEmpleado.Cajero)
            {
                btnActualizarEstadoPedido.Background = new SolidColorBrush(Colors.White);
                btnActualizarEstadoPedido.BorderBrush = _colorBrushRojo;
                btnActualizarEstadoPedido.Foreground = _colorBrushRojo;
                btnActualizarEstadoPedido.BorderThickness = new Thickness(2);
                btnActualizarEstadoPedido.Content = "Cancelar pedido";
            }
            if (idEstadoPedido == (int)EnumEstadosPedido.Preparado)
            {
                btnActualizarEstadoPedido.Background = new SolidColorBrush(Colors.Black);
                btnActualizarEstadoPedido.Foreground = new SolidColorBrush(Colors.White);
                btnActualizarEstadoPedido.BorderThickness = new Thickness(0);
                btnActualizarEstadoPedido.Content = "Marcar como entregado";
            }
            btnActualizarEstadoPedido.Visibility = Visibility.Visible;
            if (idEstadoPedido == (int)EnumEstadosPedido.Cancelado || idEstadoPedido == (int)EnumEstadosPedido.Entregado)
            {
                btnActualizarEstadoPedido.Visibility = Visibility.Collapsed;
            }
        }

        private void ImgBuscarPedidoPorCliente(object sender, EventArgs e)
        {
            string valorBusqueda = BqdClientes.TxtBusqueda.Text.ToString();
            if (!(ValidarCamposVacios(valorBusqueda)))
            {
                List<PedidoConsultaDTO> resultadoBusquedaPedidos = new List<PedidoConsultaDTO>();
                resultadoBusquedaPedidos = _pedidos.Where(
                    pedido => pedido.NombreCliente.ToLower().Contains(valorBusqueda.ToLower())).ToList();
                if (resultadoBusquedaPedidos.Count != 0)
                {
                    MostrarPedidos(resultadoBusquedaPedidos);
                }
                else
                {
                    LblMensajeAdvertenciaPedido.Content = MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO;
                }
            }
            else
            {
                LblMensajeAdvertenciaPedido.Content = MENSAJE_CAMPO_VACIO;
                Utilidad.MostrarLabelDuranteSegundos(LblMensajeAdvertenciaPedido, 2);
            }
        }

        private bool ValidarCamposVacios(string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        private void TxtBusquedaPedidoChanged(object sender, EventArgs e)
        {
            this.LblMensajeAdvertenciaPedido.Content = "";
            if (string.IsNullOrWhiteSpace(BqdClientes.TxtBusqueda.Text))
            {
                MostrarPedidos(_pedidos);
            }
        }

        private void DpkFechaBusqueda_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime fechaSeleccionada = (DateTime) (sender as DatePicker).SelectedDate;
            List<PedidoConsultaDTO> resultadoSeleccionFecha = new List<PedidoConsultaDTO>();
            resultadoSeleccionFecha = _pedidos.Where(pedido => pedido.Fecha.Date == fechaSeleccionada.Date).ToList();
            if (resultadoSeleccionFecha.Count != 0)
            {
                MostrarPedidos(resultadoSeleccionFecha);
                CambiarColorFiltroCategoria(lblTodosPedidos);
            }
            else
            {
                lblMensajeSinResultados.Content = MENSAJE_SIN_RESULTADOS_SELECCION_FECHA;
                Utilidad.MostrarLabelDuranteSegundos(lblMensajeSinResultados, 2);
            }
        }

        private void LblTodosProductos_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CambiarColorFiltroCategoria((Label)sender);
            MostrarPedidos(_pedidos);
        }

        private void CambiarColorFiltroCategoria(Label labelSeleccionado)
        {
            foreach (Label label in skpContenedorEstados.Children.OfType<Label>().ToList())
            {
                label.Foreground = _colorBrushGris;
            }
            labelSeleccionado.Foreground = _colorBrushAmarillo;
        }

        private void LbEnProceso_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CambiarColorFiltroCategoria((Label)sender);
            List<PedidoConsultaDTO> productosFiltrados = _pedidos.Where(p => 
                p.estadoPedido.IdEstadoPedido == (int)EnumEstadosPedido.EnProceso).ToList();
            if (productosFiltrados.Count > 0)
            {
                MostrarPedidos(productosFiltrados);
            }
            else
            {
                SkpContenedorPedidos.Children.Clear();
            }
        }

        private void LbPreparado_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CambiarColorFiltroCategoria((Label)sender);
            List<PedidoConsultaDTO> productosFiltrados = _pedidos.Where(p =>
                p.estadoPedido.IdEstadoPedido == (int)EnumEstadosPedido.Preparado).ToList();
            if (productosFiltrados.Count > 0)
            {
                MostrarPedidos(productosFiltrados);
            }
            else
            {
                SkpContenedorPedidos.Children.Clear();
            }
        }

        private void LbEntregado_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CambiarColorFiltroCategoria((Label)sender);
            List<PedidoConsultaDTO> productosFiltrados = _pedidos.Where(p =>
                p.estadoPedido.IdEstadoPedido == (int)EnumEstadosPedido.Entregado).ToList();
            if (productosFiltrados.Count > 0)
            {
                MostrarPedidos(productosFiltrados);
            }
            else
            {
                SkpContenedorPedidos.Children.Clear();
            }
        }

        private void LbCancelado_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CambiarColorFiltroCategoria((Label)sender);
            List<PedidoConsultaDTO> productosFiltrados = _pedidos.Where(p =>
                p.estadoPedido.IdEstadoPedido == (int)EnumEstadosPedido.Cancelado).ToList();
            if (productosFiltrados.Count > 0)
            {
                MostrarPedidos(productosFiltrados);
            }
            else
            {
                SkpContenedorPedidos.Children.Clear();
            }
        }

        private void BtnActualizarEstadoPedido_Click(object sender, RoutedEventArgs e)
        {
            if (_pedidoSeleccionado != null)
            {
                ServicioPedidosClient servicioPedidosClient = new ServicioPedidosClient();
                int idEstadoPedido = _pedidoSeleccionado.IdEstadoPedido;
                int resultado = -1;
                EmpleadoSingleton empleadoSingleton = EmpleadoSingleton.getInstance();
                empleadoSingleton.TipoEmpleado = new TipoEmpleadoDto
                {
                    IdTipoEmpleado = 3,
                    Nombre = "x"
                };
                if (idEstadoPedido == (int)EnumEstadosPedido.EnProceso
                && empleadoSingleton.TipoEmpleado.IdTipoEmpleado == (int)EnumTiposEmpleado.Chef)
                {
                    resultado =
                        servicioPedidosClient.ActualizarEstadoPedido(_pedidoSeleccionado.NumeroPedido, (int)EnumEstadosPedido.Preparado);
                }
                if (idEstadoPedido == (int)EnumEstadosPedido.EnProceso
                    && empleadoSingleton.TipoEmpleado.IdTipoEmpleado == (int)EnumTiposEmpleado.Cajero)
                {
                    resultado =
                         servicioPedidosClient.ActualizarEstadoPedido(_pedidoSeleccionado.NumeroPedido, (int)EnumEstadosPedido.Cancelado);
                }
                if (idEstadoPedido == (int)EnumEstadosPedido.Preparado)
                {
                    resultado =
                        servicioPedidosClient.ActualizarEstadoPedido(_pedidoSeleccionado.NumeroPedido, (int)EnumEstadosPedido.Entregado);
                }
                if (resultado >  0)
                {
                    _pedidos = servicioPedidosClient.RecuperarPedidos().ToList();
                    _pedidoSeleccionado = servicioPedidosClient.RecuperarPedido(_pedidoSeleccionado.NumeroPedido);
                    Cliente cliente = new ServicioUsuariosClient().RecuperarClientePorId(_pedidoSeleccionado.IdCliente);
                    MostrarPedidos(_pedidos);
                    MostrarPedido(_pedidoSeleccionado, cliente);
                }
            }            
        }
    }
}
