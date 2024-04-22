using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO = "No existen pedidos para el cliente ingresado";
        private string MENSAJE_CAMPO_VACIO = "Por favor, ingresa algo en la barra para realizar la busqueda.";
        private string MENSAJE_SIN_RESULTADOS_SELECCION_FECHA = "No hay pedidos en la fecha seleccionada.";
        private string MARCAR_COMO_PREPARADO = "Marcar como preparado";
        private string CANCELAR_PEDIDO = "Cancelar pedido";
        private string MARCAR_COMO_ENTREGADO = "Marcar como entregado";

        private SolidColorBrush _colorBrushAmarillo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6B400"));
        private SolidColorBrush _colorBrushRojo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD67272"));
        private SolidColorBrush _colorBrushNegro = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00000000"));
        private SolidColorBrush _colorBrushGris = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF656565"));

        private List<PedidoConsultaDTO> _pedidos;
        private Pedido _pedidoSeleccionado;

        public ConsultaPedidos()
        {
            InitializeComponent();
            this.Loaded += ConsultaPedidos_Loaded;
        }

        private void ConsultaPedidos_Loaded(object sender, RoutedEventArgs e)
        {
            this.BqdClientes.Placeholder.Text = "Ingresa nombre de cliente...";
            this.BqdClientes.ImgBuscarClicked += ImgBuscarPedidoPorCliente;
            lbEnProceso.Tag = (int)EnumEstadosPedido.EnProceso;
            lbPreparados.Tag = (int)EnumEstadosPedido.Preparado;
            lbEntregados.Tag = (int)EnumEstadosPedido.Entregado;
            lbCancelados.Tag = (int)EnumEstadosPedido.Cancelado;
            MostrarFiltrosEstado();
            RecuperarPedidos();
        }

        private void BtnActualizarEstadoPedido_Click(object sender, RoutedEventArgs e)
        {
            if (_pedidoSeleccionado == null) return;

            var servicioPedidosClient = new ServicioPedidosClient();
            var empleadoSingleton = EmpleadoSingleton.getInstance();
            int nuevoEstadoPedido = DeterminarNuevoEstadoPedido(_pedidoSeleccionado.IdEstadoPedido, empleadoSingleton.DatosEmpleado.IdTipoEmpleado);

            if (nuevoEstadoPedido != -1)
            {
                int resultado = servicioPedidosClient.ActualizarEstadoPedido(_pedidoSeleccionado.NumeroPedido, nuevoEstadoPedido);

                if (resultado > 0)
                {
                    ActualizarUIPostCambioEstado(servicioPedidosClient);
                }
            }
        }

        private void ElementoConsultaPedidoClick(object sender, RoutedEventArgs e)
        {
            ElementoConsultaPedido elementoConsultaPedido = sender as ElementoConsultaPedido;
            ServicioPedidosClient servicioPedidosCliente = new ServicioPedidosClient();
            int numeroPedido = int.Parse(elementoConsultaPedido.LblNumeroPedido.Content.ToString());
            try
            {
                Pedido pedido = servicioPedidosCliente.RecuperarPedido(numeroPedido);
                if (pedido != null)
                {
                    _pedidoSeleccionado = pedido;
                    ServicioUsuariosClient servicioUsuarios = new ServicioUsuariosClient();
                    Cliente cliente = servicioUsuarios.RecuperarClientePorId(pedido.IdCliente);
                    bdrSeleccionaUnPedido.Visibility = Visibility.Collapsed;
                    MostrarPedido(pedido, cliente);
                }
            }
            catch (EndpointNotFoundException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (TimeoutException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (CommunicationException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (Exception ex)
            {
                VentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
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
                Utilidad.MostrarMensaje(LblMensajeAdvertenciaPedido, 2);
            }
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
            DateTime fechaSeleccionada = (DateTime)(sender as DatePicker).SelectedDate;
            List<PedidoConsultaDTO> resultadoSeleccionFecha = new List<PedidoConsultaDTO>();
            resultadoSeleccionFecha = _pedidos.Where(pedido => pedido.Fecha.Date == fechaSeleccionada.Date).ToList();
            if (resultadoSeleccionFecha.Any())
            {
                MostrarPedidos(resultadoSeleccionFecha);
                CambiarColorFiltroCategoria(lblTodosPedidos);
            }
            else
            {
                lblMensajeSinResultados.Content = MENSAJE_SIN_RESULTADOS_SELECCION_FECHA;
                Utilidad.MostrarMensaje(lblMensajeSinResultados, 2);
            }
        }

        private void LblTodosProductos_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CambiarColorFiltroCategoria((Label)sender);
            MostrarPedidos(_pedidos);
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var label = (Label)sender;
            int estadoPedidoId = Convert.ToInt32(label.Tag);

            CambiarColorFiltroCategoria(label);
            FiltrarYMostrarPedidosPorEstado(estadoPedidoId);
        }

        private void MostrarFiltrosEstado()
        {
            int idTipoEmpleado = EmpleadoSingleton.getInstance().DatosEmpleado.IdTipoEmpleado;
            switch (idTipoEmpleado)
            {
                case (int)EnumTiposEmpleado.Mesero:
                    
                    skpContenedorEstados.Children.Remove(lbEnProceso);
                    skpContenedorEstados.Children.Remove(lbEntregados);
                    RemoverFiltrosTodosYCancelados();
                    break;

                case (int)EnumTiposEmpleado.Chef:
                    skpContenedorEstados.Children.Remove(lbEntregados);
                    skpContenedorEstados.Children.Remove(lbPreparados);
                    RemoverFiltrosTodosYCancelados();
                    break;

                default:
                    break;
            }
        }

        private void RemoverFiltrosTodosYCancelados()
        {
            skpContenedorEstados.Children.Remove(lbCancelados);
            skpContenedorEstados.Children.Remove(lblTodosPedidos);
        }

        private void RecuperarPedidos()
        {
            ServicioPedidosClient servicioPedidosCliente = new ServicioPedidosClient();
            int idTipoEmpleado = EmpleadoSingleton.getInstance().DatosEmpleado.IdTipoEmpleado;
            try
            {
                if (idTipoEmpleado == (int)EnumTiposEmpleado.Cajero)
                {
                    _pedidos = servicioPedidosCliente.RecuperarPedidos().ToList();
                }
                else if (idTipoEmpleado == (int)EnumTiposEmpleado.Chef)
                {
                    _pedidos = servicioPedidosCliente.RecuperarPedidosEnProceso().ToList();
                }
                else
                {
                    _pedidos = servicioPedidosCliente.RecuperarPedidosPreparados().ToList();
                }
                MostrarPedidos(_pedidos);
            }
            catch (EndpointNotFoundException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (TimeoutException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (CommunicationException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (Exception ex)
            {
                VentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
        }

        private void MostrarPedidos(List<PedidoConsultaDTO> pedidos)
        {
            SkpContenedorPedidos.Children.Clear();
            pedidos?.ForEach(pedido =>
            {
                var elementoConsultaPedido = new ElementoConsultaPedido
                {
                    LblNumeroPedido = { Content = pedido.NumeroPedido },
                    LblNombreCliente = { Content = pedido.NombreCliente },
                    LblCantidadProductos = { Content = $"{pedido.CantidadProductos} productos." },
                    LblFecha = { Content = pedido.Fecha.ToShortDateString() },
                    LblTotalPedido = { Content = $"${pedido.Total:F2}" },
                    LblEstadoPedido = { Content = pedido.estadoPedido.Nombre }
                };
                CambiarColorLabelEstado(pedido.estadoPedido.IdEstadoPedido, elementoConsultaPedido.LblEstadoPedido);
                elementoConsultaPedido.Click += ElementoConsultaPedidoClick;

                SkpContenedorPedidos.Children.Add(elementoConsultaPedido);
            });
        }

        private void CambiarColorLabelEstado(int idEstadoPedido, Label lbEstadoPedido)
        {
            switch (idEstadoPedido)
            {
                case (int)EnumEstadosPedido.EnProceso:
                    lbEstadoPedido.Background = _colorBrushAmarillo;
                    lbEstadoPedido.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case (int)EnumEstadosPedido.Preparado:
                    lbEstadoPedido.Background = new SolidColorBrush(Colors.White);
                    lbEstadoPedido.BorderThickness = new Thickness(2);
                    lbEstadoPedido.BorderBrush = new SolidColorBrush(Colors.Black);
                    lbEstadoPedido.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case (int)EnumEstadosPedido.Entregado:
                    lbEstadoPedido.Background = _colorBrushNegro;
                    lbEstadoPedido.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case (int)EnumEstadosPedido.Cancelado:
                    lbEstadoPedido.Background = _colorBrushRojo;
                    lbEstadoPedido.Foreground = new SolidColorBrush(Colors.White);
                    break;

                default:
                    break;
            }
        }

        private void MostrarPedido(Pedido pedido, Cliente cliente)
        {
            if (pedido != null && cliente != null)
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
                        lbNombreProducto = { Content = producto.Nombre },
                        lbDescripcionProducto = { Content = producto.Descripcion },
                        lbCantidadProductos = { Content = pedido.ProductosIncluidos[producto] },
                        lbPrecioProducto = { Content = "$" + producto.Precio },
                    };
                    skpContenedorProductos.Children.Add(consultaPedido);
                }
                lbSubtotal.Content = (pedido.Total / 1.16).ToString("F2");
                lbIVA.Content = (pedido.Total - (pedido.Total / 1.16)).ToString("F2");
                lbTotal.Content = pedido.Total.ToString("F2");
                MostrarContenidoDeBoton(pedido.IdEstadoPedido);
            }
        }

        private void MostrarContenidoDeBoton(int idEstadoPedido)
        {
            EmpleadoSingleton empleadoSingleton = EmpleadoSingleton.getInstance();
            int tipoEmpleado = empleadoSingleton.DatosEmpleado.IdTipoEmpleado;

            switch (idEstadoPedido)
            {
                case (int)EnumEstadosPedido.EnProceso when tipoEmpleado == (int)EnumTiposEmpleado.Chef:
                    ConfigurarBoton(_colorBrushAmarillo, MARCAR_COMO_PREPARADO, new SolidColorBrush(Colors.White), new Thickness(0));
                    break;

                case (int)EnumEstadosPedido.EnProceso when tipoEmpleado == (int)EnumTiposEmpleado.Cajero:
                    ConfigurarBoton(new SolidColorBrush(Colors.White), CANCELAR_PEDIDO, _colorBrushRojo, new Thickness(2), true);
                    break;

                case (int)EnumEstadosPedido.Preparado:
                    ConfigurarBoton(new SolidColorBrush(Colors.Black), MARCAR_COMO_ENTREGADO, new SolidColorBrush(Colors.White), new Thickness(0));
                    break;

                case (int)EnumEstadosPedido.Cancelado:
                case (int)EnumEstadosPedido.Entregado:
                    btnActualizarEstadoPedido.Visibility = Visibility.Collapsed;
                    return;

                default:
                    break;
            }
            btnActualizarEstadoPedido.Visibility = Visibility.Visible;
        }

        private void ConfigurarBoton(SolidColorBrush background, string content, SolidColorBrush foreground, Thickness borderThickness, bool customBorderBrush = false)
        {
            btnActualizarEstadoPedido.Background = background;
            btnActualizarEstadoPedido.Content = content;
            btnActualizarEstadoPedido.Foreground = foreground;
            btnActualizarEstadoPedido.BorderThickness = borderThickness;
            if (customBorderBrush)
            {
                btnActualizarEstadoPedido.BorderBrush = _colorBrushRojo;
            }
        }

        private bool ValidarCamposVacios(string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        private void CambiarColorFiltroCategoria(Label labelSeleccionado)
        {
            foreach (Label label in skpContenedorEstados.Children.OfType<Label>().ToList())
            {
                label.Foreground = _colorBrushGris;
            }
            labelSeleccionado.Foreground = _colorBrushAmarillo;
        }

        private void FiltrarYMostrarPedidosPorEstado(int idEstadoPedido)
        {
            List<PedidoConsultaDTO> productosFiltrados = _pedidos.Where(p =>
                p.estadoPedido.IdEstadoPedido == idEstadoPedido).ToList();

            if (productosFiltrados.Any())
            {
                MostrarPedidos(productosFiltrados);
            }
            else
            {
                SkpContenedorPedidos.Children.Clear();
            }
        }

        private int DeterminarNuevoEstadoPedido(int idEstadoActual, int idTipoEmpleado)
        {
            if (idEstadoActual == (int)EnumEstadosPedido.EnProceso && idTipoEmpleado == (int)EnumTiposEmpleado.Chef)
            {
                return (int)EnumEstadosPedido.Preparado;
            }
            if (idEstadoActual == (int)EnumEstadosPedido.EnProceso && idTipoEmpleado == (int)EnumTiposEmpleado.Cajero)
            {
                return (int)EnumEstadosPedido.Cancelado;
            }
            if (idEstadoActual == (int)EnumEstadosPedido.Preparado)
            {
                return (int)EnumEstadosPedido.Entregado;
            }
            return -1;
        }

        private void ActualizarUIPostCambioEstado(ServicioPedidosClient servicioPedidosClient)
        {
            
            try
            {
                if (EmpleadoSingleton.getInstance().DatosEmpleado.IdTipoEmpleado == (int)EnumTiposEmpleado.Cajero)
                {
                    _pedidoSeleccionado = servicioPedidosClient.RecuperarPedido(_pedidoSeleccionado.NumeroPedido);
                    Cliente cliente = new ServicioUsuariosClient().RecuperarClientePorId(_pedidoSeleccionado.IdCliente);
                    MostrarPedido(_pedidoSeleccionado, cliente);
                } else
                {
                    bdrSeleccionaUnPedido.Visibility = Visibility.Visible;
                }
                RecuperarPedidos();
            }
            catch (EndpointNotFoundException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (TimeoutException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (CommunicationException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (Exception ex)
            {
                VentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
        }

    }
}
