using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    /// Lógica de interacción para EdicionProducto.xaml
    /// </summary>
    public partial class EdicionProducto : Page
    {
        private List<UnidadMedida > _unidadesMedida;
        private List<Categoria> _categoriasInsumo;
        private List<Categoria> _categoriasProductoVenta;
        private Producto _productoEdicion;
        private string _rutaFoto;
        private Byte[] _fotoBytes;
        private UnidadMedida _unidadMedidadSeleccionada;
        private Categoria _categoriaInsumoSeleccionada;
        private Categoria _categoriaProductoVentaSeleccionada;
        private bool _fotoCambio = false;
        private const int VENTANA_ERROR = 1;
        private const int VENTANA_INFORMACION = 2;

        public EdicionProducto(List<Categoria> categoriasProductoVenta, List<Categoria> categoriasInsumo, Producto producto)
        {
            InitializeComponent();
            _categoriasProductoVenta = categoriasProductoVenta;
            _categoriasInsumo = categoriasInsumo;
            _productoEdicion = producto;
            RemoverCategoriasInnecesarias();
            this.Loaded += EdicionProducto_Loaded;
        }

        private void RemoverCategoriasInnecesarias()
        {
            Categoria categoriaInsumo = _categoriasInsumo.FirstOrDefault(c => c.Nombre.Equals("TODAS"));
            _categoriasInsumo.Remove(categoriaInsumo);
            Categoria categoriaProductoVenta = _categoriasProductoVenta.FirstOrDefault(c => c.Nombre.Equals("TODAS"));
            _categoriasProductoVenta.Remove(categoriaProductoVenta);
        }

        /*
         * Prepara la ventana recuperando los datos necesarios de la base de datos
         * y mostrandolos.
         */
        private void EdicionProducto_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _unidadesMedida = RecuperarUnidadesMedida();
                MostrarUnidadesMedida(_unidadesMedida);
                MostrarCategoriasProductoVenta(_categoriasProductoVenta);
                MostrarCategoriasInsumo(_categoriasInsumo);
                MostrarProducto(_productoEdicion);
                _categoriaProductoVentaSeleccionada = cbxCategoriasProductoVenta.SelectedItem as Categoria;
                _categoriaInsumoSeleccionada = cbxCategoriasInsumo.SelectedItem as Categoria;
                _unidadMedidadSeleccionada = cbxUnidadMedida.SelectedItem as UnidadMedida;
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

        private List<UnidadMedida> RecuperarUnidadesMedida()
        {
            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
            return servicioProductosCliente.RecuperarUnidadesMedida().ToList();
        }

        private void MostrarUnidadesMedida(List<UnidadMedida> unidadesMedidas)
        {
            cbxUnidadMedida.ItemsSource = unidadesMedidas;
            cbxUnidadMedida.SelectedItem = unidadesMedidas.FirstOrDefault();
        }

        private void MostrarCategoriasProductoVenta(List<Categoria> categoriasProductoVenta)
        {
            cbxCategoriasProductoVenta.ItemsSource = categoriasProductoVenta;
            cbxCategoriasProductoVenta.SelectedItem = categoriasProductoVenta.FirstOrDefault();
           
        }

        private void MostrarCategoriasInsumo(List<Categoria> categoriasInsumo)
        {
            cbxCategoriasInsumo.ItemsSource = categoriasInsumo;
            cbxCategoriasProductoVenta.SelectedItem = categoriasInsumo.FirstOrDefault();
        }

        private void MostrarProducto(Producto producto)
        {
            lblCodigoProducto.Content = producto.Codigo;
            tbxNombre.Text = producto.Nombre;
            tbxDescripcion.Text = producto.Descripcion;
            if (producto.ProductoVenta != null)
            {
                MostrarProductoVenta(producto.ProductoVenta);
            }
            if (producto.Insumo != null)
            {
                MostrarInsumo(producto.Insumo);
            }
        }

        private void MostrarProductoVenta(ProductoVenta productoVenta)
        {
            rectangleBloqueoProductoVenta.Visibility = Visibility.Hidden;
            tbxPrecio.Text = productoVenta.Precio.ToString("F2");
            cbxCategoriasProductoVenta.SelectedItem = _categoriasProductoVenta.FirstOrDefault(c =>
               c.Id == _productoEdicion.ProductoVenta.Categoria.Id);
            BitmapImage mapaBits = ConvertidorBytes.ConvertirBytesABitmapImage(productoVenta.Foto);
            rectangleFotoProducto.Fill = new ImageBrush(mapaBits);
            _fotoBytes = productoVenta.Foto;

        }

        private void MostrarInsumo(Insumo insumo)
        {
            rectangleBloqueoInsumo.Visibility = Visibility.Hidden;
            tbxCantidad.Text = insumo.Cantidad.ToString("F2");
            cbxUnidadMedida.SelectedItem = _unidadesMedida.FirstOrDefault(u => u.Id == _productoEdicion.Insumo.UnidadMedida.Id);
            tbxCostoUnitario.Text = insumo.CostoUnitario.ToString("F2");
            cbxCategoriasInsumo.SelectedItem = _categoriasInsumo.FirstOrDefault(c =>
                c.Id == _productoEdicion.Insumo.Categoria.Id);
            tbxRestricciones.Text = insumo.Restriccion ?? "";

        }

        private void RectangleFoto_Click(object sender, MouseButtonEventArgs e)
        {
            string filtroArchivosImagenes = "Imágenes|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            OpenFileDialog abrirExploradorArchivos = new OpenFileDialog();
            abrirExploradorArchivos.Filter = filtroArchivosImagenes;

            try
            {
                if (abrirExploradorArchivos.ShowDialog() == true)
                {
                    _rutaFoto = abrirExploradorArchivos.FileName;

                    if (_rutaFoto != null)
                    {
                        _fotoCambio = true;
                        BitmapImage mapaBits = new BitmapImage(new Uri(_rutaFoto));
                        if (ValidarTamañoImagen())
                        {
                            rectangleFotoProducto.Fill = new ImageBrush(mapaBits);
                            _fotoBytes = File.ReadAllBytes(_rutaFoto);
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private bool ValidarTamañoImagen()
        {
            int tamañoMaximoKB = 400;
            bool esTamañoValido = false;

            try
            {
                if (_fotoCambio)
                {
                    if (_rutaFoto == null)
                    {
                        string tituloErrorImagen = "Error al cargar la imagen";
                        string mensajeErrorImagen = "No se pudo cargar la imagen. Inténtelo de nuevo o hágalo más tarde";
                        VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloErrorImagen, mensajeErrorImagen,
                            Window.GetWindow(this), VENTANA_ERROR);
                        ventanaEmergente.ShowDialog();

                        esTamañoValido = false;
                    }
                    else
                    {
                        using (FileStream flujoArchivo = new FileStream(_rutaFoto, FileMode.Open, FileAccess.Read))
                        {
                            int tamañoenBytes = (int)flujoArchivo.Length;
                            int tamañoEnKB = tamañoenBytes / 1024;

                            if (tamañoEnKB <= tamañoMaximoKB)
                            {
                                esTamañoValido = true;
                            }
                            else
                            {
                                string tituloErrorImagen = "Tamaño demasiado grande";
                                string mensajeErrorImagen = "El tamaño de la imagen es muy grande, debe ser menor o igual a "
                                    + tamañoMaximoKB + " KB";

                                VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloErrorImagen, mensajeErrorImagen,
                                    Window.GetWindow(this), VENTANA_ERROR);
                                ventanaEmergente.ShowDialog();

                                esTamañoValido = false;
                            }
                        }
                    }
                } 
                else
                {
                    esTamañoValido = true;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.StackTrace);
                esTamañoValido = false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.StackTrace);
                esTamañoValido = false;
            }

            return esTamañoValido;
        }

        private void cbxCategoriaProductoVenta_Changed(object sender, SelectionChangedEventArgs e)
        {
            _categoriaProductoVentaSeleccionada = cbxCategoriasProductoVenta.SelectedItem as Categoria;
        }

        private void cbxCategoriasInsumo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _categoriaInsumoSeleccionada = cbxCategoriasInsumo.SelectedItem as Categoria;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Productos());
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            bool camposLlenos = ValidarCamposLlenos();
            if (camposLlenos)
            {
                bool camposCorrectos = ValidarFormatosCorrectos();
                if (camposCorrectos)
                {
                    Producto producto = CrearProductoInicial();
                    bool productoPreparado = PrepararProducto(producto);
                    if (productoPreparado)
                    {
                        try
                        {
                            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
                            int filasAfectadas = servicioProductosCliente.ActualizarProducto(producto);
                            if (filasAfectadas != -1)
                            {
                                ManejarRegistroExitoso();
                            }
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
                }
            }
        }

        private void ManejarRegistroExitoso()
        {
            string tituloExito = "Producto modificado";
            string mensajeExito = "¡Producto modificado con éxito!";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloExito, mensajeExito, Window.GetWindow(this), VENTANA_INFORMACION);
            ventanaEmergente.ShowDialog();

            NavigationService.Navigate(new Productos());
        }

        private Producto CrearProductoInicial()
        {
            return new Producto()
            {
                Codigo = _productoEdicion.Codigo,
                Nombre = tbxNombre.Text,
                Descripcion = tbxDescripcion.Text
            };
        }

        private bool PrepararProducto(Producto producto)
        {
            if (_productoEdicion.ProductoVenta != null)
            {
                producto.ProductoVenta = CrearProductoVenta();
                if (producto.ProductoVenta == null)
                    return false;
            }
            if (_productoEdicion.Insumo != null)
            {
                producto.Insumo = CrearInsumo();
            }
            return true;
        }

        private Insumo CrearInsumo()
        {
            return new Insumo()
            {
                Cantidad = float.Parse(tbxCantidad.Text),
                Categoria = _categoriaInsumoSeleccionada,
                Codigo = _productoEdicion.Codigo,
                CostoUnitario = float.Parse(tbxCostoUnitario.Text),
                Restriccion = tbxRestricciones.Text,
                UnidadMedida = _unidadMedidadSeleccionada
            };
        }

        private ProductoVenta CrearProductoVenta()
        {
            bool pesoValido = ValidarTamañoImagen();
            if (pesoValido)
            {
                ProductoVenta productoVenta = new ProductoVenta()
                {
                    Categoria = _categoriaProductoVentaSeleccionada,
                    Precio = float.Parse(tbxPrecio.Text),
                    Foto = _fotoBytes
                };
                return productoVenta;
            }
            return null;
        }

        private bool ValidarFormatosCorrectos()
        {
            bool camposCorrectos = true;
            if (!UtilidadValidacion.EsNombreProductoValido(tbxNombre.Text))
            {
                camposCorrectos = false;
                Utilidad.MostrarTextoError(lblErrorNombre, "Datos no válidos.");
            }
            if (!UtilidadValidacion.EsDescripcionProductoValida(tbxDescripcion.Text))
            {
                camposCorrectos |= false;
                Utilidad.MostrarTextoError(lblErrorDescripcion, "Datos no válidos.");
            }
            if (_productoEdicion.ProductoVenta != null)
            {
                camposCorrectos = ValidarFormatosCorrectosProductoVenta();
                if (camposCorrectos == false)
                {
                    return false;
                }
            }
            if (_productoEdicion.Insumo != null)
            {
                camposCorrectos = ValidarFormatosCorrectosInsumo();
            }

            return camposCorrectos;
        }

        private bool ValidarFormatosCorrectosInsumo()
        {
            bool camposCorrectos = true;
            if (!UtilidadValidacion.ValidarCantidadInsumo(float.Parse(tbxCantidad.Text), _unidadMedidadSeleccionada.Nombre, lblErrorCantidad))
            {
                camposCorrectos = false;
            }
            if (double.Parse(tbxCostoUnitario.Text) <= 0)
            {
                camposCorrectos = false;
                Utilidad.MostrarTextoError(lblErrorCostoUnitario, "Datos no validos. El costo debe ser mayor a 0");
            }
            if (!UtilidadValidacion.esRestriccionInsumoValida(tbxRestricciones.Text))
            {
                camposCorrectos = false;
                Utilidad.MostrarTextoError(lblErrorRestriccion, "Datos no validos.");
            }
            return camposCorrectos;
        }

        private bool ValidarFormatosCorrectosProductoVenta()
        {
            bool camposCorrectos = true;
            if (double.Parse(tbxPrecio.Text) <= 0)
            {
                camposCorrectos = false;
                Utilidad.MostrarTextoError(lblErrorPrecio, "Datos no válidos.");
            }
            return camposCorrectos;
        }

        private void cbxUnidadMedida_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _unidadMedidadSeleccionada = (UnidadMedida) (sender as ComboBox).SelectedItem;
        }

        private bool ValidarCamposLlenos()
        {
            bool camposLlenos = true;

            if (string.IsNullOrWhiteSpace(tbxNombre.Text))
            {
                camposLlenos = false;
                Utilidad.MostrarTextoError(lblErrorNombre, "Campo obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(tbxDescripcion.Text))
            {
                camposLlenos = false;
                Utilidad.MostrarTextoError(lblErrorDescripcion, "Campo obligatorio.");
            }

            if (_productoEdicion.ProductoVenta != null)
            {
                if (!ValidarCamposLlenosProductoVenta())
                {
                    camposLlenos = false;
                }
            }

            if (_productoEdicion.Insumo != null)
            {
                if (!ValidarCamposLlenosInsumo())
                {
                    camposLlenos = false; 
                }
            }

            return camposLlenos;
        }

        private bool ValidarCamposLlenosProductoVenta()
        {
            bool camposLlenos = true;

            if (string.IsNullOrWhiteSpace(tbxPrecio.Text))
            {
                camposLlenos = false;
                Utilidad.MostrarTextoError(lblErrorPrecio, "Campo obligatorio.");
            }

            return camposLlenos;
        }

        private bool ValidarCamposLlenosInsumo()
        {
            bool camposLlenos = true;

            if (string.IsNullOrWhiteSpace(tbxCantidad.Text))
            {
                camposLlenos = false;
                Utilidad.MostrarTextoError(lblErrorCantidad, "Campo obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(tbxCostoUnitario.Text))
            {
                camposLlenos = false;
                Utilidad.MostrarTextoError(lblErrorCostoUnitario, "Campo obligatorio.");
            }

            return camposLlenos;
        }


        private void TbxNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblErrorNombre.Visibility = Visibility.Hidden;
        }

        private void TbxDescripcion_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblErrorDescripcion.Visibility = Visibility.Hidden;
        }

        private void TbxCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblErrorCantidad.Visibility = Visibility.Hidden;
        }

        private void TbxCostoUnitario_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblErrorCostoUnitario.Visibility= Visibility.Hidden;
        }

        private void TbxPrecio_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblErrorPrecio.Visibility = Visibility.Hidden;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            double result;
            TextBox textBox = sender as TextBox;
            string fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            bool isDecimal = Double.TryParse(fullText, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            if (!isDecimal || fullText.Equals(""))
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
