using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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
    /// Lógica de interacción para Recetas.xaml
    /// </summary>
    public partial class Recetas : Page
    {
        private const int VENTANA_INFORMACION = 2;
        private const int VENTANA_CONFIRMACION = 3;
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
        }

        private void AgregarEventos()
        {
            barraDeBusquedaRecetas.tbxBusqueda_TextChanged += TbxBusqueda_TextChanged;
            barraDeBusquedaRecetas.imgBuscar_Click += ImgBuscar_Click;
            barraDeBusquedaRecetas.enter_Pressed += Enter_Pressed;
        }

        private void CargarRecetas()
        {
            RecuperarRecetas();
            bool existenRecetas = ValidarExistenciaRecetas();

            if (existenRecetas)
            {
                lbSinRecetas.Visibility = Visibility.Collapsed;
                MostrarRecetas();
            } 
            else
            {
                lbSinRecetas.Visibility = Visibility.Visible;
            }
        }

        private bool ValidarExistenciaRecetas()
        {
            int sinRecetas = 0;
            bool existenRecetas = false;

            if (_recetas != null && _recetas.Count() > sinRecetas)
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
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (TimeoutException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (CommunicationException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (Exception ex)
            {
                VentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
        }

        private void MostrarRecetas()
        {
            if (_recetas != null && _recetas.Count() > 0)
            {
                wrapPanelRecetas.Children.Clear();

                foreach (Receta receta in _recetas)
                {
                    MostrarReceta(receta);
                }
            }
        }

        private void MostrarReceta(Receta receta)
        {
            ElementoReceta elementoReceta = CrearElementoReceta(receta);

            wrapPanelRecetas.Children.Add(elementoReceta);
        }

        private ElementoReceta CrearElementoReceta(Receta receta)
        {
            ElementoReceta elementoReceta = new ElementoReceta();
            elementoReceta.lbNombreReceta.Content = receta.Nombre;
            elementoReceta.RecetaAsignada = receta;

            BitmapImage foto = ConvertidorBytes.ConvertirBytesABitmapImage(receta.FotoProducto);

            if (foto != null)
            {
                elementoReceta.imgFotoProductoReceta.Source = foto;
            }

            elementoReceta.gridReceta_Click += ElementoReceta_Click;
            elementoReceta.imgEditar_Click += ImgEditarReceta_Click;
            elementoReceta.imgEliminar_Click += ImgEliminar_Click;

            return elementoReceta;
        }

        private void ElementoReceta_Click(object sender, EventArgs e)
        {
            ElementoReceta recetaSeleccionada = sender as ElementoReceta;
            gridInsumosReceta.Visibility = Visibility.Visible;
            int idReceta = recetaSeleccionada.RecetaAsignada.Id;
            lbNombreReceta.Content = recetaSeleccionada.lbNombreReceta.Content;

            CargarInsumosReceta(idReceta);
        }

        private void CargarInsumosReceta(int idReceta)
        {
            InsumoReceta[] insumosReceta = RecuperarInsumosReceta(idReceta);
            MostrarInsumosReceta(insumosReceta);
        }

        private InsumoReceta[] RecuperarInsumosReceta(int idReceta)
        {
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();
            InsumoReceta[] insumosReceta = new InsumoReceta[0];

            if (idReceta > 0)
            {
                try
                {
                    insumosReceta = servicioRecetasCliente.RecuperarInsumosReceta(idReceta);
                }
                catch (EndpointNotFoundException ex)
                {
                    VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                    ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                }
                catch (TimeoutException ex)
                {
                    VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                    ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                }
                catch (FaultException<ExcepcionServidorItaliaPizza> ex)
                {
                    VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                    ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                }
                catch (FaultException ex)
                {
                    VentanasEmergentes.MostrarVentanaErrorServidor();
                    ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                }
                catch (CommunicationException ex)
                {
                    VentanasEmergentes.MostrarVentanaErrorServidor();
                    ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                }
                catch (Exception ex)
                {
                    VentanasEmergentes.MostrarVentanaErrorInesperado();
                    ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                }
            }

            return insumosReceta;
        }

        private void MostrarInsumosReceta(InsumoReceta[] insumosReceta)
        {
            stackPanelInsumos.Children.Clear();

            if (insumosReceta != null && insumosReceta.Count() > 0)
            {
                foreach (InsumoReceta insumo in insumosReceta)
                {
                    ElementoInsumoReceta elementoInsumoReceta = CrearElementoInsumoReceta(insumo);

                    stackPanelInsumos.Children.Add(elementoInsumoReceta);
                }
            }
        }

        private ElementoInsumoReceta CrearElementoInsumoReceta(InsumoReceta insumo)
        {
            string formatoCantidad = "F2";
            ElementoInsumoReceta elementoInsumoReceta = new ElementoInsumoReceta();

            double cantidad = insumo.Cantidad;
            elementoInsumoReceta.lbNombreInsumo.Content = insumo.Nombre;
            elementoInsumoReceta.lbCantidadInsumo.Content = cantidad.ToString(formatoCantidad);
            elementoInsumoReceta.lbUnidadMedidaInsumo.Content = insumo.UnidadMedida.Nombre;

            return elementoInsumoReceta;
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

        private void BuscarReceta()
        {
            if (barraDeBusquedaRecetas.tbxBusqueda.Text != string.Empty)
            {
                wrapPanelRecetas.Children.Clear();

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

        private void CerrarGridInsumosReceta()
        {
            stackPanelInsumos.Children.Clear();
            gridInsumosReceta.Visibility = Visibility.Collapsed;
        }

        private void BtnRegistrarReceta_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistroReceta());
        }

        private void ImgEliminar_Click(object sender, EventArgs e)
        {
            ElementoReceta elementoReceta = sender as ElementoReceta;
            MostrarVentanaEmergenteEliminación(elementoReceta);
        }

        private void MostrarVentanaEmergenteEliminación(ElementoReceta elementoRecetaAEliminar)
        {
            string nombreReceta = elementoRecetaAEliminar.lbNombreReceta.Content.ToString();

            string titulo = "Eliminar receta";
            string mensaje = $"¿Estás seguro de que deseas eliminar la receta para {nombreReceta}?";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(titulo, mensaje, "Sí", "No", Window.GetWindow(this), VENTANA_CONFIRMACION);
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

            if (filasAfectadas > 0)
            {
                wrapPanelRecetas.Children.Clear();
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

        private void ImgEditarReceta_Click(object sender, EventArgs e)
        {
            // TODO NavigationService.Navigate(new EdicionReceta());
            string titulo = "Funcionalidad próxima";
            string mensaje = "Esta funcionalidad se incluirá en un futuro";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(titulo, mensaje, Window.GetWindow(this), VENTANA_INFORMACION);
            ventanaEmergente.ShowDialog();
        }
    }
}
