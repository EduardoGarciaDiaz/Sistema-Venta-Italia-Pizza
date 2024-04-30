using System;
using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using ItaliaPizza_Cliente.Utilidades;
using System.ServiceModel;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para RegistroPagoPedido.xaml
    /// </summary>
    public partial class RegistroPagoPedido : Page
    {

        private static readonly Regex REGEX = new Regex("[^0-9.]+");
        private readonly Pedido _pedido;
        private Cliente _cliente;
        private RegistroPedido _registroPedido;
        private bool _pagoRegistrado = false;

        public RegistroPagoPedido(Pedido pedido, RegistroPedido registroPedido)
        {
            this._pedido = pedido;
            this._registroPedido = registroPedido;
            this.Unloaded += RegistroDePagoPedido_Unloaded;
            InitializeComponent();
            MostrarPedido(pedido);
            MostrarCliente(pedido.IdCliente);
        }

        private void RegistroDePagoPedido_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!_pagoRegistrado)
            {
                _registroPedido.DesapartarTodosLosProductosEnPedido();
            }
        }

        private void TxtCantidadPagaCliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsTextoPermitido(e.Text + tbxCantidadPagaCliente.Text);
        }

        private void ImgRegresar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegistroPedido());
        }

        private void TxtCantidadPagaCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                if (double.Parse(textBox.Text) < _pedido.Total)
                {
                    lblMensajeErrorPago.Content = "Faltan $" + (_pedido.Total - double.Parse(textBox.Text));
                }
                else
                {
                    lblMensajeErrorPago.Content = "";
                }
            }
            else
            {
                lblMensajeErrorPago.Content = string.Empty;
            }
        }

        private void BtnConfirmarPago_Click(object sender, RoutedEventArgs e)
        {
            bool camposValidos = ValidarCampos();
            if (!camposValidos)
            {
                lblMensajeErrorPago.Content = "Por favor, ingresa la cantidad con la que el cliente paga";
                return;
            }

            if (!double.TryParse(tbxCantidadPagaCliente.Text, out double cantidadPagada) || cantidadPagada < _pedido.Total)
            {
                return;
            }

            ServicioPedidosClient servicioPedidosCliente = new ServicioPedidosClient();
            try
            {
                int numeroPedido = servicioPedidosCliente.GuardarPedido(_pedido);
                if (numeroPedido > 0)
                {
                    _pagoRegistrado = true;
                    MostrarConfirmacionPedido(numeroPedido);
                }
            }
            catch (EndpointNotFoundException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (TimeoutException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (CommunicationException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (Exception ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
        }

        private void BtnAceptarPagoClick(object sender, EventArgs e)
        {
            Window confirmacion = sender as Window;
            confirmacion.Close();
            RegistroPedido registroPedido = new RegistroPedido();
            NavigationService.Navigate(registroPedido);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _registroPedido.DesapartarTodosLosProductosEnPedido();
            NavigationService.Navigate(new RegistroPedido());
        }

        private bool ValidarCampos()
        {
            return string.IsNullOrWhiteSpace(tbxCantidadPagaCliente.Text);
        }

        private void MostrarConfirmacionPedido(int numeroPedido)
        {
            ConfirmacionRegistroPedido confirmacionRegistroPedido = new ConfirmacionRegistroPedido();
            confirmacionRegistroPedido.lblNumeroPedido.Content = numeroPedido;
            confirmacionRegistroPedido.lblNombreCliente.Content = _cliente.NombreCliente;
            confirmacionRegistroPedido.lblCambio.Content = (double.Parse(tbxCantidadPagaCliente.Text) - _pedido.Total).ToString("F2");
            confirmacionRegistroPedido.Click += BtnAceptarPagoClick;
            confirmacionRegistroPedido.ShowDialog();
        }

        private void MostrarCliente(int idCliente)
        {
            ServicioUsuariosClient servicioUsuariosCliente = new ServicioUsuariosClient();
            try
            {
                _cliente = new Cliente();
                _cliente = servicioUsuariosCliente.RecuperarClientePorId(idCliente);
                if (_cliente != null)
                {
                    lblNombreCliente.Content = _cliente.NombreCliente;
                    lblCorreoElectronicoCliente.Content = _cliente.CorreoElectronicoCliente;
                    lblNumeroTelefonoCliente.Content = _cliente.NumeroTelefonoCliente;
                    lblDireccionCliente.Text = _cliente.DireccionCliente;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (TimeoutException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (CommunicationException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (Exception ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
        }

        private void MostrarPedido(Pedido pedido)
        {
            if (pedido != null)
            {
                lblTipoServicio.Content = pedido.TipoServicio.Nombre;
                MostrarProductosEnPedido(pedido);
                lblConteoProductos.Content = pedido.CantidadProductos + " productos.";
                lblSubtotal.Content = (pedido.Total / 1.16).ToString("F2");
                lblIva.Content = (pedido.Total - (pedido.Total / 1.16)).ToString("F2");
                lblTotal.Content = pedido.Total.ToString("F2");
            }
        }

        private void MostrarProductosEnPedido(Pedido pedido)
        {
            foreach (ProductoVentaPedidos productoVenta in pedido.ProductosIncluidos.Keys)
            {
                ElementoTicketPedido elementoTicket = new ElementoTicketPedido();
                elementoTicket.lblCantidadProducto.Content = pedido.ProductosIncluidos[productoVenta];
                elementoTicket.lblNombreProducto.Content = productoVenta.Nombre;
                elementoTicket.lblTotalPorProducto.Content = "$" + pedido.ProductosIncluidos[productoVenta] * productoVenta.Precio;
                SkpContenedorElementosTicket.Children.Add(elementoTicket);
            }
        }

        private bool EsTextoPermitido(string texto)
        {
            return !REGEX.IsMatch(texto) && texto.Length < 6 && texto != "." && texto.Count(c => c == '.') < 2;
        }

    }
}
