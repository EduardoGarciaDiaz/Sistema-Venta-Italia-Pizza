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
            AgregarBotonPedido();
            AgregarBotonPedidos();
            AgregarBotonProductos();
            AgregarBotonAgregarUsuario();
            AgregarBotonRecetas();
        }

        private void AgregarBotonPedido()
        {
            BtnMenuLateral pedidos = new BtnMenuLateral();
            pedidos.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_carrito_seleccionado.png", UriKind.Relative));
            pedidos.LblNombreBoton.Content = "Pedido";
            pedidos.Click += BtnPedidoClick;
            SkpMenuLateral.Children.Add(pedidos);
        }

        private void BtnPedidoClick(object sender, RoutedEventArgs e)
        {
            RegistroPedido registroPedido = new RegistroPedido();
            FrameNavigator.NavigationService.Navigate(registroPedido);
        }

        private void AgregarBotonProductos()
        {
            BtnMenuLateral producto = new BtnMenuLateral();
            producto.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_agregar_producto.png", UriKind.Relative));
            producto.LblNombreBoton.Content = "Productos";
            producto.Click += BtnProductoClick;
            SkpMenuLateral.Children.Add(producto);
        }

        private void BtnProductoClick(object sender, RoutedEventArgs e)
        {
            Productos producto = new Productos();
            FrameNavigator.NavigationService.Navigate(producto);
        }

        private void AgregarBotonAgregarUsuario()
        {
            BtnMenuLateral usuario = new BtnMenuLateral();
            usuario.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_agregar_usuario.png", UriKind.Relative));
            usuario.LblNombreBoton.Content = "Usuarios";
            usuario.Click += BtnUsuarioClick;
            SkpMenuLateral.Children.Add(usuario);
        }

        private void BtnUsuarioClick(object sender, RoutedEventArgs e)
        {
            RegistroUsuario registroUsuario = new RegistroUsuario();
            FrameNavigator.NavigationService.Navigate(registroUsuario);
        }

        private void AgregarBotonRecetas()
        {
            BtnMenuLateral receta = new BtnMenuLateral();
            receta.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_receta.png",
                UriKind.Relative));
            receta.LblNombreBoton.Content = "Recetas";
            receta.Click += BtnRecetaClick;
            SkpMenuLateral.Children.Add(receta);
        }

        private void BtnRecetaClick(object sender, RoutedEventArgs e)
        {
            Recetas recetas = new Recetas();
            FrameNavigator.NavigationService.Navigate(recetas);
        }

        private void AgregarBotonPedidos()
        {
            BtnMenuLateral pedidos = new BtnMenuLateral();
            pedidos.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_pedidos.png",
                UriKind.Relative));
            pedidos.LblNombreBoton.Content = "Pedidos";
            pedidos.Click += BtnPedidosClick;
            SkpMenuLateral.Children.Add(pedidos);
        }

        private void BtnPedidosClick(object sender, RoutedEventArgs e)
        {
            ConsultaPedidos consultaPedidos = new ConsultaPedidos();
            FrameNavigator.NavigationService.Navigate(consultaPedidos);
        }

        private void Salir_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
