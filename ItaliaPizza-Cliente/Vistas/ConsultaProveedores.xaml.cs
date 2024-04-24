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
    /// Interaction logic for ConsultaProveedores.xaml
    /// </summary>
    public partial class ConsultaProveedores : Page
    {
        private List<ProveedorDto> _listaProveedores = new List<ProveedorDto>();
        private readonly List<ElementoProveedor> _listaElementosProveedores = new List<ElementoProveedor>();


        public ConsultaProveedores()
        {
            InitializeComponent();
            this.Loaded += ObtenerProveedores;
        }

        private void ObtenerProveedores(object sender, RoutedEventArgs e)
        {
            try
            {
                ServicioProveedoresClient servicioProveedoresClient = new ServicioProveedoresClient(); 
                _listaProveedores = servicioProveedoresClient.RecuperarProveedores().ToList();
                InatanciarElementosProveedores(_listaProveedores);
                MostrarProveedores(_listaElementosProveedores);
                barraBusquedaProveedor.plhrPista.Text = "Buscar por Nombre o RFC...";
                barraBusquedaProveedor.tbxBusqueda.Text = String.Empty;
                barraBusquedaProveedor.ImgBuscarClicked += BtnBuscar_Click;
            }
            catch (EndpointNotFoundException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (TimeoutException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (CommunicationException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (Exception ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
           
           string criterio = barraBusquedaProveedor.tbxBusqueda.Text.Trim().ToLower();
            if (String.IsNullOrEmpty(criterio))
            {
                MostrarProveedores(_listaElementosProveedores);
            }
            else
            {
                var proveedoreFiltrados = FiltrarProveedores(criterio);
                MostrarProveedores(proveedoreFiltrados);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            ElementoProveedor elementoSeleccionado = sender as ElementoProveedor;
            ProveedorDto proveedorSeleccionado = elementoSeleccionado.ProveedorDto;
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
            EdicionProveedor paginaEdicionProveedor = new EdicionProveedor(proveedorSeleccionado);
            ventanaPrincipal.FrameNavigator.Navigate(paginaEdicionProveedor);
        }

        private void BtnRegistrarProveedor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
            RegistroProveedor paginaRegistroProveedor = new RegistroProveedor(true);
            ventanaPrincipal.FrameNavigator.Navigate(paginaRegistroProveedor);
        }

        private void BtnCambiarEstadoProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                ElementoProveedor elementoSeleccionado = sender as ElementoProveedor;
                bool esActivo = elementoSeleccionado.EsActivo;
                int idProveedor = elementoSeleccionado.Id;
                bool exitoAccion = CambiarEstadoProveedor(esActivo, idProveedor);
                ActualizarEstadoEnPantalla(esActivo, exitoAccion, elementoSeleccionado);
            }
            catch (EndpointNotFoundException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (TimeoutException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (CommunicationException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (Exception ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
        }

        private List<ElementoProveedor> FiltrarProveedores(string criterioBusqueda)
        {
            return _listaElementosProveedores.Where(proveedor => proveedor.lblNombre.Text.ToLower().Contains(criterioBusqueda) ||
                                                               proveedor.lblRFC.Text.ToLower().Contains(criterioBusqueda)).ToList();
        }

        private void InatanciarElementosProveedores(List<ProveedorDto> listaProveedores)
        {
            foreach (var proveedor in listaProveedores)
            {
                ElementoProveedor elementoProveedor = new ElementoProveedor(proveedor);
                elementoProveedor.BtnCambiarEstadoProveedorClicked += BtnCambiarEstadoProveedor_Click;
                elementoProveedor.BtnModificarProveedorClicked += BtnModificar_Click;
                _listaElementosProveedores.Add(elementoProveedor);
;            }
        }

        private void MostrarProveedores(List<ElementoProveedor> listaElemetosProveedores)
        {
            wrpProveedoresInfromacion.Children.Clear();
            foreach (var item in listaElemetosProveedores)
            {
                wrpProveedoresInfromacion.Children.Add(item);
            }
        }               

        private bool CambiarEstadoProveedor(bool esActivo, int idProveedor)
        {
            ServicioProveedoresClient servicioProveedoresClient = new ServicioProveedoresClient();
            return servicioProveedoresClient.CambiarEstadoProveedor(esActivo, idProveedor);
        }

        private void ActualizarEstadoEnPantalla(bool estabaActivo, bool exitoAccion, ElementoProveedor elementoProveedor)
        {
            if (exitoAccion)
            {
                if (estabaActivo)
                {
                    elementoProveedor.CambiarEstado(false);
                    MostrarMensaje("Exito", "Se ha desactivado correctamente al proveedor", 2);
                }
                else
                {
                    elementoProveedor.CambiarEstado(true);
                    MostrarMensaje("Exito", "Se ha activado correctamente al proveedorr", 2 );
                }
            }
            else
            {
                MostrarMensaje("UPSS", "Ocurrio un error al desactivar al proveedor, intentelo mas tarde", 1);
            }            
        }

        private void MostrarMensaje(string titulo, string descripcion, int tipo)
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente(titulo, descripcion, Window.GetWindow(this), tipo);
            ventanaEmergente.ShowDialog();
        }       

    }
}
