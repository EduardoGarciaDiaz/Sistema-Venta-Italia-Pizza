using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using ItaliaPizza_Cliente.Vistas;
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
            this.Closing += MainWindow_Cerrando;
        }

        public void FiltrarOpcionesPanelLateral(int idTipoEmpleado)
        {
            switch (idTipoEmpleado)
            {
                case (int) EnumTiposEmpleado.Cajero:
                    OpcionesPanelCajero();
                    break;
                case (int)EnumTiposEmpleado.Mesero:
                    OpcionesPanelMesero();
                    break;
                case (int)EnumTiposEmpleado.Chef:
                    OpcionesPanelChef();
                    break;
                case (int)EnumTiposEmpleado.Inventarista:
                    OpcionesPanelInventarista();
                    break;
                default:
                    OpcionesPanelAdmin();
                    break;
            }
        }
        private void OpcionesPanelAdmin()
        {
            AgregarBotonPedido();
            AgregarBotonPedidos();            
            AgregarBotonProductos();
            AgregarBotonConsultaUsuarios();
            AgregarBotonRecetas();
            AgregarBotonConsultaOrdenesDeCompra();
        }

        private void OpcionesPanelCajero()
        {
            AgregarBotonPedido();
            AgregarBotonPedidos();
            AgregarBotonProductos();
            AgregarBotonRegistrarUsuarios();
            AgregarBotonRecetas();
            AgregarBotonConsultaOrdenesDeCompra();
        }

        private void OpcionesPanelChef()
        {
            AgregarBotonPedidos();
            AgregarBotonRecetas();
        }
        private void OpcionesPanelInventarista()
        {
            AgregarBotonProductos();
            AgregarBotonRecetas();
            AgregarBotonConsultaOrdenesDeCompra();
        }
        private void OpcionesPanelMesero()
        {
            AgregarBotonPedidos();
        }

        public void MostrarNombre(string nombreCompleto)
        {
            lblNombre.Content = nombreCompleto;
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

        private void AgregarBotonConsultaUsuarios()
        {
            BtnMenuLateral usuario = new BtnMenuLateral();
            usuario.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_agregar_usuario.png", UriKind.Relative));
            usuario.LblNombreBoton.Content = "Usuarios";
            usuario.Click += BtnUsuarioClick;
            SkpMenuLateral.Children.Add(usuario);
        }

        private void BtnUsuarioClick(object sender, RoutedEventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            FrameNavigator.NavigationService.Navigate(usuarios);
        }

        private void AgregarBotonRegistrarUsuarios()
        {
            BtnMenuLateral usuario = new BtnMenuLateral();
            usuario.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_agregar_usuario.png", UriKind.Relative));
            usuario.LblNombreBoton.Content = "Usuarios";
            usuario.Click += BtnRegistroUsuarioClick;
            SkpMenuLateral.Children.Add(usuario);
        }

        private void BtnRegistroUsuarioClick(object sender, RoutedEventArgs e)
        {
            RegistroUsuario registroUsuarios = new RegistroUsuario();
            FrameNavigator.NavigationService.Navigate(registroUsuarios);
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

        private void AgregarBotonConsultaOrdenesDeCompra()
        {
            BtnMenuLateral ordenes = new BtnMenuLateral();
            ordenes.ImgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_Orden_Compra.png",
                UriKind.Relative));
            ordenes.LblNombreBoton.Content = "Ordenes";
            ordenes.Click += BtnOrdenesCompraClick;
            SkpMenuLateral.Children.Add(ordenes);
        }

        private void BtnOrdenesCompraClick(object sender, RoutedEventArgs e)
        {
            ConsultaOrdenesDeCompra consultaOrdenesDeCompra = new ConsultaOrdenesDeCompra();
            FrameNavigator.NavigationService.Navigate(consultaOrdenesDeCompra);
        }



        private void MainWindow_Cerrando(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CerrarSesion();
        }

        private void Salir_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CerrarSesion();
            IrInicioSesion();
        }

        private void IrInicioSesion()
        {
            InicioSesion inicioSesion = new InicioSesion();
            FrameNavigator.NavigationService.Navigate(inicioSesion);
        }

        private void CerrarSesion()
        {
            try
            {
                SkpMenuLateral.Children.Clear();
                ServicioInicioSesionClient servicioInicioSesionClient = new ServicioInicioSesionClient();
                servicioInicioSesionClient.CerrarSesion(EmpleadoSingleton.getInstance().IdUsuario);
            }
            catch (EndpointNotFoundException)
            {
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
            }
            catch (TimeoutException)
            {
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
            }
            catch (FaultException<ExcepcionServidorItaliaPizza>)
            {
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
            }
            catch (FaultException)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
            }
            catch (CommunicationException)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
            }
            catch (Exception)
            {
                VentanasEmergentes.MostrarVentanaErrorInesperado();
            }
            EmpleadoSingleton.LimpiarSingleton();
        }


    } 

}
