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
    /// Lógica de interacción para InicioSesion.xaml
    /// </summary>
    public partial class InicioSesion : Page
    {
        public InicioSesion()
        {
            InitializeComponent();
            this.Loaded += LoadedPage;
        }

        private void LoadedPage(object sender, RoutedEventArgs e)
        {
            MainWindow ventana = (MainWindow)Window.GetWindow(this);
            ventana.SkpMenuLateral.Visibility = Visibility.Hidden;
            ventana.SkpMenuLateral.Children.Clear();
        }

        private void BtnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            string nombreUsuario = txbNombreUsuario.Text.Trim();
            string contrasena = txbContrasena.Password.ToString().Trim();
            if(ValidarCamposVacios(nombreUsuario, contrasena))
            {
                try
                {
                    ValidarCredenciales(nombreUsuario, contrasena);
                }
                catch (EndpointNotFoundException )
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
                catch (Exception )
                {
                    VentanasEmergentes.MostrarVentanaErrorInesperado();
                }
            }
        }

        private bool ValidarCamposVacios(String nombreUsuario, string contrasena)
        {
            bool camposlLenos = true;
            if (String.IsNullOrEmpty(nombreUsuario))
            {
                lblErrrNombreUsuario.Content = "* Campo obligatorio requerido";
                camposlLenos = false;
            }
            else
            {
                lblErrrNombreUsuario.Content = String.Empty;
            }
            if (String.IsNullOrEmpty(contrasena))
            {
                lblErrorCOntrasena.Content = "* Campo obligatorio requerido";
                camposlLenos = false;
            }
            else
            {
                lblErrorCOntrasena.Content = String.Empty;
            }
            return camposlLenos;

        }

        private void ValidarCredenciales(String nombreUsuario, string contrasena)
        {
            ServicioInicioSesionClient servicioInicioSesionClient = new ServicioInicioSesionClient();
            int usuarioExiste = servicioInicioSesionClient.ValidarCredenciales(nombreUsuario, contrasena);
            if(usuarioExiste == 0)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Upps!!", "Credenciales incorrectas, ingrese una credenciales validas para iniciar sesion", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }
            else if(usuarioExiste == -1)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Upps!!", "Parece ser que has inciado sesion previamente, cierra tu sesion activa para volver a ingresar", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }
            else if(usuarioExiste == 2){
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Upps!!", "Parece ser que tu cuenta esta desactivada, verificalo con tu administrador", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }
            else
            {
                ObtenerDatosEmpleado(nombreUsuario);
                EmpleadoSingleton empleado = EmpleadoSingleton.getInstance();
                if (empleado.DatosEmpleado != null && empleado.DatosDireccion != null && empleado.DatosUsuario!= null)
                {
                    NavegarVentanaInicio();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Upps!!", "Hubo un problema al recuperar tu información intentalo de nuevo", Window.GetWindow(this), 1);
                    ventanaEmergente.ShowDialog();
                }
            }
        }

        private void ObtenerDatosEmpleado(string nombreUsuario)
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            EmpleadoDto datosEmpleado = servicioUsuariosClient.RecuperarEmpleadoPorNombreUsuario(nombreUsuario);
            EmpleadoSingleton.getInstance(datosEmpleado, datosEmpleado.Usuario, datosEmpleado.Usuario.Direccion);   
            
        }

        private void NavegarVentanaInicio()
        {
            MainWindow ventana = (MainWindow)Window.GetWindow(this);
            PaginaDeIncio paginaDeIncio = new PaginaDeIncio();
            EmpleadoSingleton empleado = EmpleadoSingleton.getInstance();
            ventana.MostrarNombre(empleado.NombreUsuario);
            ventana.FiltrarOpcionesPanelLateral(empleado.DatosEmpleado.IdTipoEmpleado);
            ventana.SkpMenuLateral.Visibility = Visibility.Visible;
            ventana.FrameNavigator.NavigationService.Navigate(paginaDeIncio);
            ventana.MostrarBotonAgregarGastosVarios();
        }


    }
}