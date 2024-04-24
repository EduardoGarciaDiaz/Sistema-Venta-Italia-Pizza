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
        List<ProveedorDto> listaProveedores = new List<ProveedorDto>();
        List<ElementoProveedor> listElementosProveedores = new List<ElementoProveedor>();


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
                listaProveedores = servicioProveedoresClient.RecuperarProveedores().ToList();
                InatanciarElementosProveedores(listaProveedores);
                MostrarProveedores(listElementosProveedores);
                barraBusquedaProveedor.Placeholder.Text = "Buscar por Nombre o RFC...";
                barraBusquedaProveedor.TxtBusqueda.Text = String.Empty;
                barraBusquedaProveedor.ImgBuscarClicked += BtnBuscar_Click;
            }
            catch (EndpointNotFoundException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (TimeoutException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (CommunicationException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (Exception ex)
            {
                VentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
           
           string criterio = barraBusquedaProveedor.TxtBusqueda.Text.Trim().ToLower();
            if (String.IsNullOrEmpty(criterio))
            {
                MostrarProveedores(listElementosProveedores);
            }
            else
            {
                var proveedoreFiltrados = FiltrarProveedores(criterio);
                MostrarProveedores(proveedoreFiltrados);
            }
        }

        private List<ElementoProveedor> FiltrarProveedores(string criterioBusqueda)
        {
            return listElementosProveedores.Where(proveedor => proveedor.lblNombre.Text.ToLower().Contains(criterioBusqueda) ||
                                                               proveedor.lblRFC.Text.ToLower().Contains(criterioBusqueda)).ToList();
        }

        private void InatanciarElementosProveedores(List<ProveedorDto> listaProveedores)
        {
            foreach (var proveedor in listaProveedores)
            {
                ElementoProveedor elementoProveedor = new ElementoProveedor(proveedor);
                elementoProveedor.btnCambiarEstadoProveedor_Click += BtnCambiarEstadoProveedor_Click;
                elementoProveedor.btnModificarProveedor_Click += BtnModificar_Click;
                listElementosProveedores.Add(elementoProveedor);
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


        private void BtnCambiarEstadoProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                ElementoProveedor elementoSeleccionado = sender as ElementoProveedor;
                bool esActivo = elementoSeleccionado.esActivo;
                int idProveedor = elementoSeleccionado.Id;
                bool exitoAccion = CambiarEstadoProveedor(esActivo, idProveedor);
                ActualizarEstadoEnPantalla(esActivo, exitoAccion, elementoSeleccionado);
            }
            catch (EndpointNotFoundException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (TimeoutException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (CommunicationException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (Exception ex)
            {
                VentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
        }

        private bool CambiarEstadoProveedor(bool esActivo, int idProveedor)
        {
            ServicioProveedoresClient servicioProveedoresClient = new ServicioProveedoresClient();
            return servicioProveedoresClient.CmabiarEstadoProveedor(esActivo, idProveedor);
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


        private void BtnModificar_Click(object sender, EventArgs e)
        {
            ElementoProveedor elementoSeleccionado = sender as ElementoProveedor;
            ProveedorDto proveedorSeleccionado = elementoSeleccionado.proveedorDto;
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

    }
}
