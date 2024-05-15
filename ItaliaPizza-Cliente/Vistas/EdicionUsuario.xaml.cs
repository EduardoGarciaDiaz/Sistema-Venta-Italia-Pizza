using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
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
    public partial class EdicionUsuario : Page
    {
        List<TipoEmpleadoDto> tiposEmpleados = new List<TipoEmpleadoDto>();
        private const int VENTANA_ERROR = 1;
        private const int VENTANA_INFORMACION = 2;
        private const int VENTANA_CONFIRMACION = 3;
        private const string CAMPO_VACIO = "* Campo obligatorio";
        private const string CORREO_INVALIDO = "* Correo no válido";
        private const string TELEFONO_INVALIDO = "* Teléfono no válido";
        private const string NOMBRE_USUARIO_REPETIDO = "El nombre de Usuario capturado ya existe, ingrese uno que no exista.";
        private const string CORREO_REPETIDO ="El correo capturado ya existe, ingrese uno que no exista.";
        private const string CODIGO_POSTAL_INVALIDO = "* Código postal no válido";
        private const string CORREO_VALIDO_REGEX = "^(?=.{1,90}$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        private const string CORREO_PERMITIDO_REGEX = "^[a-zA-Z0-9@,._=]{1,90}$";
        private const string CODIGO_POSTAL_REGEX = "^[0-9]{5}$";
        private const char CARACTER_SEPARADOR = ' ';
        private EmpleadoDto _empleadoEdicion;
        private UsuarioDto _usuarioEdicion;

        public EdicionUsuario()
        {
            InitializeComponent();
            this.Loaded += PrepararVentana;
        }

        public EdicionUsuario(EmpleadoDto empleado)
        {
            InitializeComponent();
            this.Loaded += PrepararVentana;
            _empleadoEdicion = empleado;
        }

        public EdicionUsuario(UsuarioDto usuario)
        {
            InitializeComponent();
            this.Loaded += PrepararVentana;
            _usuarioEdicion = usuario;
        }

        private void PrepararVentana(object sender, RoutedEventArgs e)
        {
            ObtenerTiposEmpleados();

            if (_empleadoEdicion != null)
            {
                CargarDatosUsuario(_empleadoEdicion.Usuario);
            }
            
            if (_usuarioEdicion != null)
            {
                CargarDatosUsuario(_usuarioEdicion);
            }


            txbTelefono.PreviewKeyDown += UtilidadValidacion.EntradaTextl_KeyDown;
            txbTelefono.PreviewMouseRightButtonUp += UtilidadValidacion.MouseClicDerecho_Click;
            txbCodigoPostal.PreviewKeyDown += UtilidadValidacion.EntradaTextl_KeyDown;
            txbCodigoPostal.PreviewMouseRightButtonUp += UtilidadValidacion.MouseClicDerecho_Click;
            txbNumeroExterior.PreviewKeyDown += UtilidadValidacion.EntradaTextl_KeyDown;
            txbNumeroExterior.PreviewMouseRightButtonUp += UtilidadValidacion.MouseClicDerecho_Click;
        }

        private void CargarDatosUsuario(UsuarioDto usuarioEdicion)
        {
            int indice_nombre = 0;
            int indice_1erApellido = 1;
            int indice_2doApellido = 2;

            txbNombre.Text = usuarioEdicion.NombreCompleto.Split(CARACTER_SEPARADOR)[indice_nombre];
            txb1erApellido.Text = usuarioEdicion.NombreCompleto.Split(CARACTER_SEPARADOR)[indice_1erApellido];
            txb2doApellido.Text = usuarioEdicion.NombreCompleto.Split(CARACTER_SEPARADOR)[indice_2doApellido];
            txbTelefono.Text = usuarioEdicion.NumeroTelefono;
            txbCorreo.Text = usuarioEdicion.CorreoElectronico;

            if (_empleadoEdicion != null)
            {
                rdbEmpleado.IsChecked = true;
                cbmTipoEmpleado.SelectedItem = tiposEmpleados.Find(tipo => tipo.IdTipoEmpleado == _empleadoEdicion.IdTipoEmpleado);
                txbNombreUsuario.Text = _empleadoEdicion.NombreUsuario;
            }
            else
            {
                rdbCliente.IsChecked = true;
            }

            txbCiudad.Text = usuarioEdicion.Direccion.Ciudad;
            txbColonia.Text = usuarioEdicion.Direccion.Colonia;
            txbCalle.Text = usuarioEdicion.Direccion.Calle;
            txbCodigoPostal.Text = usuarioEdicion.Direccion.CodigoPostal;
            txbNumeroExterior.Text = usuarioEdicion.Direccion.Numero.ToString();
        }
       

        private void ObtenerTiposEmpleados()
        {
            try
            {
                ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
                tiposEmpleados = proxyServicioUsuariosClient.RecuperarTiposEmpleado().ToList();
                CargarTiposEnLista(tiposEmpleados);
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

        private void CargarTiposEnLista(List<TipoEmpleadoDto> tiposEmpleadosRecuperados)
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

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LimpiarMensajesError();
                ActualizarUsuario();
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

        private void ActualizarUsuario()
        {
            bool sePuedeGuardar;
            bool esEmpleado = (bool)rdbEmpleado.IsChecked;
            sePuedeGuardar = ValidarCamposLlenosUsuario();

            if (esEmpleado && sePuedeGuardar)
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
                sePuedeGuardar = ValidarDatosDireccion();
            }

            if (sePuedeGuardar)
            {
                DireccionDto direccion = CrearObjetoDireccion();
                UsuarioDto usuarioNuevo = CrearObjetoUsuario(direccion);
                if (esEmpleado)
                {
                    EmpleadoDto empleadoNuevo = CrearObjetoEmpleado(usuarioNuevo);
                    ActualizarEmpleado(empleadoNuevo);
                    MostrarMensajeExito();
                }
                else
                {
                    ActualizarCliente(usuarioNuevo);
                    MostrarMensajeExito();
                }
            }
        }

        private bool ValidarDatosDireccion()
        {
            bool datosValidos = true;
            if (!Regex.IsMatch(txbCodigoPostal.Text.Trim(), CODIGO_POSTAL_REGEX))
            {
                lblCodigoError.Content = CODIGO_POSTAL_INVALIDO;
                datosValidos = false;
            }

            if (!ValidarCamposLlenosDireccion())
            {
                datosValidos = false;
            }

            return datosValidos;
        }

        private bool ValidarCamposLlenosDireccion()
        {
            bool datosLlenos = true;
            List<CampoTextoConLabel> camposDeDireccion = new List<CampoTextoConLabel>()
            {
                new CampoTextoConLabel(txbCiudad,lblCiudadError), new CampoTextoConLabel(txbColonia,lblColoniaError), new CampoTextoConLabel(txbCalle,lblCalleError), 
                new CampoTextoConLabel(txbCodigoPostal,lblCodigoError), new CampoTextoConLabel(txbNumeroExterior,lblNumeroExtError)
            };

            foreach (var campo in camposDeDireccion)
            {
                if (!RevisarCampoVacio(campo.TextBox.Text.Trim(), campo.LabelError, CAMPO_VACIO)) { datosLlenos = false; }
            }

            return datosLlenos;
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
                if (!RevisarCampoVacio(campo.TextBox.Text.Trim(), campo.LabelError, CAMPO_VACIO)) {camposLlenos = false; }
            }

            if (rdbEmpleado.IsChecked == false && rdbCliente.IsChecked == false)
            {
                 lblTipoUsuarioError.Content = CAMPO_VACIO;
                camposLlenos = false;
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

            if (chbxActualizarContraseña.IsChecked == true
                && String.IsNullOrEmpty(txbContrasena.Password.ToString().Trim()))
            {
                lblContrasena.Content = CAMPO_VACIO;
                camposLlenos = false;
            }

            if (cbmTipoEmpleado.SelectedItem == null)
            {
                lblTipoEmpleadoError.Content = CAMPO_VACIO;
                camposLlenos = false;
            }

            return camposLlenos;
        }

        private bool ValidarFormatosDeCampos()
        {
            bool formatosValidos = true;

            if (!Regex.IsMatch(txbCorreo.Text.Trim().ToLower(), CORREO_VALIDO_REGEX) || !Regex.IsMatch(txbCorreo.Text.Trim().ToLower(), CORREO_PERMITIDO_REGEX))
            {
                lblCorreoError.Content = CORREO_INVALIDO;
                formatosValidos = false;
            }

            if (txbTelefono.Text.Length != 10)
            {
                lblTelefonoError.Content = TELEFONO_INVALIDO;
                formatosValidos = false;
            }

            return formatosValidos;
        }


        private bool ValidarCamposUnicos()
        {
            bool sonUnicos = true;
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();

            if (txbContrasena.IsEnabled)
            {
                if (!servicioUsuariosClient.ValidarActualizacionNombreDeUsuarioUnico(txbNombreUsuario.Text.Trim(), _empleadoEdicion.IdUsuario))
                {
                    sonUnicos = false;
                    lblNombreUsuarioError.Content = NOMBRE_USUARIO_REPETIDO;
                }
            }

            if (!servicioUsuariosClient.ValidarActualizacionCorreoUnico(txbCorreo.Text.Trim().ToLower(), _empleadoEdicion.IdUsuario))
            {
                sonUnicos = false;
                lblCorreoError.Content = CORREO_REPETIDO;
            } 
            
            return sonUnicos;
        }

        private void LimpiarMensajesError()
        {
            lblCodigoError.Content = String.Empty;
            lblCiudadError.Content = String.Empty;
            lblColoniaError.Content = String.Empty;
            lblCalleError.Content = String.Empty;
            lblNumeroExtError.Content = String.Empty;
            lblTipoUsuarioError.Content = String.Empty;
            lblContrasena.Content = String.Empty;
            lblTipoEmpleadoError.Content = String.Empty;
            lblTipoEmpleadoError.Content = String.Empty;
            lblTelefonoError.Content = String.Empty;
            lblNombreUsuarioError.Content = String.Empty;
            lblCorreoError.Content = String.Empty;
        }

        private DireccionDto CrearObjetoDireccion()
        {
            DireccionDto direccion = new DireccionDto();

            if (_empleadoEdicion != null)
            {
                direccion.IdDireccion = _empleadoEdicion.Usuario.IdDireccion;
            }
            else
            {
                direccion.IdDireccion = _usuarioEdicion.IdDireccion;
            }

            direccion.Ciudad = txbCiudad.Text.Trim();
            direccion.Colonia = txbColonia.Text.Trim();
            direccion.Calle = txbCalle.Text.Trim();
            direccion.CodigoPostal = txbCodigoPostal.Text.Trim();
            direccion.Numero = int.Parse(txbNumeroExterior.Text.Trim());

            return direccion;
        }

        private UsuarioDto CrearObjetoUsuario(DireccionDto direccionNueva)
        {
            UsuarioDto usuario = new UsuarioDto();

            if (_empleadoEdicion != null)
            {
                usuario.IdUsuario = _empleadoEdicion.Usuario.IdUsuario;
                usuario.IdDireccion = _empleadoEdicion.Usuario.IdDireccion;
                usuario.EsActivo = _empleadoEdicion.Usuario.EsActivo;
            }
            else
            {
                usuario.IdUsuario = _usuarioEdicion.IdUsuario;
                usuario.IdDireccion = _usuarioEdicion.IdDireccion;
                usuario.EsActivo = _usuarioEdicion.EsActivo;
            }

            usuario.NombreCompleto = txbNombre.Text.Trim() + " " + txb1erApellido.Text.Trim() + " " + txb2doApellido.Text.Trim();
            usuario.NumeroTelefono = txbTelefono.Text.Trim();
            usuario.CorreoElectronico = txbCorreo.Text.Trim().ToLower();
            usuario.Direccion = direccionNueva;

            return usuario;
        }

        private EmpleadoDto CrearObjetoEmpleado(UsuarioDto usuarioNuevo)
        {
            EmpleadoDto empleadoActualizado = new EmpleadoDto()
            {
                NombreUsuario = txbNombreUsuario.Text.Trim(),
                Contraseña = _empleadoEdicion.Contraseña,
                IdTipoEmpleado = (cbmTipoEmpleado.SelectedItem as TipoEmpleadoDto).IdTipoEmpleado,
                TipoEmpleado = (cbmTipoEmpleado.SelectedItem as TipoEmpleadoDto).Nombre,
                IdUsuario = _empleadoEdicion.IdUsuario,
                Usuario = usuarioNuevo
            };

            if (chbxActualizarContraseña.IsChecked == true)
            {
                empleadoActualizado.Contraseña = CifradorContraseñas.EncriptarContraseña(txbContrasena.Password.Trim());
            }

            return empleadoActualizado;
        }

        private bool ActualizarEmpleado(EmpleadoDto empleadoNuevo)
        {
            ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
            bool esActualzado = proxyServicioUsuariosClient.ActualizarEmpleado(empleadoNuevo);

            return esActualzado;
        }

        private bool ActualizarCliente(UsuarioDto usuarioNuevo)
        {
            ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
            bool esActualizado = proxyServicioUsuariosClient.ActualizarCliente(usuarioNuevo);

            return esActualizado;
        }   

        private void MostrarMensajeExito()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("¡Éxito!", "Modificación exitosa", Window.GetWindow(this), VENTANA_INFORMACION);
            ventanaEmergente.ShowDialog();
            LimpiarCampos();
            NavigationService.GoBack();
        }

        private void MostrarMensajeError()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("Error ", "Ocurrió un error al guardar los cambios", Window.GetWindow(this), VENTANA_ERROR);
            ventanaEmergente.ShowDialog();            
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

        private void ChbxActualizarContrasena_Checked(object sender, RoutedEventArgs e)
        {
            if (chbxActualizarContraseña.IsChecked == true)
            {
                rectangleBloqueoContrasena.Visibility = Visibility.Collapsed;
            }
        }

        private void ChbxActualizarContrasena_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chbxActualizarContraseña.IsChecked == false)
            {
                rectangleBloqueoContrasena.Visibility = Visibility.Visible;
            }
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
            txbContrasena.Password = "Mesero";
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
            VentanaEmergente ventanaEmergente = new VentanaEmergente("Cancelar edición", "¿Estás seguro de que desea cancelar la modificación del Usuario?", "Sí", "No", Window.GetWindow(this), VENTANA_CONFIRMACION);
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

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensajeConfirmacion();
        }
    }
    
}
