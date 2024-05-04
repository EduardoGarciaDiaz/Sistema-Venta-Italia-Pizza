using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para Recetas.xaml
    /// </summary>
    public partial class Recetas : Page
    {
        private const int VENTANA_INFORMACION = 2;
        private const int VENTANA_CONFIRMACION = 3;
        private const int SIN_RECETAS = 0;
        private Receta[] _recetas;

        public Recetas()
        {
            InitializeComponent();

            this.Loaded += Recetas_Loaded;
        }

        private void Recetas_Loaded(object sender, RoutedEventArgs e)
        {
            CargarRecetas();
            AgregarEventos();
            CerrarGridInsumosReceta();
        }

        private void BtnRegistrarReceta_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistroReceta());
        }

        private void ImgBuscar_Click(object sender, EventArgs e)
        {
            BuscarReceta();
        }

        private void Enter_Pressed(object sender, EventArgs e)
        {
            if (e is KeyEventArgs keyEventArgs && keyEventArgs.Key == Key.Enter)
            {
                BuscarReceta();
            }
        }

        private void TbxBusqueda_TextChanged(object sender, EventArgs e)
        {
            if (barraDeBusquedaRecetas.tbxBusqueda.Text.Trim() == string.Empty)
            {
                MostrarRecetas();
            }
        }

        private void ImgCerrarInsumos_Click(object sender, RoutedEventArgs e)
        {
            CerrarGridInsumosReceta();
        }

        private void ElementoReceta_Click(object sender, EventArgs e)
        {
            ElementoReceta recetaSeleccionada = sender as ElementoReceta;
            grdInsumosReceta.Visibility = Visibility.Visible;
            int idReceta = recetaSeleccionada.RecetaAsignada.Id;
            lblNombreReceta.Content = recetaSeleccionada.lblNombreReceta.Content;

            CargarInsumosReceta(idReceta);
        }

        private void ImgEliminar_Click(object sender, EventArgs e)
        {
            ElementoReceta elementoReceta = sender as ElementoReceta;
            MostrarVentanaEmergenteEliminación(elementoReceta);
        }

        private void ImgEditarReceta_Click(object sender, EventArgs e)
        {
            Receta receta = (sender as ElementoReceta).RecetaAsignada;

            EdicionReceta edicionReceta = new EdicionReceta(receta);
            NavigationService.Navigate(edicionReceta);
        }

        private void AgregarEventos()
        {
            barraDeBusquedaRecetas.TbxBusquedaTextChanged += TbxBusqueda_TextChanged;
            barraDeBusquedaRecetas.ImgBuscarClicked += ImgBuscar_Click;
            barraDeBusquedaRecetas.EnterPressed += Enter_Pressed;
        }

        private void CargarRecetas()
        {
            RecuperarRecetas();
            bool existenRecetas = ValidarExistenciaRecetas();

            if (existenRecetas)
            {
                lblSinRecetas.Visibility = Visibility.Collapsed;
                MostrarRecetas();
            } 
            else
            {
                lblSinRecetas.Visibility = Visibility.Visible;
            }
        }

        private bool ValidarExistenciaRecetas()
        {
            bool existenRecetas = false;

            if (_recetas != null && _recetas.Count() > SIN_RECETAS)
            {
                existenRecetas = true;
            }

            return existenRecetas;
        }

        private void RecuperarRecetas()
        {
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();

            try
            {
                _recetas = servicioRecetasCliente.RecuperarRecetas();
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

        private void MostrarRecetas()
        {
            if (_recetas != null && _recetas.Count() > SIN_RECETAS)
            {
                wrpRecetas.Children.Clear();

                foreach (Receta receta in _recetas)
                {
                    MostrarReceta(receta);
                }
            }
        }

        private void MostrarReceta(Receta receta)
        {
            ElementoReceta elementoReceta = CrearElementoReceta(receta);

            wrpRecetas.Children.Add(elementoReceta);
        }

        private ElementoReceta CrearElementoReceta(Receta receta)
        {
            ElementoReceta elementoReceta = new ElementoReceta();
            elementoReceta.lblNombreReceta.Content = receta.Nombre;
            elementoReceta.RecetaAsignada = receta;

            BitmapImage foto = ConvertidorBytes.ConvertirBytesABitmapImage(receta.FotoProducto);

            if (foto != null)
            {
                elementoReceta.imgFotoProductoReceta.Source = foto;
            }

            elementoReceta.GrdRecetaClicked += ElementoReceta_Click;
            elementoReceta.ImgEditarClicked += ImgEditarReceta_Click;
            elementoReceta.ImgEliminarClicked += ImgEliminar_Click;

            return elementoReceta;
        }

        private void CargarInsumosReceta(int idReceta)
        {
            InsumoReceta[] insumosReceta = RecuperarInsumosReceta(idReceta);
            MostrarInsumosReceta(insumosReceta);
        }

        private InsumoReceta[] RecuperarInsumosReceta(int idReceta)
        {
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();
            InsumoReceta[] insumosReceta = new InsumoReceta[SIN_RECETAS];

            if (idReceta > SIN_RECETAS)
            {
                try
                {
                    insumosReceta = servicioRecetasCliente.RecuperarInsumosReceta(idReceta);
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

            return insumosReceta;
        }

        private void MostrarInsumosReceta(InsumoReceta[] insumosReceta)
        {
            skpInsumos.Children.Clear();

            if (insumosReceta != null && insumosReceta.Count() > SIN_RECETAS)
            {
                foreach (InsumoReceta insumo in insumosReceta)
                {
                    ElementoInsumoReceta elementoInsumoReceta = CrearElementoInsumoReceta(insumo);

                    skpInsumos.Children.Add(elementoInsumoReceta);
                }
            }
        }

        private ElementoInsumoReceta CrearElementoInsumoReceta(InsumoReceta insumo)
        {
            string formatoCantidad = "F2";
            ElementoInsumoReceta elementoInsumoReceta = new ElementoInsumoReceta();

            double cantidad = insumo.Cantidad;
            elementoInsumoReceta.lblNombreInsumo.Content = insumo.Nombre;
            elementoInsumoReceta.lblCantidadInsumo.Content = cantidad.ToString(formatoCantidad);
            elementoInsumoReceta.lblUnidadMedidaInsumo.Content = insumo.UnidadMedida.Nombre;

            return elementoInsumoReceta;
        }

        private void BuscarReceta()
        {
            if (barraDeBusquedaRecetas.tbxBusqueda.Text != string.Empty)
            {
                wrpRecetas.Children.Clear();

                string textoABuscar = barraDeBusquedaRecetas.tbxBusqueda.Text.Trim().ToUpper();
                MostrarCoincidencias(textoABuscar);
            }
        }

        private void MostrarCoincidencias(string textoABuscar)
        {
            foreach (Receta receta in _recetas)
            {
                if (receta.Nombre.ToUpper().Contains(textoABuscar))
                {
                    MostrarReceta(receta);
                }
            }
        }

        private void CerrarGridInsumosReceta()
        {
            skpInsumos.Children.Clear();
            grdInsumosReceta.Visibility = Visibility.Collapsed;
        }

        private void MostrarVentanaEmergenteEliminación(ElementoReceta elementoRecetaAEliminar)
        {
            string nombreReceta = elementoRecetaAEliminar.lblNombreReceta.Content.ToString();
            string titulo = "Eliminar receta";
            string mensaje = $"¿Estás seguro de que deseas eliminar la receta para {nombreReceta}?";
            string contenidoBtnAceptar = "Sí";
            string contenidoBtnCancelar = "No";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(titulo, mensaje, contenidoBtnAceptar, contenidoBtnCancelar, Window.GetWindow(this), VENTANA_CONFIRMACION);
            ventanaEmergente.ShowDialog();

            if (ventanaEmergente.AceptarAccion)
            {
                EliminarReceta(elementoRecetaAEliminar.RecetaAsignada.Id);
            }
        }

        private void EliminarReceta(int idReceta)
        {
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();
            int filasAfectadas = servicioRecetasCliente.EliminarReceta(idReceta);
            int SinFilasAfectadas = 0;

            if (filasAfectadas > SinFilasAfectadas)
            {
                wrpRecetas.Children.Clear();
                CargarRecetas();
                CerrarGridInsumosReceta();
            } 
            else
            {
                string titulo = "No se pudo eliminar";
                string mensaje = "Ocurrió un error al eliminar la receta y no se pudo eliminar, inténtalo de nuevo";

                VentanaEmergente ventanaEmergente = new VentanaEmergente(titulo, mensaje, Window.GetWindow(this), VENTANA_INFORMACION);
                ventanaEmergente.ShowDialog();
            }
        }
    }
}
