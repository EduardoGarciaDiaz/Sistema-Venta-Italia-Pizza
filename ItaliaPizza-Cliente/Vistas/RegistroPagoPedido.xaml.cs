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

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para RegistroPagoPedido.xaml
    /// </summary>
    public partial class RegistroPagoPedido : Page
    {

        private static readonly Regex _regex = new Regex("[^0-9.]+");
        private readonly Pedido _pedido;
        private Cliente _cliente;

        public RegistroPagoPedido(Pedido pedido)
        {
            this._pedido = pedido;
            InitializeComponent();
            MostrarDatosDePedido(pedido);
            MostrarDatosDeCliente(pedido.IdCliente);
        }

        private void MostrarDatosDeCliente(int idCliente)
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            try
            {
                _cliente = new Cliente();
                _cliente = servicioUsuariosClient.RecuperarClientePorId(idCliente);
                if (_cliente != null)
                {
                    LblNombreCliente.Content = _cliente.NombreCliente;
                    LblCorreoElectronicoCliente.Content = _cliente.CorreoElectronicoCliente;
                    LblNumeroTelefonoCliente.Content = _cliente.NumeroTelefonoCliente;
                    LblDireccionCliente.Text = _cliente.DireccionCliente;
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void MostrarDatosDePedido(Pedido pedido)
        {
            LblTipoServicio.Content = pedido.TipoServicio.Nombre;
            MostrarProductosEnPedido(pedido);
            LblConteoProductos.Content = pedido.CantidadProductos + " productos.";
            LblSubtotal.Content = (pedido.Total / 1.16).ToString("F2");
            LblIva.Content = (pedido.Total - (pedido.Total / 1.16)).ToString("F2");
            LblTotal.Content = pedido.Total.ToString("F2");
        }

        private void MostrarProductosEnPedido(Pedido pedido)
        {
            foreach (ProductoVentaPedidos productoVenta in pedido.ProductosIncluidos.Keys)
            {
                ElementoTicketPedido elementoTicket = new ElementoTicketPedido();
                elementoTicket.LblCantidadProducto.Content = pedido.ProductosIncluidos[productoVenta];
                elementoTicket.LblNombreProducto.Content = productoVenta.Nombre;
                elementoTicket.LblTotalPorProducto.Content = "$" + pedido.ProductosIncluidos[productoVenta] * productoVenta.Precio;
                SkpContenedorElementosTicket.Children.Add(elementoTicket);
            }
        }

        private void TxtCantidadPagaCliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsTextoPermitido(e.Text + TxtCantidadPagaCliente.Text);
        }

        private bool EsTextoPermitido(string texto)
        {
            return !_regex.IsMatch(texto) && texto.Length < 6 && texto != "." && texto.Count(c => c == '.') < 2;
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
                    LblMensajeErrorPago.Content = "Faltan $" + (_pedido.Total - double.Parse(textBox.Text));
                } 
                else
                {
                    LblMensajeErrorPago.Content = "";
                }
            }
            else
            {
                LblMensajeErrorPago.Content = string.Empty;
            }
        }

        private void BtnConfirmarPago_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtCantidadPagaCliente.Text))
            {
                LblMensajeErrorPago.Content = "Por favor, ingresa la cantidad con la que el cliente paga";
            } 
            else
            {
                if (!(double.Parse(TxtCantidadPagaCliente.Text) < _pedido.Total))
                {
                    ServicioPedidosClient servicioPedidosClient = new ServicioPedidosClient();
                    int numeroPedido = servicioPedidosClient.GuardarPedido(_pedido);
                    if (numeroPedido > 0)
                    {
                        ConfirmacionRegistroPedido confirmacionRegistroPedido = new ConfirmacionRegistroPedido();
                        confirmacionRegistroPedido.LblNumeroPedido.Content = numeroPedido;
                        confirmacionRegistroPedido.LblNombreCliente.Content = _cliente.NombreCliente;
                        confirmacionRegistroPedido.LblCambio.Content = (double.Parse(TxtCantidadPagaCliente.Text) - _pedido.Total).ToString("F2");
                        confirmacionRegistroPedido.Click += BtnAceptarPagoClick;
                        confirmacionRegistroPedido.ShowDialog();
                    }
                }
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
            NavigationService.GoBack();
        }
    }
}
