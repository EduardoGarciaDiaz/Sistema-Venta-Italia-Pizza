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
        private List<CampoTextoConLabel> camposDeDatos;
        private bool ventanaAnteriorEsConsultar;


        public RegistroProveedor(bool ventanaAnteriorEsConsultar)
        {
            InitializeComponent();
            this.ventanaAnteriorEsConsultar = ventanaAnteriorEsConsultar;
            this.Loaded += PrepararVentana;
        }


        private void PrepararVentana(object sender, RoutedEventArgs e)
        {
            camposDeDatos = new List<CampoTextoConLabel>()
            {
                new CampoTextoConLabel(txbNombre,lblNombreError), new CampoTextoConLabel(txbRfc,lblRfcError), new CampoTextoConLabel(txbTelefono,lblTelefonoError),
                new CampoTextoConLabel(txbCorreo,lblCorreoError),  new CampoTextoConLabel(txbCiudad,lblCiudadError),  new CampoTextoConLabel(txbColonia,lblColoniaError),
                new CampoTextoConLabel(txbCalle,lblCalleError),  new CampoTextoConLabel(txbCodigoPostal,lblCodigoError),  new CampoTextoConLabel(txbNumeroExterior,lblNumeroExtError)
            };
        }

        private void EntryJustInteger(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void BtnGuardarProveedor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardarProveedor();
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

        private void GuardarProveedor()
        {
            bool sePuedeGuardar;
            sePuedeGuardar = ValidarCamposLlenosUsuario();
            if (sePuedeGuardar)
            {
                sePuedeGuardar = ValidarFormatosDeCampos();
                if (sePuedeGuardar)
                {
                    string rfc = txbRfc.Text.Trim();
                    string correo = txbCorreo.Text.Trim();
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
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Registro Exitoso", "Se ha guardado correctamente el proveedor nuevo.", Window.GetWindow(this), 2);
                    ventanaEmergente.ShowDialog();
                    LimpiarCampos();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Upss!!", "Ocurrio un error al guardar el proveedor, intentelo mas tarde.", Window.GetWindow(this), 1);
                    ventanaEmergente.ShowDialog();
                }                               
            }
        }

        private bool ValidarCamposLlenosUsuario()
        {
            bool camposLlenos = true;
            foreach (var campo in camposDeDatos)
            {
                if (!RevisarCampoVacio(campo.textBox.Text.Trim(), campo.labelError, CAMPO_VACIO)) { camposLlenos = false; }
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

        private bool ValidarFormatosDeCampos()
        {
            bool formatosValidos = true;
            if (!Regex.IsMatch(txbCorreo.Text.Trim().ToLower(), EMAIL_RULES_CHAR) || !Regex.IsMatch(txbCorreo.Text.Trim().ToLower(), EMAIL_ALLOW_CHAR))
            {
                lblCorreoError.Content = CORREO_INVALIDO;
                formatosValidos = false;
            }
            else
            {
                lblCorreoError.Content = String.Empty;
            }
            if (txbTelefono.Text.Length != 10)
            {
                lblTelefonoError.Content = TELEFONO_INVALIDO;
                formatosValidos = false;
            }
            else
            {
                lblTelefonoError.Content = String.Empty;
            }
            if (!Regex.IsMatch(txbRfc.Text.Trim(), RFC_FORMATT))
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
                Ciudad = txbCiudad.Text.Trim(),
                Colonia = txbColonia.Text.Trim(),
                Calle = txbCalle.Text.Trim(),
                CodigoPostal = txbCodigoPostal.Text.Trim(),
                Numero = int.Parse(txbNumeroExterior.Text.Trim())
            };
            return new ProveedorDto()
            {
                IdProveedor = 0,
                NombreCompleto = txbNombre.Text.Trim(),
                RFC = txbRfc.Text.Trim(),
                NumeroTelefono = txbTelefono.Text.Trim(),
                CorreoElectronico = txbCorreo.Text.Trim(),
                EsActivo = true,
                Direccion = direccionProveedor
            };
        }

        private void LimpiarCampos()
        {
            txbNombre.Text = String.Empty;
            txbRfc.Text = String.Empty;
            txbTelefono.Text = String.Empty;
            txbCorreo.Text = String.Empty;
            txbCiudad.Text = String.Empty;
            txbColonia.Text = String.Empty;
            txbCalle.Text = String.Empty;
            txbCodigoPostal.Text = String.Empty;
            txbNumeroExterior.Text = String.Empty;
        }


        private void BtnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensajeConfirmacion();
        }

        private void MostrarMensajeConfirmacion()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("Cuidado!!!", "¿Seguro que desea cancelar el registro?, se perderán los datos del proveedor?", "Si, Cancelar Registro", "No, Cancelar Accion", Window.GetWindow(this), 3);
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
            if (ventanaAnteriorEsConsultar)
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
