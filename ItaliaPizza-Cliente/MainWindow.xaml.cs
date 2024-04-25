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

        private void BtnPedidoClick(object sender, RoutedEventArgs e)
        {
            RegistroPedido registroPedido = new RegistroPedido();
            FrameNavigator.NavigationService.Navigate(registroPedido);
        }

        private void BtnProductoClick(object sender, RoutedEventArgs e)
        {
            Productos producto = new Productos();
            FrameNavigator.NavigationService.Navigate(producto);
        }

        private void BtnUsuarioClick(object sender, RoutedEventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            FrameNavigator.NavigationService.Navigate(usuarios);
        }

        private void BtnRegistroUsuarioClick(object sender, RoutedEventArgs e)
        {
            RegistroUsuario registroUsuarios = new RegistroUsuario();
            FrameNavigator.NavigationService.Navigate(registroUsuarios);
        }

        private void BtnRecetaClick(object sender, RoutedEventArgs e)
        {
            Recetas recetas = new Recetas();
            FrameNavigator.NavigationService.Navigate(recetas);
        }

        private void BtnPedidosClick(object sender, RoutedEventArgs e)
        {
            ConsultaPedidos consultaPedidos = new ConsultaPedidos();
            FrameNavigator.NavigationService.Navigate(consultaPedidos);
        }

        private void BtnOrdenesCompraClick(object sender, RoutedEventArgs e)
        {
            ConsultaOrdenesDeCompra consultaOrdenesDeCompra = new ConsultaOrdenesDeCompra();
            FrameNavigator.NavigationService.Navigate(consultaOrdenesDeCompra);
        }

        private void BtnProveedores_Click(object sender, RoutedEventArgs e)
        {
            ConsultaProveedores paginaConsultaProveedores = new ConsultaProveedores();
            FrameNavigator.NavigationService.Navigate(paginaConsultaProveedores);
        }

        private void BtnReporteProductos_Click(object sender, RoutedEventArgs e)
        {
            ReporteProductos paginaReporteProductos = new ReporteProductos(FrameNavigator);
            paginaReporteProductos.ShowDialog();
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



        private void GastosVarios_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string nombre = lblNombre.Content.ToString();

            if (!string.IsNullOrEmpty(nombre))
            {
                GastosVarios gastosGenerales = new GastosVarios(this);
                gastosGenerales.ShowDialog();
            }
        }

        private void CorteCaja_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (EmpleadoSingleton.getInstance().DatosEmpleado.IdTipoEmpleado == (int)EnumTiposEmpleado.Cajero)
            {
                RegistroCorteCaja registroCorteCaja = new RegistroCorteCaja(this);
                registroCorteCaja.ShowDialog();
            }
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
        public void MostrarNombre(string nombreCompleto)
        {
            lblNombre.Content = nombreCompleto;
        }

        public void MostrarBotonAgregarGastosVarios()
        {
            grdGastos.Visibility = Visibility.Visible;
        }

        private void OpcionesPanelAdmin()
        {         
            AgregarBotonConsultaUsuarios();
        }

        private void OpcionesPanelCajero()
        {
            AgregarBotonPedido();
            AgregarBotonPedidos();
            AgregarBotonCorte();
        }

        private void AgregarBotonCorte()
        {
            brdCorteCaja.Visibility = Visibility.Visible;
            lblTituloCorte.Visibility = Visibility.Visible;
            imgCorteCaja.Visibility= Visibility.Visible;
        }

        private void OpcionesPanelChef()
        {
            AgregarBotonPedidos();
            AgregarBotonRecetas();
        }

        private void OpcionesPanelInventarista()
        {
            AgregarBotonProductos();
            AgregarBotonReporteProductos();
            AgregarBotonConsultaOrdenesDeCompra();
            AgregarBotonProveedores();
        }
        private void OpcionesPanelMesero()
        {
            AgregarBotonPedidos();
        }

        private void AgregarBotonPedido()
        {
            BtnMenuLateral pedidos = new BtnMenuLateral();
            pedidos.imgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_carrito_seleccionado.png", UriKind.Relative));
            pedidos.lblNombreBoton.Content = "Pedido";
            pedidos.BtnMenuLateralClicked += BtnPedidoClick;
            SkpMenuLateral.Children.Add(pedidos);
        }        

        private void AgregarBotonProductos()
        {
            BtnMenuLateral producto = new BtnMenuLateral();
            producto.imgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_agregar_producto.png", UriKind.Relative));
            producto.lblNombreBoton.Content = "Productos";
            producto.BtnMenuLateralClicked += BtnProductoClick;
            SkpMenuLateral.Children.Add(producto);
        }       

        private void AgregarBotonConsultaUsuarios()
        {
            BtnMenuLateral usuario = new BtnMenuLateral();
            usuario.imgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_agregar_usuario.png", UriKind.Relative));
            usuario.lblNombreBoton.Content = "Usuarios";
            usuario.BtnMenuLateralClicked += BtnUsuarioClick;
            SkpMenuLateral.Children.Add(usuario);
        }         

        private void AgregarBotonRecetas()
        {
            BtnMenuLateral receta = new BtnMenuLateral();
            receta.imgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_receta.png",
                UriKind.Relative));
            receta.lblNombreBoton.Content = "Recetas";
            receta.BtnMenuLateralClicked += BtnRecetaClick;
            SkpMenuLateral.Children.Add(receta);
        }       

        private void AgregarBotonPedidos()
        {
            BtnMenuLateral pedidos = new BtnMenuLateral();
            pedidos.imgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_pedidos.png",
                UriKind.Relative));
            pedidos.lblNombreBoton.Content = "Pedidos";
            pedidos.BtnMenuLateralClicked += BtnPedidosClick;
            SkpMenuLateral.Children.Add(pedidos);
        }

        private void AgregarBotonConsultaOrdenesDeCompra()
        {
            BtnMenuLateral ordenes = new BtnMenuLateral();
            ordenes.imgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_Orden_Compra.png",
                UriKind.Relative));
            ordenes.lblNombreBoton.Content = "Ordenes";
            ordenes.BtnMenuLateralClicked += BtnOrdenesCompraClick;
            SkpMenuLateral.Children.Add(ordenes);
        }       

        private void AgregarBotonProveedores()
        {
            BtnMenuLateral proveedor = new BtnMenuLateral();
            proveedor.imgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_proveedor.png",UriKind.Relative));
            proveedor.lblNombreBoton.Content = "Proveedores";
            proveedor.BtnMenuLateralClicked += BtnProveedores_Click;
            SkpMenuLateral.Children.Add(proveedor);
        }    

        private void AgregarBotonReporteProductos()
        {
            BtnMenuLateral reprorteProductos = new BtnMenuLateral();
            reprorteProductos.imgIconoBoton.Source = new BitmapImage(new Uri("/Recursos/Iconos/icono_proveedor.png", UriKind.Relative));
            reprorteProductos.lblNombreBoton.Content = "Reportes";
            reprorteProductos.BtnMenuLateralClicked += BtnReporteProductos_Click;
            SkpMenuLateral.Children.Add(reprorteProductos);
        }      

        private void IrInicioSesion()
        {
            InicioSesion inicioSesion = new InicioSesion();
            this.Close();
            inicioSesion.Show();
        }

        private void CerrarSesion()
        {
            try
            {
                SkpMenuLateral.Children.Clear();
                ServicioInicioSesionClient servicioInicioSesionClient = new ServicioInicioSesionClient();
                servicioInicioSesionClient.CerrarSesion(EmpleadoSingleton.getInstance().IdUsuario);
                lblNombre.Content = String.Empty;
                lblTituloCorte.Visibility = Visibility.Hidden;
                imgCorteCaja.Visibility = Visibility.Hidden;
                brdCorteCaja.Visibility = Visibility.Hidden;
            }
            catch (EndpointNotFoundException)
            {
            }
            catch (TimeoutException)
            {
            }
            catch (FaultException<ExcepcionServidorItaliaPizza>)
            {
            }
            catch (FaultException)
            {
            }
            catch (CommunicationException)
            {
            }
            catch (Exception)
            {
            }
            EmpleadoSingleton.LimpiarSingleton();
        }

       
    } 

}
