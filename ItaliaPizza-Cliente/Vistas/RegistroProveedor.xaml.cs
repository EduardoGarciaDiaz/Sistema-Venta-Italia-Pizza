using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RegistroProveedor.xaml
    /// </summary>
    public partial class RegistroProveedor : Page
    {
        private const string CAMPO_VACIO = "* Campo obligatorio";
        private const string CORREO_INVALIDO = "* Correo no valido";
        private const string TELEFONO_INVALIDO = "* Telefono no valido";
        private const string RFC_INVALIDO = "* RFC no valido";
        private const string RFC_REPETIDO = "El RFC capturado ya existe, ingrese uno que no exista.";
        private const string CORREO_REPETIDO = "El correo capturado ya existe, ingrese uno que no exista.";
        private readonly string EMAIL_RULES_CHAR = "^(?=.{1,90}$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        private readonly string EMAIL_ALLOW_CHAR = "^[a-zA-Z0-9@,._=]{1,90}$";
        private readonly string RFC_FORMATT = @"^[A-Z&Ñ]{3,4}\d{6}[A-Z\d]{3}$";
        private List<CampoTextoConLabel> _camposDeDatos;
        private readonly bool _ventanaAnteriorEsConsultar;


        public RegistroProveedor(bool ventanaAnteriorEsConsultar)
        {
            InitializeComponent();
            this._ventanaAnteriorEsConsultar = ventanaAnteriorEsConsultar;
            this.Loaded += PrepararVentana;
        }


        private void PrepararVentana(object sender, RoutedEventArgs e)
        {
            _camposDeDatos = new List<CampoTextoConLabel>()
            {
                new CampoTextoConLabel(tbxNombre,lblNombreError), new CampoTextoConLabel(tbxRfc,lblRfcError), new CampoTextoConLabel(tbxTelefono,lblTelefonoError),
                new CampoTextoConLabel(tbxCorreo,lblCorreoError),  new CampoTextoConLabel(tbxCiudad,lblCiudadError),  new CampoTextoConLabel(tbxColonia,lblColoniaError),
                new CampoTextoConLabel(tbxCalle,lblCalleError),  new CampoTextoConLabel(tbxCodigoPostal,lblCodigoError),  new CampoTextoConLabel(tbxNumeroExterior,lblNumeroExtError)
            };
            tbxNumeroExterior.PreviewKeyDown += UtilidadValidacion.EntradaTextl_KeyDown;
            tbxNumeroExterior.PreviewMouseRightButtonUp += UtilidadValidacion.MouseClicDerecho_Click;
            tbxTelefono.PreviewKeyDown += UtilidadValidacion.EntradaTextl_KeyDown;
            tbxTelefono.PreviewMouseRightButtonUp += UtilidadValidacion.MouseClicDerecho_Click;
            tbxCodigoPostal.PreviewKeyDown += UtilidadValidacion.EntradaTextl_KeyDown;
            tbxCodigoPostal.PreviewMouseRightButtonUp += UtilidadValidacion.MouseClicDerecho_Click;
        }

        private void EntryJustInteger(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void BtnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensajeConfirmacion();
        }

        private void BtnGuardarProveedor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardarProveedor();
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

        private void GuardarProveedor()
        {
            bool sePuedeGuardar;
            sePuedeGuardar = ValidarCamposLlenos();
            if (sePuedeGuardar)
            {
                sePuedeGuardar = ValidarFormatos();
                if (sePuedeGuardar)
                {
                    string rfc = tbxRfc.Text.Trim();
                    string correo = tbxCorreo.Text.Trim();
                    sePuedeGuardar = ValidarCamposUnicos(rfc, correo);
                }
            }
            if (sePuedeGuardar)
            {
                ProveedorDto proveedorNuevo = CrearObjetoProveedor();
                ServicioProveedoresClient servicioProveedoresClient = new ServicioProveedoresClient();
                bool fueGuardado = servicioProveedoresClient.GuardarProveedorNuevo(proveedorNuevo);
                if (fueGuardado)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Registro exitoso", "Se ha guardado correctamente el proveedor nuevo.", Window.GetWindow(this), 2);
                    ventanaEmergente.ShowDialog();
                    LimpiarCampos();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("¡Ups!", "Ocurrió un error al guardar el proveedor, intentelo mas tarde.", Window.GetWindow(this), 1);
                    ventanaEmergente.ShowDialog();
                }                               
            }
        }

        private bool ValidarCamposLlenos()
        {
            bool camposLlenos = true;
            foreach (var campo in _camposDeDatos)
            {
                if (!RevisarCampoVacio(campo.TextBox.Text.Trim(), campo.LabelError, CAMPO_VACIO)) { camposLlenos = false; }
            }
            return camposLlenos;
        }

        private bool RevisarCampoVacio(String campoVerificar, Label labelDeCampo, string mensaje)
        {
            bool camposLlenos = true;
            if (String.IsNullOrEmpty(campoVerificar))
            {
                labelDeCampo.Content = mensaje;
                camposLlenos = false;
            }
            else
            {
                labelDeCampo.Content = String.Empty;
            }
            return camposLlenos;
        }

        private bool ValidarFormatos()
        {
            bool formatosValidos = true;
            if (!Regex.IsMatch(tbxCorreo.Text.Trim().ToLower(), EMAIL_RULES_CHAR) || !Regex.IsMatch(tbxCorreo.Text.Trim().ToLower(), EMAIL_ALLOW_CHAR))
            {
                lblCorreoError.Content = CORREO_INVALIDO;
                formatosValidos = false;
            }
            else
            {
                lblCorreoError.Content = String.Empty;
            }
            if (tbxTelefono.Text.Length != 10)
            {
                lblTelefonoError.Content = TELEFONO_INVALIDO;
                formatosValidos = false;
            }
            else
            {
                lblTelefonoError.Content = String.Empty;
            }
            if (!Regex.IsMatch(tbxRfc.Text.Trim(), RFC_FORMATT))
            {
                lblRfcError.Content = RFC_INVALIDO;
                formatosValidos = false;
            }
            else
            {
                lblRfcError.Content = String.Empty;
            }
            return formatosValidos;
        }

        private bool ValidarCamposUnicos(string rfc, string correo)
        {
            bool sonUnicos = true;
            ServicioProveedoresClient servicioProveedoresClient = new ServicioProveedoresClient();
            if (!servicioProveedoresClient.ValidarRfcUnicoProveedor(rfc))
            {
                sonUnicos = false;
                lblRfcError.Content = RFC_REPETIDO;
            }
            else
            {
                lblRfcError.Content = String.Empty;
            }
            if (!servicioProveedoresClient.ValidarCorreoUnicoProveedor(correo))
            {
                sonUnicos = false;
                lblCorreoError.Content = CORREO_REPETIDO;
            }
            else
            {
                lblCorreoError.Content = String.Empty;

            }
            return sonUnicos;
        }

        private ProveedorDto CrearObjetoProveedor()
        {
            DireccionDto direccionProveedor =  new DireccionDto()
            {
                IdDireccion = 0,
                Ciudad = tbxCiudad.Text.Trim(),
                Colonia = tbxColonia.Text.Trim(),
                Calle = tbxCalle.Text.Trim(),
                CodigoPostal = tbxCodigoPostal.Text.Trim(),
                Numero = int.Parse(tbxNumeroExterior.Text.Trim())
            };
            return new ProveedorDto()
            {
                IdProveedor = 0,
                NombreCompleto = tbxNombre.Text.Trim(),
                RFC = tbxRfc.Text.Trim(),
                NumeroTelefono = tbxTelefono.Text.Trim(),
                CorreoElectronico = tbxCorreo.Text.Trim(),
                EsActivo = true,
                Direccion = direccionProveedor
            };
        }

        private void LimpiarCampos()
        {
            tbxNombre.Text = String.Empty;
            tbxRfc.Text = String.Empty;
            tbxTelefono.Text = String.Empty;
            tbxCorreo.Text = String.Empty;
            tbxCiudad.Text = String.Empty;
            tbxColonia.Text = String.Empty;
            tbxCalle.Text = String.Empty;
            tbxCodigoPostal.Text = String.Empty;
            tbxNumeroExterior.Text = String.Empty;
        }


       private void MostrarMensajeConfirmacion()
       {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("¡Cuidado!", "¿Seguro que desea cancelar el registro? Se perderán los datos del proveedor?", "Sí, cancelar registro", "No, cancelar acción", Window.GetWindow(this), 3);
            ventanaEmergente.ShowDialog();
            if (ventanaEmergente.AceptarAccion)
            {
                LimpiarCampos();
                IrAventanaAnterior();
            }
        }

        private void IrAventanaAnterior()
        {
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
            if (_ventanaAnteriorEsConsultar)
            {
                ConsultaProveedores paginaConsultarProveedores =  new ConsultaProveedores();
                ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaConsultarProveedores);
            }
            else
            {
                RegistroOrdenCompra paginaRegistroOrdenCompra = new RegistroOrdenCompra();
                ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaRegistroOrdenCompra);
            }
        }

    }
}
