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
    /// Lógica de interacción para RegistroProducto.xaml
    /// </summary>
    public partial class RegistroProducto : Page
    {
        private const int SELECCION_POR_DEFECTO_COMBO_BOX = 0;
        private const string ATRIBUTO_A_MOSTRAR_COMBO_BOX = "Nombre";
        private const string MENSAJE_CAMPO_OBLIGATORIO = "*Campo obligatorio";
        private Byte[] _fotoBytes;
        private string _rutaFoto;

        public RegistroProducto()
        {
            InitializeComponent();
            CargarComboBoxes();
        }

        private void CargarComboBoxes()
        {
            CargarComboBoxCategorias();
            CargarComboBoxUnidadMedida();
        }

        private void CargarComboBoxCategorias()
        {
            string categoriaInicial = "Selecciona una categoría";
            Categoria[] categorias = RecuperarCategorias();

            Categoria categoriaDefault = new Categoria()
            {
                Id = SELECCION_POR_DEFECTO_COMBO_BOX,
                Nombre = categoriaInicial
            };

            cbxCategoria.Items.Add(categoriaDefault);

            if (categorias != null)
            {
                foreach (Categoria categoria in categorias)
                {
                    cbxCategoria.Items.Add(categoria);
                }

                cbxCategoria.DisplayMemberPath = ATRIBUTO_A_MOSTRAR_COMBO_BOX;
                cbxCategoria.SelectedIndex = SELECCION_POR_DEFECTO_COMBO_BOX;
            }
        }

        private Categoria[] RecuperarCategorias()
        {
            Categoria[] categorias = new Categoria[0]; 

            try
            {
                ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
                categorias = servicioProductosCliente.RecuperarCategorias();
            }
            catch (EndpointNotFoundException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (TimeoutException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (FaultException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (CommunicationException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }

            return categorias;
        }

        private void CargarComboBoxUnidadMedida()
        {
            string unidadMedidaInicial = "Selecciona unidad medición";
            UnidadMedida[] unidadesMedida = RecuperarUnidadesMedida();

            UnidadMedida unidadDefault = new UnidadMedida()
            {
                Id = SELECCION_POR_DEFECTO_COMBO_BOX,
                Nombre = unidadMedidaInicial
            };

            cbxUnidadMedida.Items.Add(unidadDefault);

            if (unidadesMedida != null)
            {
                foreach (UnidadMedida unidad in unidadesMedida)
                {
                    cbxUnidadMedida.Items.Add(unidad);
                }

                cbxUnidadMedida.DisplayMemberPath = ATRIBUTO_A_MOSTRAR_COMBO_BOX;
                cbxUnidadMedida.SelectedIndex = SELECCION_POR_DEFECTO_COMBO_BOX;
            }
        }

        private UnidadMedida[] RecuperarUnidadesMedida()
        {
            UnidadMedida[] unidadesMedida = new UnidadMedida[0];

            try
            {
                ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
                unidadesMedida = servicioProductosCliente.RecuperarUnidadesMedida();
            }
            catch (EndpointNotFoundException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (TimeoutException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (FaultException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (CommunicationException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }

            return unidadesMedida;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarEtiquetasError();

            if (ValidarSeccionRegistroHabilitada())
            {
                if (ValidarRegistro())
                {
                    if (ValidarCodigoUnico())
                    {
                        GuardarProducto();
                    } 
                    else
                    {
                        string mensajeErrorCodigo = "Código existente";
                        MostrarEtiquetaError(lbErrorCodigo, mensajeErrorCodigo);
                    }
                }
            }
        }

        private void GuardarProducto()
        {
            //TRY-CATCH
            int filasAfectadas = -1;
            Producto producto = GenerarProductoAGuardar();

            try
            {
                ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
                filasAfectadas = servicioProductosClient.GuardarProducto(producto);
            }
            catch (EndpointNotFoundException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (TimeoutException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (FaultException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (CommunicationException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }

            if (filasAfectadas > 0)
            {
                ManejarRegistroExitoso();
            }
        }

        private Producto GenerarProductoAGuardar()
        {
            Producto producto = CrearProducto();

            if (chbxEsInventariado.IsChecked == true)
            {
                producto.EsInventariado = true;
                producto.Insumo = CrearInsumo();
            }

            if (chbxEsProductoVenta.IsChecked == true)
            {
                producto.ProductoVenta = CrearProductoVenta();
            }

            return producto;
        }

        private Producto CrearProducto()
        {
            bool esActivo = true;
            string codigo = tbxCodigo.Text.Trim();
            string nombre = tbxNombre.Text.Trim();
            string descripcion = tbxDescripcion.Text.Trim();
            Categoria categoria = (Categoria)cbxCategoria.SelectedItem;

            Producto producto = new Producto();
            producto.Codigo = codigo;
            producto.Nombre = nombre;
            producto.Descripcion = descripcion;
            producto.EsActivo = esActivo;

            return producto;
        }

        private Insumo CrearInsumo()
        {
            string codigo = tbxCodigo.Text.Trim();
            string cantidad = tbxCantidad.Text.Trim();
            float cantidadInsumo = ConvertirStringAFloat(cantidad, null);
            UnidadMedida unidadMedida = (UnidadMedida)cbxUnidadMedida.SelectedItem;
            string costo = tbxCostoUnitario.Text.Trim();
            float costoUnitarioInsumo = ConvertirStringAFloat(costo, null);
            string restricciones = tbxRestricciones.Text.Trim();
            Categoria categoria = (Categoria)cbxCategoria.SelectedItem;

            Insumo insumo = new Insumo
            {
                Codigo = codigo,
                Cantidad = cantidadInsumo,
                UnidadMedida = unidadMedida,
                CostoUnitario = costoUnitarioInsumo,
                Restriccion = restricciones,
                Categoria = categoria
            };

            return insumo;
        }

        private ProductoVenta CrearProductoVenta()
        {
            string codigo = tbxCodigo.Text.Trim();
            string precio = tbxPrecio.Text.Trim();
            float precioProductoVenta = ConvertirStringAFloat(precio, null);
            Byte[] foto = _fotoBytes;
            Categoria categoria = (Categoria)cbxCategoria.SelectedItem;

            ProductoVenta productoVenta = new ProductoVenta
            {
                Codigo = codigo,
                Precio = precioProductoVenta,
                Categoria = categoria,
                Foto = foto
            };

            return productoVenta;
        }

        private void ManejarRegistroExitoso()
        {
            string tituloExito = "Producto registrado";
            string mensajeExito = "Producto registrado con éxito";

            LimpiarCampos();

            MessageBoxResult result = System.Windows.MessageBox.Show(
                    mensajeExito,
                    tituloExito, MessageBoxButton.OK);
        }

        private void LimpiarEtiquetasError()
        {
            lbErrorCodigo.Visibility = Visibility.Collapsed;
            lbErrorNombre.Visibility = Visibility.Collapsed;
            lbErrorCategoria.Visibility = Visibility.Collapsed;
            lbErrorDescripcion.Visibility = Visibility.Collapsed;

            lbErrorPrecio.Visibility = Visibility.Collapsed;
            lbErrorFoto.Visibility = Visibility.Collapsed;

            lbErrorCantidad.Visibility = Visibility.Collapsed;
            lbErrorUnidadMedida.Visibility = Visibility.Collapsed;
            lbErrorCostoUnitario.Visibility = Visibility.Collapsed;
            lbErrorRestricciones.Visibility = Visibility.Collapsed;
        }

        private void MostrarEtiquetaError(Label lbError, string mensajeError)
        {
            lbError.Content = mensajeError;
            lbError.Visibility = Visibility.Visible;
        }

        private void MostrarErrorCampoObligatorio(Label lbError)
        {
            MostrarEtiquetaError(lbError, MENSAJE_CAMPO_OBLIGATORIO);
        }

        private bool ValidarRegistro()
        {
            bool esRegistroValido = true;

            if (!ValidarDatosProducto())
            {
                esRegistroValido = false;
            }
            
            if (chbxEsInventariado.IsChecked == true)
            {
                if (!ValidarDatosInsumo())
                {
                    esRegistroValido = false;
                }
            }

            if (chbxEsProductoVenta.IsChecked == true)
            {
                if (!ValidarDatosProductoVenta())
                {
                    esRegistroValido = false;
                }
            }

            return esRegistroValido;
        }

        private bool ValidarDatosProducto()
        {
            bool datosProductoValidos = true;

            if (!ValidarFormatoCodigo())
            {
                datosProductoValidos = false;
            }

            if (!ValidarNombreProducto())
            {
                datosProductoValidos = false;
            }

            if (!ValidarCategoriaProducto())
            {
                datosProductoValidos = false;
            }

            if (!ValidarDescripcionProducto())
            {
                datosProductoValidos = false;
            }

            return datosProductoValidos;
        }

        private bool ValidarFormatoCodigo()
        {
            bool esCodigoProductoValido = false;
            string mensajeErrorCodigo = "Código no válido";

            string codigoProducto = tbxCodigo.Text.Trim();
            esCodigoProductoValido = UtilidadValidacion.EsCodigoProductoValido(codigoProducto);

            if (string.IsNullOrEmpty(codigoProducto))
            {
                MostrarErrorCampoObligatorio(lbErrorCodigo);
                esCodigoProductoValido = false;
            }
            else if (!esCodigoProductoValido)
            {
                MostrarEtiquetaError(lbErrorCodigo, mensajeErrorCodigo);
            }

            return esCodigoProductoValido;
        }

        private bool ValidarCodigoUnico()
        {
            //TRY-CATCH
            bool esCodigoUnico = false;

            string codigoProducto = tbxCodigo.Text.Trim();

            try
            {
                ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
                esCodigoUnico = servicioProductosClient.ValidarCodigoProducto(codigoProducto);
            }
            catch (EndpointNotFoundException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (TimeoutException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (FaultException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (CommunicationException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }

            return esCodigoUnico;
        }

        private bool ValidarNombreProducto()
        {
            string mensajeErrorNombre = "Datos no válidos";
            string nombreProducto = tbxNombre.Text.Trim();

            bool esNombreProductoValido = UtilidadValidacion.EsNombreProductoValido(nombreProducto);

            if (string.IsNullOrEmpty(nombreProducto))
            {
                MostrarErrorCampoObligatorio(lbErrorNombre);
                esNombreProductoValido = false;
            }
            else if (!esNombreProductoValido)
            {
                MostrarEtiquetaError(lbErrorNombre, mensajeErrorNombre);
            }

            return esNombreProductoValido;
        }

        private bool ValidarCategoriaProducto()
        {
            bool esCategoriaValida = true;

            if (cbxCategoria.SelectedIndex <= SELECCION_POR_DEFECTO_COMBO_BOX)
            {
                MostrarErrorCampoObligatorio(lbErrorCategoria);
                esCategoriaValida = false;
            }

            return esCategoriaValida;
        }

        private bool ValidarDescripcionProducto()
        {
            string mensajeErrorDescripcion = "Datos no válidos";
            string descripcionProducto = tbxDescripcion.Text.Trim();

            bool esDescripcionValida = UtilidadValidacion.EsDescripcionProductoValida(descripcionProducto);

            if (string.IsNullOrEmpty(descripcionProducto))
            {
                MostrarErrorCampoObligatorio(lbErrorDescripcion);
                esDescripcionValida = false;
            }
            else if (!esDescripcionValida)
            {
                MostrarEtiquetaError(lbErrorDescripcion, mensajeErrorDescripcion);
            }

            return esDescripcionValida;
        }

        private bool ValidarDatosInsumo()
        {
            bool datosInsumoValido = true;

            if (!ValidarUnidadMedidaInsumo())
            {
                datosInsumoValido = false;
            }

            if (!ValidarCantidadInsumo())
            {
                datosInsumoValido = false;
            }

            if (!ValidarCostoUnitarioInsumo())
            {
                datosInsumoValido = false;
            }

            if (!ValidarRestriccionInsumo())
            {
                datosInsumoValido = false;
            }

            return datosInsumoValido;
        }

        private bool ValidarUnidadMedidaInsumo()
        {
            bool esUnidadMedidaValida = true;

            if (cbxUnidadMedida.SelectedIndex <= SELECCION_POR_DEFECTO_COMBO_BOX)
            {
                MostrarErrorCampoObligatorio(lbErrorUnidadMedida);
                esUnidadMedidaValida = false;
            }

            return esUnidadMedidaValida;
        }

        private bool ValidarCantidadInsumo()
        {
            bool esCantidadValida = true;

            string medidaEnteros = "Unidad";
            string mensajeErrorCantidad = "Datos no válidos. Debe ser positiva la cantidad";

            string cantidad = tbxCantidad.Text.Trim();
            float cantidadInsumo = ConvertirStringAFloat(cantidad, lbErrorCantidad);
            UnidadMedida unidadSeleccionada = (UnidadMedida)cbxUnidadMedida.SelectedItem;
            string nombreUnidadSeleccionada = unidadSeleccionada.Nombre;

            if (string.IsNullOrEmpty(cantidad))
            {
                MostrarErrorCampoObligatorio(lbErrorCantidad);
                esCantidadValida = false;
            }
            else if (cantidadInsumo >= 0 && nombreUnidadSeleccionada == medidaEnteros)
            {
                esCantidadValida = ValidarCantidadUnitaria(cantidadInsumo);
            }
            else if (cantidadInsumo <= 0)
            {
                MostrarEtiquetaError(lbErrorCantidad, mensajeErrorCantidad);
                esCantidadValida = false;
            }

            return esCantidadValida;
        }

        private float ConvertirStringAFloat(string numeroEnString, Label lbError)
        {
            float numeroConvertido = -1;

            try
            {
                numeroConvertido = Convert.ToSingle(numeroEnString);
            }
            catch (FormatException)
            {
                string mensajeErrorFormato = "Solo puede incluir números. Ej. 2, .5";
                MostrarEtiquetaError(lbError, mensajeErrorFormato);
            }
            catch (OverflowException)
            {
                string mensajeErrorOverFlow = "Cantidad no permitida, por favor corrigelo.";
                MostrarEtiquetaError(lbError, mensajeErrorOverFlow);
            }

            return numeroConvertido;
        }

        private bool ValidarCantidadUnitaria(float cantidadInsumo)
        {
            bool esCantidadValida = true;
            string mensajeErrorCantidad = "Datos no válidos. Deben ser números enteros";

            if (cantidadInsumo % 1 != 0)
            {
                MostrarEtiquetaError(lbErrorCantidad, mensajeErrorCantidad);
                esCantidadValida = false;
            }
            
            return esCantidadValida;
        }

        private bool ValidarCostoUnitarioInsumo()
        {
            bool esCostoValido = true;
            string mensajeErrorCosto = "Costo no válido. Debe ser número positivo";
            float costoMinimoValido = 0;

            string costo = tbxCostoUnitario.Text.Trim();
            float costoUnitario = ConvertirStringAFloat(costo, lbErrorCostoUnitario);

            if (string.IsNullOrEmpty(costo))
            {
                MostrarErrorCampoObligatorio(lbErrorCostoUnitario);
                esCostoValido = false;
            }
            else if (costoUnitario <= costoMinimoValido)
            {
                esCostoValido = false;
                MostrarEtiquetaError(lbErrorCostoUnitario, mensajeErrorCosto);
            }

            return esCostoValido;
        }

        private bool ValidarRestriccionInsumo()
        {
            string mensajeErrorRestriccion = "Restricciones no válidas";

            string restriccion = tbxRestricciones.Text.Trim();
            bool esRestriccionValida = UtilidadValidacion.esRestriccionInsumoValida(restriccion);

            if (!esRestriccionValida)
            {
                MostrarEtiquetaError(lbErrorRestricciones, mensajeErrorRestriccion);
            }

            return esRestriccionValida;
        }

        private bool ValidarDatosProductoVenta()
        {
            bool esProductoVentaValido = true;

            if (!ValidarPrecioProductoVenta())
            {
                esProductoVentaValido = false;
            }

            return esProductoVentaValido;
        }

        private bool ValidarPrecioProductoVenta()
        {
            bool esPrecioValido = true;
            string mensajeErrorPrecio = "Precio no válido.";
            string precio = tbxPrecio.Text.Trim();
            float precioProductoVenta = ConvertirStringAFloat(precio, lbErrorPrecio);
            float precioMinimoValido = 0;

            if (string.IsNullOrEmpty(precio))
            {
                MostrarErrorCampoObligatorio(lbErrorPrecio);
                esPrecioValido = false;
            }
            else if (precioProductoVenta <= precioMinimoValido)
            {
                MostrarEtiquetaError(lbErrorPrecio, mensajeErrorPrecio);
                esPrecioValido = false;
            }

            return esPrecioValido;
        }

        private bool ValidarSeccionRegistroHabilitada()
        {
            bool haySeccionHabilitada = false;

            if (chbxEsProductoVenta.IsChecked == true || chbxEsInventariado.IsChecked == true)
            {
                haySeccionHabilitada = true;
            }
            else
            {
                // TODO: Ventana emergente personalizada
                string tituloError = "Habilita una sección";
                string mensajeError = "Debes indicar si el producto está destinado a la venta o si es inventariado";

                MessageBoxResult result = System.Windows.MessageBox.Show(
                    mensajeError,
                    tituloError, MessageBoxButton.OK);
            }

            return haySeccionHabilitada;
        }
       
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            string tituloCancelar = "Cancelar Registro";
            string mensajeCancelar = "¿Estás seguro de que deseas cancelar el registro del producto?";

            MessageBoxResult result = System.Windows.MessageBox.Show(
                    mensajeCancelar,
                    tituloCancelar, MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                LimpiarCampos();
                //NavigationService.GoBack();
            }
        }

        private void LimpiarCampos()
        {
            tbxCodigo.Text = string.Empty;
            tbxNombre.Text = string.Empty;
            tbxDescripcion.Text = string.Empty;
            cbxCategoria.SelectedIndex = SELECCION_POR_DEFECTO_COMBO_BOX;

            chbxEsInventariado.IsChecked = false;
            tbxCantidad.Text = string.Empty;
            tbxCostoUnitario.Text = string.Empty;
            tbxRestricciones.Text = string.Empty;
            cbxUnidadMedida.SelectedIndex = SELECCION_POR_DEFECTO_COMBO_BOX;

            chbxEsProductoVenta.IsChecked = false;
            tbxPrecio.Text = string.Empty;
            rectangleFotoProducto.Fill = new SolidColorBrush(Colors.Transparent);
        }

        private void ChbxEsInventariado_Checked(object sender, RoutedEventArgs e)
        {
            if (chbxEsInventariado.IsChecked == true)
            {
                rectangleBloqueoInsumo.Visibility = Visibility.Collapsed;
            }
        }

        private void ChbxEsInventariado_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chbxEsInventariado.IsChecked == false)
            {
                rectangleBloqueoInsumo.Visibility = Visibility.Visible;
            }
        }

        private void ChbxEsProductoVenta_Checked(object sender, RoutedEventArgs e)
        {
            if (chbxEsProductoVenta.IsChecked == true)
            {
                rectangleBloqueoProductoVenta.Visibility = Visibility.Collapsed;
            }
        }

        private void ChbxEsProductoVenta_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chbxEsProductoVenta.IsChecked == false)
            {
                rectangleBloqueoProductoVenta.Visibility = Visibility.Visible;
            }
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
                if (_rutaFoto == null)
                {
                    string tituloErrorImagen = "Error al cargar la imagen";
                    string mensajeErrorImagen = "Ocurrió un error al cargar la imagen";
                    MessageBoxResult result = System.Windows.MessageBox.Show(
                    mensajeErrorImagen,
                    tituloErrorImagen, MessageBoxButton.OK);

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

                            MessageBoxResult result = System.Windows.MessageBox.Show(
                            mensajeErrorImagen,
                            tituloErrorImagen, MessageBoxButton.OK);

                            esTamañoValido = false;
                        }
                    }
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
    }
}
