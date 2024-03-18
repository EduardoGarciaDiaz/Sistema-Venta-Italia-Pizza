using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.Vistas;
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

namespace ItaliaPizza_Cliente
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AgregarBotonPedidos();
            AgregarBotonAgregarProducto();
            AgregarBotonAgregarUsuario();
        }

        private void AgregarBotonPedidos()
        {
            BtnMenuLateral pedidos = new BtnMenuLateral();
            pedidos.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_carrito_seleccionado.png", UriKind.Relative));
            pedidos.LblNombreBoton.Content = "Pedido";
            pedidos.Click += BtnPedidosClick;
            SkpMenuLateral.Children.Add(pedidos);
        }

        private void BtnPedidosClick(object sender, RoutedEventArgs e)
        {
            RegistroPedido registroPedido = new RegistroPedido();
            FrameNavigator.NavigationService.Navigate(registroPedido);
        }

        private void AgregarBotonAgregarProducto()
        {
            BtnMenuLateral producto = new BtnMenuLateral();
            producto.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_agregar_producto.png", UriKind.Relative));
            producto.LblNombreBoton.Content = "Producto";
            producto.Click += BtnProductoClick;
            SkpMenuLateral.Children.Add(producto);
        }

        private void BtnProductoClick(object sender, RoutedEventArgs e)
        {
            RegistroProducto registroProducto = new RegistroProducto();
            FrameNavigator.NavigationService.Navigate(registroProducto);
        }

        private void AgregarBotonAgregarUsuario()
        {
            BtnMenuLateral usuario = new BtnMenuLateral();
            usuario.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_agregar_usuario.png", UriKind.Relative));
            usuario.LblNombreBoton.Content = "Usuario";
            usuario.Click += BtnUsuarioClick;
            SkpMenuLateral.Children.Add(usuario);
        }

        private void BtnUsuarioClick(object sender, RoutedEventArgs e)
        {
            RegistroUsuario registroUsuario = new RegistroUsuario();
            FrameNavigator.NavigationService.Navigate(registroUsuario);
        }

        private void Salir_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
