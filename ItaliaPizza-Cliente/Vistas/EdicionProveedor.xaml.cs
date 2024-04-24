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
    /// Interaction logic for EdicionProveedor.xaml
    /// </summary>
    public partial class EdicionProveedor : Page
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
        private readonly ProveedorDto _proveedorSeleccionado;


        public EdicionProveedor(ProveedorDto proveedorSeleccionado)
        {
            InitializeComponent();
            this.Loaded += PreparaVentana;
            CargarInformacionEnCampos(proveedorSeleccionado);
            this._proveedorSeleccionado = proveedorSeleccionado;
        }

        private void TxbPermitirSoloEnteros(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void BtnCancelarEdicion_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensajeConfirmacion();
        }

        private void BtnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardarProveedor();
            }
            catch (EndpointNotFoundException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (TimeoutException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (CommunicationException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (Exception ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
        }

        private void CargarInformacionEnCampos(ProveedorDto proveedorSeleccionado)
        {
            tbxNombre.Text = proveedorSeleccionado.NombreCompleto.ToString();
            tbxRfc.Text = proveedorSeleccionado.RFC.ToString();
            tbxTelefono.Text = proveedorSeleccionado.NumeroTelefono;
            tbxCorreo.Text = proveedorSeleccionado.CorreoElectronico.ToString();
            tbxCiudad.Text = proveedorSeleccionado.Direccion.Ciudad.ToString();
            tbxColonia.Text = proveedorSeleccionado.Direccion.Colonia.ToString();
            tbxCalle.Text = proveedorSeleccionado.Direccion.Calle.ToString();
            tbxCodigoPostal.Text = proveedorSeleccionado.Direccion.CodigoPostal.ToString();
            tbxNumeroExterior.Text = proveedorSeleccionado.Direccion.Numero.ToString();
        }

        private void PreparaVentana(object sender, RoutedEventArgs e)
        {
            _camposDeDatos = new List<CampoTextoConLabel>()
            {
                new CampoTextoConLabel(tbxNombre,lblNombreError), new CampoTextoConLabel(tbxRfc,lblRfcError), new CampoTextoConLabel(tbxTelefono,lblTelefonoError),
                new CampoTextoConLabel(tbxCorreo,lblCorreoError),  new CampoTextoConLabel(tbxCiudad,lblCiudadError),  new CampoTextoConLabel(tbxColonia,lblColoniaError),
                new CampoTextoConLabel(tbxCalle,lblCalleError),  new CampoTextoConLabel(tbxCodigoPostal,lblCodigoError),  new CampoTextoConLabel(tbxNumeroExterior,lblNumeroExtError)
            };
        }

      
        private void GuardarProveedor()
        {
            bool sePuedeGuardar;
            sePuedeGuardar = ValidarCamposVacios();
            if (sePuedeGuardar)
            {
                sePuedeGuardar = ValidarFormatosDeCampos();
                if (sePuedeGuardar)
                {
                    string rfc = tbxRfc.Text.Trim();
                    string correo = tbxCorreo.Text.Trim();
                    sePuedeGuardar = ValidarCamposUnicos(rfc, correo, _proveedorSeleccionado.IdProveedor);
                }
            }
            if (sePuedeGuardar)
            {
                ProveedorDto proveedorNuevo = CrearObjetoProveedor();
                ServicioProveedoresClient servicioProveedoresClient = new ServicioProveedoresClient();
                bool fueGuardado = servicioProveedoresClient.ActualizarInformacionProveedor(proveedorNuevo);
                if (fueGuardado)
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Cambios Guardados", "Los datos del proveedor fueron actualizados correctamente.", Window.GetWindow(this), 2);
                    ventanaEmergente.ShowDialog();
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Upss!!", "Ocurrio un error al guardar el proveedor, intentelo mas tarde.", Window.GetWindow(this), 1);
                    ventanaEmergente.ShowDialog();
                }
            }
        }

        private bool ValidarCamposVacios()
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

        private bool ValidarFormatosDeCampos()
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

        private bool ValidarCamposUnicos(string rfc, string correo, int idProveedor)
        {
            bool sonUnicos = true;
            ServicioProveedoresClient servicioProveedoresClient = new ServicioProveedoresClient();
            if (!servicioProveedoresClient.ValidarRfcUnicoProveedorEditado(rfc, idProveedor))
            {
                sonUnicos = false;
                lblRfcError.Content = RFC_REPETIDO;
            }
            else
            {
                lblRfcError.Content = String.Empty;
            }
            if (!servicioProveedoresClient.ValidarCorreoUnicoProveedorEditado(correo, idProveedor))
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
            DireccionDto direccionProveedor = new DireccionDto()
            {
                IdDireccion = _proveedorSeleccionado.IdDireccion,
                Ciudad = tbxCiudad.Text.Trim(),
                Colonia = tbxColonia.Text.Trim(),
                Calle = tbxCalle.Text.Trim(),
                CodigoPostal = tbxCodigoPostal.Text.Trim(),
                Numero = int.Parse(tbxNumeroExterior.Text.Trim())
            };
            return new ProveedorDto()
            {
                IdProveedor = _proveedorSeleccionado.IdProveedor,
                NombreCompleto = tbxNombre.Text.Trim(),
                RFC = tbxRfc.Text.Trim(),
                NumeroTelefono = tbxTelefono.Text.Trim(),
                CorreoElectronico = tbxCorreo.Text.Trim(),
                EsActivo = _proveedorSeleccionado.EsActivo,
                IdDireccion = _proveedorSeleccionado.IdDireccion,
                Direccion = direccionProveedor
            };
        }      

        private void MostrarMensajeConfirmacion()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("Cuidado!!!", "¿Seguro que desea cancelar la edición?, se perderán los datos del proveedor que no se hayan guardado?", "Si, Cancelar", "No, Continuar editando", Window.GetWindow(this), 3);
            ventanaEmergente.ShowDialog();
            if (ventanaEmergente.AceptarAccion)
            {
                CerrarVentana();
            }
        }

        private void CerrarVentana()
        {
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);            
            ConsultaProveedores paginaConsultaProveedores = new ConsultaProveedores();
            ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaConsultaProveedores);
            ventanaPrincipal.FrameNavigator.NavigationService.RemoveBackEntry();
        }

    }
}
