using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
using static System.Net.Mime.MediaTypeNames;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Interaction logic for RegistroUsuario.xaml
    /// </summary>
    public partial class RegistroUsuario : Page
    {
        List<TipoEmpleadoDto> tiposEmpleados = new List<TipoEmpleadoDto>();
        private const string CAMPO_VACIO = "* Campo obligatorio";
        private const string CORREO_INVALIDO = "* Correo no valido";
        private const string TELEFONO_INVALIDO = "* Telefono no valido";
        private const string NOMBRE_USUARIO_REPETIDO = "El nombre de usuario capturado ya existe, ingrese uno que no exista.";
        private const string CORREO_REPETIDO ="El correo capturado ya existe, ingrese uno que no exista.";
        private readonly string EMAIL_RULES_CHAR = "^(?=.{1,90}$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        private readonly string EMAIL_ALLOW_CHAR = "^[a-zA-Z0-9@,._=]{1,90}$";

        public RegistroUsuario()
        {
            InitializeComponent();
            PrepareWindow();
        }

        private void PrepareWindow()
        {
            ObtenerTiposEmpleados();
            if ((int)EnumTiposEmpleado.Cajero == EmpleadoSingleton.getInstance().DatosEmpleado.IdTipoEmpleado)
            {
                rdbCliente.IsChecked = true;
                rdbCliente.IsEnabled = false;
                rdbEmpleado.IsEnabled = false;
            }
        }

        private void BloquearCampos()
        {            
            var empleadoMesero = tiposEmpleados.Where(emp => emp.IdTipoEmpleado == (int)EnumTiposEmpleado.Mesero);
            cbmTipoEmpleado.SelectedItem = empleadoMesero;
            cbmTipoEmpleado.IsEditable = false;
            txbNombre.Text = "Empleado Mesero";
            txb1erApellido.Text = "_";
            txbCorreo.Text = "_";
            txbTelefono.Text = "0000000000";
            txbCiudad.Text = "Local";
            txbColonia.Text = "Local";
            txbCalle.Text = "Local";
            txbCodigoPostal.Text = "0000";
            txbNumeroExterior.Text = "00";
            txbNombre.Text = "Empleado Mesero";
            txb1erApellido.Text = "_";
            txbCorreo.Text = "_";
            txbTelefono.Text = "0000000000";
            txbCiudad.Text = "Local";
            txbColonia.Text = "Local";
            txbCalle.Text = "Local";
            txbCodigoPostal.Text = "0000";
            txbNumeroExterior.Text = "00";
            txb2doApellido.IsEnabled = false;
        }

        private void ObtenerTiposEmpleados()
        {
            try
            {
                ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
                tiposEmpleados = proxyServicioUsuariosClient.RecuperarTiposEmpleado().ToList();
                cbmTipoEmpleado.ItemsSource = tiposEmpleados;
                cbmTipoEmpleado.DisplayMemberPath = "Nombre";
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

        private bool ValidarCamposLlenosUsuario()
        {
            bool camposLlenos = true;
            List<CampoTextoConLabel> camposDeDatos = new List<CampoTextoConLabel>()
            {
                new CampoTextoConLabel(txbNombre,lblNombreError), new CampoTextoConLabel(txb1erApellido,lbl1erApellidoError), new CampoTextoConLabel(txbTelefono,lblTelefonoError), 
                new CampoTextoConLabel(txbCorreo,lblCorreoError),  new CampoTextoConLabel(txbCiudad,lblCiudadError),  new CampoTextoConLabel(txbColonia,lblColoniaError),  
                new CampoTextoConLabel(txbCalle,lblCalleError),  new CampoTextoConLabel(txbCodigoPostal,lblCodigoError),  new CampoTextoConLabel(txbNumeroExterior,lblNumeroExtError)
            };
            foreach (var campo in camposDeDatos)
            {
                if(!RevisarCampoVacio(campo.textBox.Text.Trim(), campo.labelError, CAMPO_VACIO)) {camposLlenos = false; }
            }
            if(rdbEmpleado.IsChecked == false && rdbCliente.IsChecked == false)
            {
                 lblTipoUsuarioError.Content = CAMPO_VACIO;
                camposLlenos = false;
            }
            else
            {
                lblTipoUsuarioError.Content = String.Empty;
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

        private bool ValidarCamposLLenosEmpleado()
        {
            bool camposLlenos = true;
            if (String.IsNullOrEmpty(txbNombreUsuario.Text.Trim()))
            {
                lblNombreUsuarioError.Content = CAMPO_VACIO;
                camposLlenos = false;
            }
            else
            {
                lblNombreUsuarioError.Content = String.Empty;
            }
            if (String.IsNullOrEmpty(txbContrasena.Password.ToString().Trim()))
            {
                lblContrasena.Content = CAMPO_VACIO;
                camposLlenos = false;
            }
            else
            {
                lblContrasena.Content = String.Empty;
            }
            if (cbmTipoEmpleado.SelectedItem == null)
            {
                lblTipoEmpleadoError.Content = CAMPO_VACIO;
                camposLlenos = false;
            }
            else
            {
                lblTipoEmpleadoError.Content = String.Empty;
            }
            return camposLlenos;
        }

        private bool ValidarFormatos()
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
            if(txbTelefono.Text.Length != 10)
            {
                lblTelefonoError.Content = TELEFONO_INVALIDO;
                formatosValidos = false;
            }
            else
            {
                lblTelefonoError.Content = String.Empty;
            }
            return formatosValidos;
        }

        private void EntryJustInteger(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private class CampoTextoConLabel
        {
            public TextBox textBox { get; set; }
            public Label labelError { get; set; }

            public CampoTextoConLabel(TextBox textBox, Label labelError)
            {
                this.textBox = textBox;
                this.labelError = labelError;
            }
        }

        private bool ValidarCamposUnicos()
        {
            bool sonUnicos = true;
            try
            {
                ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
                if (!servicioUsuariosClient.ValidarNombreDeUsuarioUnico(txbNombreUsuario.Text.Trim()))
                {
                    sonUnicos = false;
                    lblNombreUsuarioError.Content = NOMBRE_USUARIO_REPETIDO;
                }
                else
                {
                    lblNombreUsuarioError.Content = String.Empty;
                }
                if (!servicioUsuariosClient.ValidarCorreoUnico(txbCorreo.Text.Trim().ToLower()))
                {
                    sonUnicos = false;
                    lblCorreoError.Content = CORREO_REPETIDO;
                }
                else
                {
                    lblCorreoError.Content = String.Empty;

                }
            }
            catch (EndpointNotFoundException ex)
            {
                sonUnicos = false;
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (TimeoutException ex)
            {
                sonUnicos = false;
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                sonUnicos = false;
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException ex)
            {
                sonUnicos = false;
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (CommunicationException ex)
            {
                sonUnicos = false;
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (Exception ex)
            {
                sonUnicos = false;
                VentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }            
            return sonUnicos;
        }

        private bool GuardarCliente(UsuarioDto usuarioNuevo)
        {
            ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
            return proxyServicioUsuariosClient.GuardarCliente(usuarioNuevo);
        }

        private bool GuardarEmpleado(EmpleadoDto empleadoNuevo)
        {
            ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
            return proxyServicioUsuariosClient.GuardarEmpleado(empleadoNuevo);
        }

        private void MostrarMensajeGuardadoConExito(bool exito)
        {
            if (exito)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Registro Exitoso", "Se ha guardado correctamente al usuario nuevo.", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                LimpiarCampos();
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error ", "Ocurrio un error al guardar al usuario nuevo.", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }
        }

        private void LimpiarCampos()
        {
            txbNombre.Text = String.Empty;
            txb1erApellido.Text = String.Empty;
            txb2doApellido.Text = String.Empty;
            txbTelefono.Text = String.Empty;
            txbCorreo.Text = String.Empty;
            rdbEmpleado.IsChecked = false;
            rdbCliente.IsChecked = false;
            rdbEmpleado.Background = new SolidColorBrush(Colors.WhiteSmoke);
            rdbCliente.Background = new SolidColorBrush(Colors.WhiteSmoke);
            txbCiudad.Text = String.Empty;
            txbColonia.Text = String.Empty;
            txbCalle.Text = String.Empty;
            txbCodigoPostal.Text = String.Empty;
            txbNumeroExterior.Text = String.Empty;
            txbNombreUsuario.Text = String.Empty;
            txbContrasena.Password = String.Empty;
            cbmTipoEmpleado.SelectedIndex = -1;
            brdCoverDatosEmpleado.Visibility = Visibility.Visible;
        }

        private void GuardarUsuario()
        {
            bool sePuedeGuardar;
            bool esEmpleado = (bool)rdbEmpleado.IsChecked;
            sePuedeGuardar = ValidarCamposLlenosUsuario();
            if (esEmpleado)
            {
                sePuedeGuardar = ValidarCamposLLenosEmpleado();
            }
            if (sePuedeGuardar)
            {
                sePuedeGuardar = ValidarFormatos();
                if (esEmpleado && sePuedeGuardar)
                {
                    sePuedeGuardar = ValidarCamposUnicos();
                }
            }
            if (sePuedeGuardar)
            {
                DireccionDto direccion = CrearObjetoDireccion();
                UsuarioDto usuarioNuevo = CrearObjetoUsuario(direccion);
                try
                {
                    if (esEmpleado)
                    {
                        EmpleadoDto empleadoNuevo = CrearObjetoEmpleado(usuarioNuevo);
                        bool fueGuardado = GuardarEmpleado(empleadoNuevo);
                        MostrarMensajeGuardadoConExito(fueGuardado);
                    }
                    else
                    {
                        bool fueGuardado = GuardarCliente(usuarioNuevo);
                        MostrarMensajeGuardadoConExito(fueGuardado);
                    }
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
        }

        private DireccionDto CrearObjetoDireccion()
        {
            return new DireccionDto()
            {
                IdDireccion = 0,
                Ciudad = txbCiudad.Text.Trim(),
                Colonia = txbColonia.Text.Trim(),
                Calle = txbCalle.Text.Trim(),
                CodigoPostal = txbCodigoPostal.Text.Trim(),
                Numero = int.Parse(txbNumeroExterior.Text.Trim())
            };
        }

        private UsuarioDto CrearObjetoUsuario(DireccionDto direccionNueva)
        {
            return new UsuarioDto()
            {
                IdUsuario = 0,
                NombreCompleto = txbNombre.Text.Trim() + " " + txb1erApellido.Text.Trim() + " " + txb2doApellido.Text.Trim(),
                NumeroTelefono = txbTelefono.Text.Trim(),
                CorreoElectronico = txbCorreo.Text.Trim().ToLower(),
                EsActivo = true,
                IdDireccion = 0,
                Direccion = direccionNueva
            };
        }

        private EmpleadoDto CrearObjetoEmpleado(UsuarioDto usuarioNuevo)
        {
            return new EmpleadoDto()
            {
                NombreUsuario = txbNombreUsuario.Text.Trim(),
                Contraseña = txbContrasena.Password.Trim(),
                IdTipoEmpleado = (cbmTipoEmpleado.SelectedItem as TipoEmpleadoDto).IdTipoEmpleado,
                TipoEmpleado = (cbmTipoEmpleado.SelectedItem as TipoEmpleadoDto).Nombre,
                IdUsuario = 0,
                Usuario = usuarioNuevo
            };
        }

        private void BtnGuardarUsuario_Click(object sender, MouseButtonEventArgs e)
        {
            GuardarUsuario();
        }
        private void BtnGuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            GuardarUsuario();
        }

        private void MostrarMensajeConfirmacion()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("Cuidado!!!", "¿Seguro que desea cancelar el registro?, se perderán los datos del usuario?", "Si, Cancelar Registro", "No, Cancelar Accion", Window.GetWindow(this), 3);
            ventanaEmergente.ShowDialog();
            if (ventanaEmergente.AceptarAccion)
            {
                LimpiarCampos();
                Usuarios paginaUsuarios = new Usuarios();
                MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
                ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaUsuarios);
            }
        }

        private void BtnCancelarRegistro_Click(object sender, MouseButtonEventArgs e)
        {
           MostrarMensajeConfirmacion();
        }

        private void BtnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensajeConfirmacion();
        }

        private void rdbEmpleado_Checked(object sender, RoutedEventArgs e)
        {
            brdCoverDatosEmpleado.Visibility = Visibility.Collapsed;
            rdbEmpleado.Background = new SolidColorBrush(Colors.Black);
            rdbCliente.Background = new SolidColorBrush(Colors.WhiteSmoke);
        }

        private void rdbCliente_Checked(object sender, RoutedEventArgs e)
        {
            brdCoverDatosEmpleado.Visibility =Visibility.Visible;
            rdbEmpleado.Background = new SolidColorBrush(Colors.WhiteSmoke);
            rdbCliente.Background = new SolidColorBrush(Colors.Black);
            txbNombreUsuario.Text = string.Empty;
            txbContrasena.Password = string.Empty;
        }

        private void bttVerContrasena_Click(object sender, MouseButtonEventArgs e)
        {
            lblContrasenaVer.Content = txbContrasena.Password.ToString();
            txbContrasena.Visibility = Visibility.Hidden;
            lblContrasenaVer.Visibility = Visibility.Visible;
        }

        private void bttVerContrasena_Leave(object sender, MouseEventArgs e)
        {
            if(lblContrasenaVer != null && lblContrasenaVer.IsVisible)
            {
                txbContrasena.Password = lblContrasenaVer.Content.ToString();
                txbContrasena.Visibility = Visibility.Visible;
                lblContrasenaVer.Visibility = Visibility.Hidden;
            }
        }

    }
    
}
