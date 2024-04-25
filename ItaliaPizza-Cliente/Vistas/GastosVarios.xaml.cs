using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para GastosVarios.xaml
    /// </summary>
    public partial class GastosVarios : Window
    {
        private const int VENTANA_INFORMACION = 2;
        private readonly Window _windowOrigen;

        public GastosVarios(Window windowOrigen)
        {
            InitializeComponent();
            _windowOrigen = windowOrigen;
            ConfigurarVentana();
            CargarFechaActual();
            CargarUsuario();
        }

        private void CargarFechaActual()
        {
            MostrarFecha(DateTime.Now);
        }

        private void CargarUsuario()
        {
            EmpleadoSingleton empleado = EmpleadoSingleton.getInstance();
            lblNombreUsuario.Content = empleado.NombreUsuario;
        }

        private void MostrarFecha(DateTime fechaActual)
        {
            dpkFechaGasto.SelectedDate = fechaActual;
            lblFechaActual.Content = fechaActual.ToString();
        }

        private void DpkFechaGasto_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (dpkFechaGasto.SelectedDate != null)
            {
                MostrarFecha(dpkFechaGasto.SelectedDate.Value);
            }
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            bool esRegistroValido = ValidarRegistro();

            if (esRegistroValido)
            {
                RegistrarGastoVario();
            }
        }

        private void RegistrarGastoVario()
        {
            GastoVario gastoVario = CrearGastoVario();
            int filasAfectadas = -1;

            try
            {
                ServicioGastosVariosClient servicioGastosVarios = new ServicioGastosVariosClient();
                filasAfectadas = servicioGastosVarios.GuardarGastoVario(gastoVario);

                if (filasAfectadas > 0)
                {
                    ManejarRegistroExitoso();
                }
            }
            catch (EndpointNotFoundException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, _windowOrigen, this);
            }
            catch (TimeoutException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, _windowOrigen, this);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, _windowOrigen, this);
            }
            catch (FaultException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, _windowOrigen, this);
            }
            catch (CommunicationException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, _windowOrigen, this);
            }
            catch (Exception ex)
            {
                VentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, _windowOrigen, this);
            }
        }

        private GastoVario CrearGastoVario()
        {
            GastoVario gastoVario = new GastoVario
            {
                Fecha = dpkFechaGasto.SelectedDate.Value,
                Monto = Utilidad.ConvertirStringAFloat(tbxMonto.Text.Trim(), lblErrorMonto),
                Descripcion = tbxDescripcion.Text.Trim(),
                NombreUsuario = lblNombreUsuario.Content.ToString()
            };

            return gastoVario;
        }

        private void ManejarRegistroExitoso()
        {
            string tituloExito = "Gasto registrado";
            string mensajeExito = "Gasto registrado con éxito";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloExito, mensajeExito, Window.GetWindow(this), VENTANA_INFORMACION);
            ventanaEmergente.ShowDialog();

            this.Close();
        }

        private void EntradaSoloNumeros(object sender, TextCompositionEventArgs e)
        {
            if (!float.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private bool ValidarRegistro()
        {
            bool esRegistroValido = false;

            LimpiarErrores();
            bool hayCamposVacios = ValidarCamposVacios();

            if (!hayCamposVacios)
            {
                bool esMontoValido = ValidarMonto();
                bool esDescripcionValida = ValidarDescripcion();

                if (esMontoValido && esDescripcionValida)
                {
                    esRegistroValido = true;
                }
            }

            return esRegistroValido;
        }

        private bool ValidarCamposVacios()
        {
            bool hayCamposVacios = false;

            string monto = tbxMonto.Text.Trim();
            string descripcion = tbxDescripcion.Text.Trim();

            if (string.IsNullOrEmpty(monto))
            {
                hayCamposVacios = true;
                MostrarMensajeError("Campo obligatorio", lblErrorMonto);
            }

            if (string.IsNullOrEmpty(descripcion))
            {
                hayCamposVacios = true;
                MostrarMensajeError("Campo obligatorio", lblErrorDescripcion);
            }

            return hayCamposVacios;
        }

        private void LimpiarErrores()
        {
            lblErrorMonto.Visibility = Visibility.Collapsed;
            lblErrorMonto.Content = string.Empty;
            lblErrorDescripcion.Visibility = Visibility.Collapsed;
            lblErrorDescripcion.Content = string.Empty;
        }

        private void MostrarMensajeError(string mensaje, Label label)
        {
            label.Content = mensaje;
            label.Visibility = Visibility.Visible;
        }

        private bool ValidarMonto()
        {
            bool esMontoValido = true;
            string monto = tbxMonto.Text.Trim();
            float montoFloat = Utilidad.ConvertirStringAFloat(monto, lblErrorMonto);

            if (montoFloat <= 0)
            {
                esMontoValido = false;
                MostrarMensajeError("Monto no válido", lblErrorMonto);
            }

            return esMontoValido;
        }

        private bool ValidarDescripcion()
        {
            bool esDescripcionValida = true;
            string descripcion = tbxDescripcion.Text.Trim();

            if (!UtilidadValidacion.EsDescripcionProductoValida(descripcion))
            {
                esDescripcionValida = false;
                MostrarMensajeError("Descripción no válida", lblErrorDescripcion);
            }

            return esDescripcionValida;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfigurarVentana()
        {
            SetSizeWindow();
            SetCenterWindow();
        }

        private void SetSizeWindow()
        {
            this.Width = _windowOrigen.Width;
            this.Height = _windowOrigen.Height;
        }

        private void SetCenterWindow()
        {
            double centerX = _windowOrigen.Left + (_windowOrigen.Width - this.Width) / 2;
            double centerY = _windowOrigen.Top + (_windowOrigen.Height - this.Height) / 2;
            this.Left = centerX;
            this.Top = centerY;
        }
    }
}
