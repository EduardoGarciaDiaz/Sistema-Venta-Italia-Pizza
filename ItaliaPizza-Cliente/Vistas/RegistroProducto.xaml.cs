using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            Categoria[] categorias = RecuperarCategorias();

            cbxCategoria.Items.Add("Selecciona una categoría");

            if (categorias != null)
            {
                foreach (var categoria in categorias)
                {
                    Console.WriteLine(categoria);
                    cbxCategoria.Items.Add(categoria);
                }

                cbxCategoria.DisplayMemberPath = "Nombre";
                cbxCategoria.SelectedIndex = 0;
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
            // TODO
            List<string> unidadesMedida = new List<string>();
            unidadesMedida.Add("Seleccionar unidad medición");
            unidadesMedida.Add("Kg");
            unidadesMedida.Add("Lts");
            unidadesMedida.Add("Unidad");
            cbxUnidadMedida.ItemsSource = unidadesMedida;
            cbxUnidadMedida.SelectedIndex = 0;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            bool esRegistroValido = false;
            bool esCodigoUnico = false;

            LimpiarEtiquetasError();

            if (ValidarSeccionRegistroHabilitada())
            {
                esRegistroValido = ValidarRegistro();
                esCodigoUnico = ValidarCodigoUnico();
                
                if (esRegistroValido && esCodigoUnico)
                {
                    GuardarProducto();
                }
                else
                {
                    Console.WriteLine("No se pudo guardar, revisa los datos ingresados");
                }
            }
        }

        private void GuardarProducto()
        {
            Console.WriteLine("Guardando producto...");
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
            MostrarEtiquetaError(lbError, "*Campo obligatorio");
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

            string codigoProducto = tbxCodigo.Text.Trim();
            esCodigoProductoValido = UtilidadValidacion.EsCodigoProductoValido(codigoProducto);

            if (string.IsNullOrEmpty(codigoProducto))
            {
                MostrarErrorCampoObligatorio(lbErrorCodigo);
                esCodigoProductoValido = false;
            }
            else if (!esCodigoProductoValido)
            {
                MostrarEtiquetaError(lbErrorCodigo, "Datos no válidos");
            }

            return esCodigoProductoValido;
        }

        private bool ValidarCodigoUnico()
        {
            bool esCodigoUnico = true; //cambiar a false
            //TODO: Ir a servicio

            return esCodigoUnico;
        }

        private bool ValidarNombreProducto()
        {
            bool esNombreProductoValido = false;

            string nombreProducto = tbxNombre.Text.Trim();
            esNombreProductoValido = UtilidadValidacion.EsNombreProductoValido(nombreProducto);

            if (string.IsNullOrEmpty(nombreProducto))
            {
                MostrarErrorCampoObligatorio(lbErrorNombre);
                esNombreProductoValido = false;
            }
            else if (!esNombreProductoValido)
            {
                MostrarEtiquetaError(lbErrorNombre, "Datos no válidos");
            }

            return esNombreProductoValido;
        }

        private bool ValidarCategoriaProducto()
        {
            bool esCategoriaValida = false;

            if (cbxCategoria.SelectedIndex > 0)
            {
                esCategoriaValida = true;
            } else
            {
                MostrarErrorCampoObligatorio(lbErrorCategoria);
                esCategoriaValida = false;
            }

            return esCategoriaValida;
        }

        private bool ValidarDescripcionProducto()
        {
            bool esDescripcionValida = false;
            string descripcionProducto = tbxDescripcion.Text.Trim();

            esDescripcionValida = UtilidadValidacion.EsDescripcionProductoValida(descripcionProducto);

            if (string.IsNullOrEmpty(descripcionProducto))
            {
                MostrarErrorCampoObligatorio(lbErrorDescripcion);
                esDescripcionValida = false;
            }
            else if (!esDescripcionValida)
            {
                MostrarEtiquetaError(lbErrorDescripcion, "Datos no válidos");
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

            if (!ValidarCantidadValidaInsumo())
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
            bool esUnidadMedidaValida = false;

            if (cbxUnidadMedida.SelectedIndex > 0)
            {
                esUnidadMedidaValida = true;
            }
            else
            {
                MostrarErrorCampoObligatorio(lbErrorUnidadMedida);
                esUnidadMedidaValida = false;
            }

            return esUnidadMedidaValida;
        }

        private bool ValidarCantidadValidaInsumo()
        {
            bool esCantidadValida = true;

            string cantidad = tbxCantidad.Text.Trim();
            float cantidadInsumo = ConvertirStringAFloat(cantidad, lbErrorCantidad);

            if (string.IsNullOrEmpty(cantidad))
            {
                MostrarErrorCampoObligatorio(lbErrorCantidad);
                esCantidadValida = false;
            }
            else if (cantidadInsumo >= 0)
            {
                // TODO: Cambiar le toString por getName cuando se tenga el objeto producto:
                if (cbxUnidadMedida.SelectedItem.ToString() == "Unidad")
                {
                    esCantidadValida = ValidarCantidadUnitaria(cantidadInsumo);
                }
            }
            else
            {
                MostrarEtiquetaError(lbErrorCantidad, "Datos no válidos. Debe ser positiva la cantidad");
                esCantidadValida = false;
            }

            return esCantidadValida;
        }

        private float ConvertirStringAFloat(string numeroEnString, Label lbError)
        {
            float numero = -1;

            try
            {
                numero = Convert.ToSingle(numeroEnString);
            }
            catch (FormatException)
            {
                MostrarEtiquetaError(lbError, "Solo puede incluir números. Ej. 2, .5");
            }
            catch (OverflowException)
            {
                MostrarEtiquetaError(lbError, "Cantidad no permitida, por favor corrigelo.");
            }

            return numero;
        }

        private bool ValidarCantidadUnitaria(float cantidadInsumo)
        {
            bool esCantidadValida = true;
            
            if (cantidadInsumo % 1 != 0)
            {
                MostrarEtiquetaError(lbErrorCantidad, "Datos no válidos. Deben ser números enteros");
                esCantidadValida = false;
            }
            
            return esCantidadValida;
        }

        private bool ValidarCostoUnitarioInsumo()
        {
            bool esCostoValido = false;

            string costo = tbxCostoUnitario.Text.Trim();
            float costoUnitario = ConvertirStringAFloat(costo, lbErrorCostoUnitario);

            if (string.IsNullOrEmpty(costo))
            {
                MostrarErrorCampoObligatorio(lbErrorCostoUnitario);
                esCostoValido = false;
            }
            else if (costoUnitario >= 0)
            {
                esCostoValido = true;
            } 
            else
            {
                MostrarEtiquetaError(lbErrorCostoUnitario, "Datos no válidos. Debe ser positivo el número");
                esCostoValido = false;
            }

            return esCostoValido;
        }

        private bool ValidarRestriccionInsumo()
        {
            bool esRestriccionValida = false;
            
            string restriccion = tbxRestricciones.Text.Trim();
            esRestriccionValida = UtilidadValidacion.esRestriccionInsumoValida(restriccion);

            if (!esRestriccionValida)
            {
                MostrarEtiquetaError(lbErrorRestricciones, "Datos no válidos.");
            }

            return esRestriccionValida;
        }

        private bool ValidarDatosProductoVenta()
        {
            bool esProductoVentaValido = true;
            //TODO: REVISAR EL VALIDAR FOTO
            if (!ValidarPrecioProductoVenta())
            {
                esProductoVentaValido = false;
            }
            
            if(!ValidarFoto())
            {
                esProductoVentaValido = false;
            }

            return esProductoVentaValido;
        }

        private bool ValidarPrecioProductoVenta()
        {
            bool esPrecioValido = false;
            string precio = tbxPrecio.Text.Trim();
            float precioProductoVenta = ConvertirStringAFloat(precio, lbErrorPrecio);

            if (string.IsNullOrEmpty(precio))
            {
                MostrarErrorCampoObligatorio(lbErrorPrecio);
            }
            else if (precioProductoVenta > 0)
            {
                esPrecioValido = true;
            } 
            else
            {
                MostrarEtiquetaError(lbErrorPrecio, "Datos no válidos.");
            }

            return esPrecioValido;
        }

        // TODO ///////////////////////////////////////////////////////////////////////////////////
        private bool ValidarFoto()
        {
            return true;
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
                MessageBoxResult result = System.Windows.MessageBox.Show(
                    "Debes indicar si el producto está destinado a la venta o si es inventariado",
                    "Habilita una sección", MessageBoxButton.OK);
            }

            return haySeccionHabilitada;
        }
       
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(
                    "“¿Estás seguro de que deseas cancelar el registro del producto?",
                    "Cancelar Registro", MessageBoxButton.YesNo);
            if (result== MessageBoxResult.Yes)
            {
                Console.WriteLine("Go back and clear");
                LimpiarCampos();
                //NavigationService.GoBack();
            }
        }

        private void LimpiarCampos()
        {
            tbxCodigo.Text = string.Empty;
            tbxNombre.Text = string.Empty;
            tbxDescripcion.Text = string.Empty;
            cbxCategoria.SelectedIndex = 0;

            chbxEsInventariado.IsChecked = false;
            tbxCantidad.Text = string.Empty;
            tbxCostoUnitario.Text = string.Empty;
            tbxRestricciones.Text = string.Empty;
            cbxUnidadMedida.SelectedIndex = 0;

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
            Console.WriteLine("Clicked");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                // TODO: Guardar imagen
            }
        }
    }
}
