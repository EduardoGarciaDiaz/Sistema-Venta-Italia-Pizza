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
        private const string NOMBRE_USUARIO_REPETIDO = "El nombre de Usuario capturado ya existe, ingrese uno que no exista.";
        private const string CORREO_REPETIDO ="El correo capturado ya existe, ingrese uno que no exista.";
        private readonly string EMAIL_RULES_CHAR = "^(?=.{1,90}$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        private readonly string EMAIL_ALLOW_CHAR = "^[a-zA-Z0-9@,._=]{1,90}$";

        public RegistroUsuario()
        {
            InitializeComponent();
            this.Loaded += PrepararVentana;
        }

        private void PrepararVentana(object sender, RoutedEventArgs e)
        {
            ObtenerTiposEmpleados();           
        }
       

        private void ObtenerTiposEmpleados()
        {
            try
            {
                ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
                tiposEmpleados = proxyServicioUsuariosClient.RecuperarTiposEmpleado().ToList();
                CargarTipoesEnLista(tiposEmpleados);
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

        private void CargarTipoesEnLista(List<TipoEmpleadoDto> tiposEmpleadosRecuperados)
        {
            cbmTipoEmpleado.ItemsSource = tiposEmpleadosRecuperados;
            cbmTipoEmpleado.DisplayMemberPath = "Nombre";
        }

        private void EntryJustInteger(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void BtnGuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GuardarUsuario();
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

        private void GuardarUsuario()
        {
            bool sePuedeGuardar;
            bool esEmpleado = (bool)rdbEmpleado.IsChecked;
            sePuedeGuardar = ValidarCamposLlenosUsuario();
            if (sePuedeGuardar && esEmpleado)
            {
                sePuedeGuardar = ValidarCamposLLenosEmpleado();
            }
            if (sePuedeGuardar)
            {
                sePuedeGuardar = ValidarFormatosDeCampos();
                if (esEmpleado && sePuedeGuardar)
                {
                    sePuedeGuardar = ValidarCamposUnicos();
                }
            }
            if (sePuedeGuardar)
            {
                DireccionDto direccion = CrearObjetoDireccion();
                UsuarioDto usuarioNuevo = CrearObjetoUsuario(direccion);              
                if (esEmpleado)
                {
                    EmpleadoDto empleadoNuevo = CrearObjetoEmpleado(usuarioNuevo);
                    GuardarEmpleado(empleadoNuevo);
                    MostrarMensajeExito();
                }
                else
                {
                    GuardarCliente(usuarioNuevo);
                    MostrarMensajeExito();
                }                
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
                if(!RevisarCampoVacio(campo.TextBox.Text.Trim(), campo.LabelError, CAMPO_VACIO)) {camposLlenos = false; }
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


        private bool ValidarCamposUnicos()
        {
            bool sonUnicos = true;
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            if (txbContrasena.IsEnabled)
            {
                if (!servicioUsuariosClient.ValidarNombreDeUsuarioUnico(txbNombreUsuario.Text.Trim()))
                {
                    sonUnicos = false;
                    lblNombreUsuarioError.Content = NOMBRE_USUARIO_REPETIDO;
                }
                else
                {
                    lblNombreUsuarioError.Content = String.Empty;
                }
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
            return sonUnicos;
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
                Contraseña = CifradorContraseñas.EncriptarContraseña(txbContrasena.Password.Trim()),
                IdTipoEmpleado = (cbmTipoEmpleado.SelectedItem as TipoEmpleadoDto).IdTipoEmpleado,
                TipoEmpleado = (cbmTipoEmpleado.SelectedItem as TipoEmpleadoDto).Nombre,
                IdUsuario = 0,
                Usuario = usuarioNuevo
            };
        }

        private bool GuardarEmpleado(EmpleadoDto empleadoNuevo)
        {
            ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
            return proxyServicioUsuariosClient.GuardarEmpleado(empleadoNuevo);
        }

        private bool GuardarCliente(UsuarioDto usuarioNuevo)
        {
            ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
            return proxyServicioUsuariosClient.GuardarCliente(usuarioNuevo);
        }   

        private void MostrarMensajeExito()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("Registro Exitoso", "Se ha guardado correctamente al Usuario nuevo.", Window.GetWindow(this), 2);
            ventanaEmergente.ShowDialog();
            LimpiarCampos();           
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


        private void RdbEmpleado_Checked(object sender, RoutedEventArgs e)
        {
            HabilitarSeccionEmpleado();
        }

        private void HabilitarSeccionEmpleado()
        {
            brdCoverDatosEmpleado.Visibility = Visibility.Collapsed;
            rdbEmpleado.Background = new SolidColorBrush(Colors.Black);
            rdbCliente.Background = new SolidColorBrush(Colors.WhiteSmoke);
        }

        private void RdbCliente_Checked(object sender, RoutedEventArgs e)
        {
            DeshabilitarSeccionEmpleado();
        }

        private void DeshabilitarSeccionEmpleado()
        {
            brdCoverDatosEmpleado.Visibility = Visibility.Visible;
            rdbEmpleado.Background = new SolidColorBrush(Colors.WhiteSmoke);
            rdbCliente.Background = new SolidColorBrush(Colors.Black);
            cbmTipoEmpleado.SelectedItem = null;
            DesBloquearCamposEmpleado();
        }

        private void CmbTipoEmpleado_Selected(object sender, SelectionChangedEventArgs e)
        {
            TipoEmpleadoDto tipoSeleccionado = (TipoEmpleadoDto)cbmTipoEmpleado.SelectedItem;
            if (tipoSeleccionado != null && tipoSeleccionado.IdTipoEmpleado == (int)EnumTiposEmpleado.Mesero)
            {
                BloquearCamposEmpleado();
            }
            else
            {
                DesBloquearCamposEmpleado();
            }
        }

        private void BloquearCamposEmpleado()
        {
            txbNombreUsuario.Text = "Mesero";
            txbNombreUsuario.IsEnabled = false;
            txbContrasena.Password = "mesero";
            txbContrasena.IsEnabled = false;
        }

        private void DesBloquearCamposEmpleado()
        {
            txbNombreUsuario.Text = String.Empty;
            txbNombreUsuario.IsEnabled = true;
            txbContrasena.Password = String.Empty;
            txbContrasena.IsEnabled = true;
        }


        private void bttVerContrasena_Click(object sender, MouseButtonEventArgs e)
        {
            MostrarContrasena();
        }

        private void MostrarContrasena()
        {
            lblContrasenaVer.Content = txbContrasena.Password.ToString();
            txbContrasena.Visibility = Visibility.Hidden;
            lblContrasenaVer.Visibility = Visibility.Visible;
        }

        private void bttVerContrasena_Leave(object sender, MouseEventArgs e)
        {
            OcultarContrasena();
        }

        private void OcultarContrasena()
        {
            if (lblContrasenaVer != null && lblContrasenaVer.IsVisible)
            {
                txbContrasena.Password = lblContrasenaVer.Content.ToString();
                txbContrasena.Visibility = Visibility.Visible;
                lblContrasenaVer.Visibility = Visibility.Hidden;
            }
        }

        private void BtnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensajeConfirmacion();
        }

        private void MostrarMensajeConfirmacion()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("Cuidado!!!", "¿Seguro que desea cancelar el registro?, se perderán los datos del Usuario", "Si, Cancelar Registro", "No, Cancelar Accion", Window.GetWindow(this), 3);
            ventanaEmergente.ShowDialog();
            if (ventanaEmergente.AceptarAccion)
            {
                LimpiarCampos();
                MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
                if ((int)EnumTiposEmpleado.Cajero == EmpleadoSingleton.getInstance().DatosEmpleado.IdTipoEmpleado)
                {
                    PaginaDeIncio paginaDeIncio = new PaginaDeIncio();
                    ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaDeIncio);
                }
                else
                {
                    Usuarios paginaUsuarios = new Usuarios();
                    ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaUsuarios);
                }
            }
        }
    }
    
}
