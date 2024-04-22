using Aspose.Pdf;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Interaction logic for ReporteProductos.xaml
    /// </summary>
    public partial class ReporteProductos : Window
    {
        List<Categoria> categoriasInsumos = new List<Categoria>();
        List<Categoria> categoriasProductosVenta = new List<Categoria>();
        private readonly Window _mainWindow;
        private Frame _frameNavigator;


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
            string fecha = "Fecha de elaboración: " + fechaActual.ToString("dd 'de' MMMM 'del' yyyy ' : ' HH:mm:ss") ;
            lblFechaActual.Content = fecha;
            ServicioProductosClient prodcutosCliente = new ServicioProductosClient();
            categoriasInsumos = prodcutosCliente.RecuperarCategoriasInsumo().ToList();
            categoriasProductosVenta = prodcutosCliente.RecuperarCategoriasProductoVenta().ToList();
            CargarCheckBoxes(categoriasInsumos, categoriasProductosVenta);
        }

        private void CargarCheckBoxes(List<Categoria> categoriasInsumo, List<Categoria> categoriasVenta)
        {
            foreach (var item in categoriasInsumo)
            {
                CheckBox categoria = new CheckBox();
                categoria.Content = item.Nombre;
                categoria.Margin = new Thickness(0,5,0,5);
                stpInsumos.Children.Add(categoria);
            }
            foreach (var item in categoriasVenta)
            {
                CheckBox categoria = new CheckBox();
                categoria.Content = item.Nombre;
                categoria.Margin = new Thickness(0, 5, 0, 5);
                stpProductosVenta.Children.Add(categoria);
            }
        }

        private void BtnTodos_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> categoriassCheckBox = ObtenerCheckBoxCategorias();
            bool almenosUnaCategoria = ValidarMinimoDeCategoriasSinSeleccionar(categoriassCheckBox);
            CambiarSeleccionTodasLasCategorias(categoriassCheckBox, almenosUnaCategoria);
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

        private void BtnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            List<Categoria> categoriasSeleccioandas = ObtenerCategoriasSelccionadas();
            if(categoriasSeleccioandas.Count != 0)
            {
                bool incluirAgotados = chbAgotados.IsChecked.Value;
                ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
                Reporte bytesReporte = servicioProductosClient.GenerarReporteProductos(categoriasSeleccioandas.ToArray(), incluirAgotados);
                GuardarReporte(bytesReporte);
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Reporte generado", "Reporte generado y guardado exitosamente", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "No se puede generar un reporte vacio, por favor selecciona al menos una categoría", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }

        }

        private void GuardarReporte(Reporte reporte)
        {
            if (reporte != null && reporte.contenidoReporte != null)
            {
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream(reporte.contenidoReporte))
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
                if (item is CheckBox)
                {
                    CheckBox categoria = (CheckBox) item;
                    if ((bool)categoria.IsChecked)
                    {
                        categoriassCh.Add(categoriasProductosVenta.FirstOrDefault(cat => cat.Nombre.Equals(categoria.Content)));
                    }
                }
            }
            foreach (var item in stpInsumos.Children)
            {
                if (item is CheckBox)
                {
                    CheckBox categoria = (CheckBox)item;
                    if ((bool)categoria.IsChecked)
                    {
                        categoriassCh.Add(categoriasInsumos.FirstOrDefault(cat => cat.Nombre.Equals(categoria.Content)));
                    }
                }
            }
            return categoriassCh;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfigurarVentana(Frame frameNavigator)
        {
            _frameNavigator = frameNavigator;
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
