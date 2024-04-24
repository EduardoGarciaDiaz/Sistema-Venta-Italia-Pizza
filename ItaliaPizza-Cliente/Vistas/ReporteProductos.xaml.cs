using Aspose.Pdf;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Interaction logic for ReporteProductos.xaml
    /// </summary>
    public partial class ReporteProductos : Window
    {
        private List<Categoria> _categoriasInsumos = new List<Categoria>();
        private List<Categoria> _categoriasProductosVenta = new List<Categoria>();
        private readonly Window _mainWindow;
        private Frame _frameNavegator;


        public ReporteProductos(Frame frameNavigator)
        {
            InitializeComponent();
            this.Loaded += PrepararVentana;
            _mainWindow = Application.Current.MainWindow;
            ConfigurarVentana(frameNavigator);

        }

        private void PrepararVentana(object sender, RoutedEventArgs e)
        {
            DateTime fechaActual = DateTime.Now;
            string fechaEscrita = "Fecha de elaboración: " + fechaActual.ToString("dd 'de' MMMM 'del' yyyy ' : ' HH:mm:ss") ;
            lblFechaActual.Content = fechaEscrita;
            ObtenerCategorias();
            CargarCheckBoxes(_categoriasInsumos, _categoriasProductosVenta);
        }

        private void BtnTodos_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> categoriassCheckBox = ObtenerCheckBoxCategorias();
            bool almenosUnaCategoria = ValidarMinimoDeCategoriasSinSeleccionar(categoriassCheckBox);
            CambiarSeleccionTodasLasCategorias(categoriassCheckBox, almenosUnaCategoria);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            List<Categoria> categoriasSeleccioandas = ObtenerCategoriasSelccionadas();
            if (categoriasSeleccioandas.Count != 0)
            {
                try
                {
                    bool incluirAgotados = chbAgotados.IsChecked.Value;
                    ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
                    Reporte bytesReporte = servicioProductosClient.GenerarReporteProductos(categoriasSeleccioandas.ToArray(), incluirAgotados);
                    GuardarReporte(bytesReporte);
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Reporte generado", "Reporte generado y guardado exitosamente", Window.GetWindow(this), 2);
                    ventanaEmergente.ShowDialog();
                }
                catch (EndpointNotFoundException ex)
                {
                    ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                    ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
                }
                catch (TimeoutException ex)
                {
                    ManejadorVentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                    ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
                }
                catch (FaultException<ExcepcionServidorItaliaPizza> ex)
                {
                    ManejadorVentanasEmergentes.MostrarVentanaErrorBaseDatos();
                    ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
                }
                catch (FaultException ex)
                {
                    ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                    ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
                }
                catch (CommunicationException ex)
                {
                    ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                    ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
                }
                catch (Exception ex)
                {
                    ManejadorVentanasEmergentes.MostrarVentanaErrorInesperado();
                    ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
                }
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "No se puede generar un reporte vacio, por favor selecciona al menos una categoría", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }
        }

        private void ObtenerCategorias()
        {
            try 
            { 
                ServicioProductosClient prodcutosCliente = new ServicioProductosClient();
                _categoriasInsumos = prodcutosCliente.RecuperarCategoriasInsumo().ToList();
                _categoriasProductosVenta = prodcutosCliente.RecuperarCategoriasProductoVenta().ToList();
            }
            catch (EndpointNotFoundException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
            }
            catch (TimeoutException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
            }
            catch (FaultException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
            }
            catch (CommunicationException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
            }
            catch (Exception ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, _frameNavegator.NavigationService);
            }
        }

        private void CargarCheckBoxes(List<Categoria> categoriasInsumo, List<Categoria> categoriasVenta)
        {
            foreach (var item in categoriasInsumo)
            {
                CheckBox chbxCategoria = new CheckBox();
                chbxCategoria.Content = item.Nombre;
                chbxCategoria.Margin = new Thickness(0,5,0,5);
                stpInsumos.Children.Add(chbxCategoria);
            }
            foreach (var item in categoriasVenta)
            {
                CheckBox chbxCategoria = new CheckBox();
                chbxCategoria.Content = item.Nombre;
                chbxCategoria.Margin = new Thickness(0, 5, 0, 5);
                stpProductosVenta.Children.Add(chbxCategoria);
            }
        }               

        private List<CheckBox> ObtenerCheckBoxCategorias()
        {
            List<CheckBox> categoriassCh = new List<CheckBox>();
            foreach (var item in stpProductosVenta.Children)
            {
                if(item is CheckBox)
                {
                    categoriassCh.Add((CheckBox)item);
                }
            }
            foreach (var item in stpInsumos.Children)
            {
                if (item is CheckBox)
                {
                    categoriassCh.Add((CheckBox)item);
                }
            }
            return categoriassCh;
        }

        private bool ValidarMinimoDeCategoriasSinSeleccionar(List<CheckBox> categoriassCheckBox)
        {
            bool validar = false;
            foreach (var item in categoriassCheckBox)
            {
                if (!(bool)item.IsChecked)
                {
                    validar = true;
                    break;
                }
            }
            return validar;
        }

        private void CambiarSeleccionTodasLasCategorias(List<CheckBox> categoriassCheckBox, bool nuevoEstado)
        {
            foreach (var item in categoriassCheckBox)
            {
               item.IsChecked = nuevoEstado;
            }
        }                   

        private void GuardarReporte(Reporte reporte)
        {
            if (reporte != null && reporte.ContenidoReporte != null)
            {
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream(reporte.ContenidoReporte))
                    {
                        using (Document document = new Document(memoryStream))
                        {
                            DateTime fechaActual = DateTime.Now;
                            string nombreArchivo = "ReporteProductos" + fechaActual.ToString("yyyyMMdd_HHmmss") + ".pdf";
                            string rutaCompleta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), nombreArchivo);
                            document.Save(rutaCompleta);
                        }
                    }
                }
                catch (Exception)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "Ocurrio un error al generar el reporte", Window.GetWindow(this), 1);
                    ventanaEmergente.ShowDialog();
                }
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "Ocurrio un error al generar el reporte", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }
        }


        private List<Categoria> ObtenerCategoriasSelccionadas()
        {
            List<Categoria> categoriassCh = new List<Categoria>();
            foreach (var item in stpProductosVenta.Children)
            {
                if (item is CheckBox chbxCategoria)
                {
                    if ((bool)chbxCategoria.IsChecked)
                    {
                        categoriassCh.Add(_categoriasProductosVenta.FirstOrDefault(cat => cat.Nombre.Equals(chbxCategoria.Content)));
                    }
                }
            }
            foreach (var item in stpInsumos.Children)
            {
                if (item is CheckBox chbxCategoria)
                {
                    if ((bool)chbxCategoria.IsChecked)
                    {
                        categoriassCh.Add(_categoriasInsumos.FirstOrDefault(cat => cat.Nombre.Equals(chbxCategoria.Content)));
                    }
                }
            }
            return categoriassCh;
        }              

        private void ConfigurarVentana(Frame frameNavigator)
        {
            this._frameNavegator = frameNavigator;
            this.Owner = _mainWindow;
            SetSizeWindow();
            SetCenterWindow();
        }

        private void SetSizeWindow()
        {
            this.Width = _mainWindow.Width;
            this.Height = _mainWindow.Height;
        }

        private void SetCenterWindow()
        {
            double centerX = _mainWindow.Left + (_mainWindow.Width - this.Width) / 2;
            double centerY = _mainWindow.Top + (_mainWindow.Height - this.Height) / 2;
            this.Left = centerX;
            this.Top = centerY;
        }
    }
}
