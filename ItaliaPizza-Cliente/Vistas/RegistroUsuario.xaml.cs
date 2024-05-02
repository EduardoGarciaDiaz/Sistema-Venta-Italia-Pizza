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
        private const string CAMPO_VACIO = "* Campo obligatorio";
        private const string CORREO_INVALIDO = "* Correo no válido";
        private const string TELEFONO_INVALIDO = "* Teléfono no válido";
        private const string NOMBRE_USUARIO_REPETIDO = "El nombre de Usuario capturado ya existe, ingrese uno que no exista.";
        private const string CORREO_REPETIDO ="El correo capturado ya existe, ingrese uno que no exista.";
        private readonly string EMAIL_RULES_CHAR = "^(?=.{1,90}$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        private readonly string EMAIL_ALLOW_CHAR = "^[a-zA-Z0-9@,._=]{1,90}$";
        private List<TipoEmpleadoDto> _tiposEmpleados = new List<TipoEmpleadoDto>();

        public RegistroUsuario()
        {
            InitializeComponent();
            this.Loaded += PrepararVentana;
        }

        private void PrepararVentana(object sender, RoutedEventArgs e)
        {
            ObtenerTiposEmpleados();           
        }

        private void EntryJustInteger(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void BtnVerContrasena_Click(object sender, MouseButtonEventArgs e)
        {
            MostrarContrasena();
        }
        private void CbxTipoEmpleado_Selected(object sender, SelectionChangedEventArgs e)
        {
            TipoEmpleadoDto tipoSeleccionado = (TipoEmpleadoDto)cbxTipoEmpleado.SelectedItem;
            if (tipoSeleccionado != null && tipoSeleccionado.IdTipoEmpleado == (int)EnumTiposEmpleado.Mesero)
            {
                BloquearCamposEmpleado();
            }
            else
            {
                DesBloquearCamposEmpleado();
            }
        }
        private void RdbEmpleado_Checked(object sender, RoutedEventArgs e)
        {
            HabilitarSeccionEmpleado();
        }
        private void RdbCliente_Checked(object sender, RoutedEventArgs e)
        {
            DeshabilitarSeccionEmpleado();
        }        

        private void BtnVerContrasena_Leave(object sender, MouseEventArgs e)
        {
            OcultarContrasena();
        }

        private void BtnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensajeConfirmacion();
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

        private void ObtenerTiposEmpleados()
        {
            try
            {
                ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
                _tiposEmpleados = proxyServicioUsuariosClient.RecuperarTiposEmpleado().ToList();
                CargarTipoesEnLista(_tiposEmpleados);
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
               
        private void CargarTipoesEnLista(List<TipoEmpleadoDto> tiposEmpleadosRecuperados)
        {
            cbxTipoEmpleado.ItemsSource = tiposEmpleadosRecuperados;
            cbxTipoEmpleado.DisplayMemberPath = "Nombre";
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
                new CampoTextoConLabel(tbxNombre,lblNombreError), new CampoTextoConLabel(tbx1erApellido,lbl1erApellidoError), new CampoTextoConLabel(tbxTelefono,lblTelefonoError), 
                new CampoTextoConLabel(tbxCorreo,lblCorreoError),  new CampoTextoConLabel(tbxCiudad,lblCiudadError),  new CampoTextoConLabel(tbxColonia,lblColoniaError),  
                new CampoTextoConLabel(tbxCalle,lblCalleError),  new CampoTextoConLabel(tbxCodigoPostal,lblCodigoError),  new CampoTextoConLabel(tbxNumeroExterior,lblNumeroExtError)
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
            if (String.IsNullOrEmpty(tbxNombreUsuario.Text.Trim()))
            {
                lblNombreUsuarioError.Content = CAMPO_VACIO;
                camposLlenos = false;
            }
            else
            {
                lblNombreUsuarioError.Content = String.Empty;
            }
            if (String.IsNullOrEmpty(pwbxContrasena.Password.ToString().Trim()))
            {
                lblContrasena.Content = CAMPO_VACIO;
                camposLlenos = false;
            }
            else
            {
                lblContrasena.Content = String.Empty;
            }
            if (cbxTipoEmpleado.SelectedItem == null)
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
            if (!Regex.IsMatch(tbxCorreo.Text.Trim().ToLower(), EMAIL_RULES_CHAR) || !Regex.IsMatch(tbxCorreo.Text.Trim().ToLower(), EMAIL_ALLOW_CHAR))
            {
                lblCorreoError.Content = CORREO_INVALIDO;
                formatosValidos = false;
            }
            else
            {
                lblCorreoError.Content = String.Empty;
            }
            if(tbxTelefono.Text.Length != 10)
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
            if (pwbxContrasena.IsEnabled)
            {
                if (!servicioUsuariosClient.ValidarNombreDeUsuarioUnico(tbxNombreUsuario.Text.Trim()))
                {
                    sonUnicos = false;
                    lblNombreUsuarioError.Content = NOMBRE_USUARIO_REPETIDO;
                }
                else
                {
                    lblNombreUsuarioError.Content = String.Empty;
                }
            }
            if (!servicioUsuariosClient.ValidarCorreoUnico(tbxCorreo.Text.Trim().ToLower()))
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
                Ciudad = tbxCiudad.Text.Trim(),
                Colonia = tbxColonia.Text.Trim(),
                Calle = tbxCalle.Text.Trim(),
                CodigoPostal = tbxCodigoPostal.Text.Trim(),
                Numero = int.Parse(tbxNumeroExterior.Text.Trim())
            };
        }

        private UsuarioDto CrearObjetoUsuario(DireccionDto direccionNueva)
        {
            return new UsuarioDto()
            {
                IdUsuario = 0,
                NombreCompleto = tbxNombre.Text.Trim() + " " + tbx1erApellido.Text.Trim() + " " + txb2doApellido.Text.Trim(),
                NumeroTelefono = tbxTelefono.Text.Trim(),
                CorreoElectronico = tbxCorreo.Text.Trim().ToLower(),
                EsActivo = true,
                IdDireccion = 0,
                Direccion = direccionNueva
            };
        }

        private EmpleadoDto CrearObjetoEmpleado(UsuarioDto usuarioNuevo)
        {
            return new EmpleadoDto()
            {
                NombreUsuario = tbxNombreUsuario.Text.Trim(),
                Contraseña = CifradorContraseñas.EncriptarContraseña(pwbxContrasena.Password.Trim()),
                IdTipoEmpleado = (cbxTipoEmpleado.SelectedItem as TipoEmpleadoDto).IdTipoEmpleado,
                TipoEmpleado = (cbxTipoEmpleado.SelectedItem as TipoEmpleadoDto).Nombre,
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
            tbxNombre.Text = String.Empty;
            tbx1erApellido.Text = String.Empty;
            txb2doApellido.Text = String.Empty;
            tbxTelefono.Text = String.Empty;
            tbxCorreo.Text = String.Empty;
            rdbEmpleado.IsChecked = false;
            rdbCliente.IsChecked = false;
            rdbEmpleado.Background = new SolidColorBrush(Colors.WhiteSmoke);
            rdbCliente.Background = new SolidColorBrush(Colors.WhiteSmoke);
            tbxCiudad.Text = String.Empty;
            tbxColonia.Text = String.Empty;
            tbxCalle.Text = String.Empty;
            tbxCodigoPostal.Text = String.Empty;
            tbxNumeroExterior.Text = String.Empty;
            tbxNombreUsuario.Text = String.Empty;
            pwbxContrasena.Password = String.Empty;
            cbxTipoEmpleado.SelectedIndex = -1;
            brdCoverDatosEmpleado.Visibility = Visibility.Visible;
        }

        
        private void HabilitarSeccionEmpleado()
        {
            brdCoverDatosEmpleado.Visibility = Visibility.Collapsed;
            rdbEmpleado.Background = new SolidColorBrush(Colors.Black);
            rdbCliente.Background = new SolidColorBrush(Colors.WhiteSmoke);
        }

        private void DeshabilitarSeccionEmpleado()
        {
            brdCoverDatosEmpleado.Visibility = Visibility.Visible;
            rdbEmpleado.Background = new SolidColorBrush(Colors.WhiteSmoke);
            rdbCliente.Background = new SolidColorBrush(Colors.Black);
            cbxTipoEmpleado.SelectedItem = null;
            DesBloquearCamposEmpleado();
        }

        private void BloquearCamposEmpleado()
        {
            tbxNombreUsuario.Text = "Mesero";
            tbxNombreUsuario.IsEnabled = false;
            pwbxContrasena.Password = "mesero";
            pwbxContrasena.IsEnabled = false;
        }

        private void DesBloquearCamposEmpleado()
        {
            tbxNombreUsuario.Text = String.Empty;
            tbxNombreUsuario.IsEnabled = true;
            pwbxContrasena.Password = String.Empty;
            pwbxContrasena.IsEnabled = true;
        }

        private void MostrarContrasena()
        {
            lblContrasenaVer.Content = pwbxContrasena.Password.ToString();
            pwbxContrasena.Visibility = Visibility.Hidden;
            lblContrasenaVer.Visibility = Visibility.Visible;
        }

        private void OcultarContrasena()
        {
            if (lblContrasenaVer != null && lblContrasenaVer.IsVisible)
            {
                pwbxContrasena.Password = lblContrasenaVer.Content.ToString();
                pwbxContrasena.Visibility = Visibility.Visible;
                lblContrasenaVer.Visibility = Visibility.Hidden;
            }
        }

        private void MostrarMensajeConfirmacion()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("¡Cuidado!", "¿Seguro que desea cancelar el registro? Se perderán los datos del Usuario", "Sí, cancelar registro", "No, cancelar acción", Window.GetWindow(this), 3);
            ventanaEmergente.ShowDialog();
            if (ventanaEmergente.AceptarAccion)
            {
                LimpiarCampos();
                MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
                if ((int)EnumTiposEmpleado.Cajero == EmpleadoSingleton.getInstance().DatosEmpleado.IdTipoEmpleado)
                {
                    PaginaDeInicio paginaDeIncio = new PaginaDeInicio();
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
