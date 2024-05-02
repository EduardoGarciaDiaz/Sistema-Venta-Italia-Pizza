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
        private const int VENTANA_ERROR = 1;
        private const int VENTANA_INFORMACION = 2;
        private const int VENTANA_CONFIRMACION = 3;
        private string _rutaFoto;
        private Byte[] _fotoBytes;

        public RegistroProducto()
        {
            InitializeComponent();
            this.Loaded += RegistroProducto_Loaded;
        }

        private void RegistroProducto_Loaded(object sender, RoutedEventArgs e)
        {
            CargarComboBoxes();
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
                        Utilidad.MostrarTextoError(lblErrorCodigo, mensajeErrorCodigo);
                    }
                }
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            string tituloCancelar = "Cancelar Registro";
            string mensajeCancelar = "¿Estás seguro de que deseas cancelar el registro del producto?";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloCancelar, mensajeCancelar, "Sí", "No", Window.GetWindow(this), VENTANA_CONFIRMACION);
            ventanaEmergente.ShowDialog();

            if (ventanaEmergente.AceptarAccion)
            {
                LimpiarCampos();
                NavigationService.GoBack();
            }
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

        private void CargarComboBoxes()
        {
            try
            {
                CargarComboBoxCategorias();
                CargarComboBoxUnidadMedida();
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

            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
            categorias = servicioProductosCliente.RecuperarCategorias();

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

            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
            unidadesMedida = servicioProductosCliente.RecuperarUnidadesMedida();

            return unidadesMedida;
        }

        private void GuardarProducto()
        {
            int filasAfectadas = -1;
            Producto producto = GenerarProductoAGuardar();

            try
            {
                ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
                filasAfectadas = servicioProductosCliente.GuardarProducto(producto);
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

            Producto producto = new Producto
            {
                Codigo = codigo,
                Nombre = nombre,
                Descripcion = descripcion,
                EsActivo = esActivo
            };

            return producto;
        }

        private Insumo CrearInsumo()
        {
            string codigo = tbxCodigo.Text.Trim();
            string cantidad = tbxCantidad.Text.Trim();
            float cantidadInsumo = Utilidad.ConvertirStringAFloat(cantidad, null);
            UnidadMedida unidadMedida = (UnidadMedida)cbxUnidadMedida.SelectedItem;
            string costo = tbxCostoUnitario.Text.Trim();
            float costoUnitarioInsumo = Utilidad.ConvertirStringAFloat(costo, null);
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
            float precioProductoVenta = Utilidad.ConvertirStringAFloat(precio, null);
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

            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloExito, mensajeExito, Window.GetWindow(this),
                VENTANA_INFORMACION);
            ventanaEmergente.ShowDialog();
        }

        private void LimpiarEtiquetasError()
        {
            lblErrorCodigo.Visibility = Visibility.Collapsed;
            lblErrorNombre.Visibility = Visibility.Collapsed;
            lblErrorCategoria.Visibility = Visibility.Collapsed;
            lblErrorDescripcion.Visibility = Visibility.Collapsed;

            lblErrorPrecio.Visibility = Visibility.Collapsed;
            lblErrorFoto.Visibility = Visibility.Collapsed;

            lblErrorCantidad.Visibility = Visibility.Collapsed;
            lblErrorUnidadMedida.Visibility = Visibility.Collapsed;
            lblErrorCostoUnitario.Visibility = Visibility.Collapsed;
            lblErrorRestricciones.Visibility = Visibility.Collapsed;
        }



        private void MostrarErrorCampoObligatorio(Label lbError)
        {
            Utilidad.MostrarTextoError(lbError, MENSAJE_CAMPO_OBLIGATORIO);
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
                MostrarErrorCampoObligatorio(lblErrorCodigo);
                esCodigoProductoValido = false;
            }
            else if (!esCodigoProductoValido)
            {
                Utilidad.MostrarTextoError(lblErrorCodigo, mensajeErrorCodigo);
            }

            return esCodigoProductoValido;
        }

        private bool ValidarCodigoUnico()
        {
            bool esCodigoUnico = false;

            string codigoProducto = tbxCodigo.Text.Trim();

            try
            {
                ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
                esCodigoUnico = servicioProductosClient.ValidarCodigoProducto(codigoProducto);
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

            return esCodigoUnico;
        }

        private bool ValidarNombreProducto()
        {
            string mensajeErrorNombre = "Datos no válidos";
            string nombreProducto = tbxNombre.Text.Trim();

            bool esNombreProductoValido = UtilidadValidacion.EsNombreProductoValido(nombreProducto);

            if (string.IsNullOrEmpty(nombreProducto))
            {
                MostrarErrorCampoObligatorio(lblErrorNombre);
                esNombreProductoValido = false;
            }
            else if (!esNombreProductoValido)
            {
                Utilidad.MostrarTextoError(lblErrorNombre, mensajeErrorNombre);
            }

            return esNombreProductoValido;
        }

        private bool ValidarCategoriaProducto()
        {
            bool esCategoriaValida = true;

            if (cbxCategoria.SelectedIndex <= SELECCION_POR_DEFECTO_COMBO_BOX)
            {
                MostrarErrorCampoObligatorio(lblErrorCategoria);
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
                MostrarErrorCampoObligatorio(lblErrorDescripcion);
                esDescripcionValida = false;
            }
            else if (!esDescripcionValida)
            {
                Utilidad.MostrarTextoError(lblErrorDescripcion, mensajeErrorDescripcion);
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
                MostrarErrorCampoObligatorio(lblErrorUnidadMedida);
                esUnidadMedidaValida = false;
            }

            return esUnidadMedidaValida;
        }

        private bool ValidarCantidadInsumo()
        {
            bool esCantidadValida = true;

            string cantidad = tbxCantidad.Text.Trim();
            float cantidadInsumo = Utilidad.ConvertirStringAFloat(cantidad, lblErrorCantidad);
            UnidadMedida unidadSeleccionada = (UnidadMedida)cbxUnidadMedida.SelectedItem;
            string nombreUnidadSeleccionada = unidadSeleccionada.Nombre;

            esCantidadValida = UtilidadValidacion.ValidarCantidadInsumo(cantidadInsumo, nombreUnidadSeleccionada, lblErrorCantidad);

            return esCantidadValida;
        }

        private bool ValidarCostoUnitarioInsumo()
        {
            bool esCostoValido = true;
            string mensajeErrorCosto = "Costo no válido. Debe ser número positivo";
            float costoMinimoValido = 0;

            string costo = tbxCostoUnitario.Text.Trim();
            float costoUnitario = Utilidad.ConvertirStringAFloat(costo, lblErrorCostoUnitario);

            if (string.IsNullOrEmpty(costo))
            {
                MostrarErrorCampoObligatorio(lblErrorCostoUnitario);
                esCostoValido = false;
            }
            else if (costoUnitario <= costoMinimoValido)
            {
                esCostoValido = false;
                Utilidad.MostrarTextoError(lblErrorCostoUnitario, mensajeErrorCosto);
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
                Utilidad.MostrarTextoError(lblErrorRestricciones, mensajeErrorRestriccion);
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
            float precioProductoVenta = Utilidad.ConvertirStringAFloat(precio, lblErrorPrecio);
            float precioMinimoValido = 0;

            if (string.IsNullOrEmpty(precio))
            {
                MostrarErrorCampoObligatorio(lblErrorPrecio);
                esPrecioValido = false;
            }
            else if (precioProductoVenta <= precioMinimoValido)
            {
                Utilidad.MostrarTextoError(lblErrorPrecio, mensajeErrorPrecio);
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
                string tituloError = "Habilita una sección";
                string mensajeError = "Debes indicar si el producto está destinado a la venta o si es inventariado";

                VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloError, mensajeError, Window.GetWindow(this), VENTANA_INFORMACION);
                ventanaEmergente.ShowDialog();
            }

            return haySeccionHabilitada;
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

        private bool ValidarTamañoImagen()
        {
            int tamañoMaximoKB = 400;
            bool esTamañoValido = false;

            try
            {
                if (_rutaFoto == null)
                {
                    string tituloErrorImagen = "Error al cargar la imágen";
                    string mensajeErrorImagen = "No se pudo cargar la imágen. Inténtelo de nuevo o hágalo más tarde";
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
                            string mensajeErrorImagen = "El tamaño de la imágen es muy grande, debe ser menor o igual a " 
                                + tamañoMaximoKB + " KB";

                            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloErrorImagen, mensajeErrorImagen,
                                Window.GetWindow(this), VENTANA_ERROR);
                            ventanaEmergente.ShowDialog();

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
