using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para RegistroCorteCaja.xaml
    /// </summary>
    public partial class RegistroCorteCaja : Window
    {
        private double _ingresosPedidos = 0;
        private double _salidasOrdenesCompra = 0;
        private double _salidasGastosVarios = 0;
        private DateTime _fechaSeleccionada;
        private double _fondoInicial = 0;
        private double _efectivoEsperado = 0;
        private double _dineroCaja = 0;
        private double _diferencia = 0;
        private bool _existeCorteCaja = false;
        private const int VENTANA_INFORMACION = 2;
        private readonly Window _mainWindow;
        private Frame _frameNavigator;

        //TODO: EXCEPCIONES
        public RegistroCorteCaja(Frame frameNavigator)
        {
            InitializeComponent();
            _mainWindow = Application.Current.MainWindow;
            ConfigurarVentana(frameNavigator);
            _fechaSeleccionada = DateTime.Now;
            this.Loaded += PrepararDatos;
        }

        private void PrepararDatos(object sender, RoutedEventArgs e)
        {
            lblFechaActual.Content = _fechaSeleccionada.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));
            _ingresosPedidos = RecuperarIngresosDePedidosPorFecha(_fechaSeleccionada);
            MostrarIngresos(_ingresosPedidos);
            _salidasOrdenesCompra = RecuperarSalidasDeOrdenesCompraPorFecha(_fechaSeleccionada);
            _salidasGastosVarios = RecuperarSalidasGastosVarios(_fechaSeleccionada);
            MostrarSalidas(_salidasGastosVarios, _salidasOrdenesCompra);
            _existeCorteCaja = ValidarExistenciaCorteCaja(_fechaSeleccionada);
            if (_existeCorteCaja)
            {
                CorteCaja corte = RecuperarCorteCaja(_fechaSeleccionada);
                _fondoInicial = corte.fondo;
                _dineroCaja = corte.dineroEnCaja;
            } 
            else
            {
                _fondoInicial = 0;
                _dineroCaja = 0;
            }
            TbxFondoInicial.Text = _fondoInicial.ToString("F2");
            TbxDineroCaja.Text = _dineroCaja.ToString("F2");
            CalcularYMostrarEfectivoEsperado();
            CalcularYMostrarDiferencia();
        }

        private bool ValidarExistenciaCorteCaja(DateTime fechaSeleccionada)
        {
            ServicioCorteCajaClient servicioCorteCajaCliente = new ServicioCorteCajaClient();
            return servicioCorteCajaCliente.ExisteCorteCaja(fechaSeleccionada);
        }

        private CorteCaja RecuperarCorteCaja(DateTime fecha)
        {
            ServicioCorteCajaClient servicioCorteCajaCliente = new ServicioCorteCajaClient();
            return servicioCorteCajaCliente.RecuperarCorteCaja(fecha);
        }

        private double RecuperarIngresosDePedidosPorFecha(DateTime fecha)
        {
            ServicioPedidosClient servicioPedidos = new ServicioPedidosClient();
            return servicioPedidos.RecuperarIngresosDePedidosPorFecha(fecha);
        }

        private double RecuperarSalidasDeOrdenesCompraPorFecha(DateTime fecha)
        {
            ServicioOrdenesCompraClient servicioOrdenesCompra = new ServicioOrdenesCompraClient();
            return servicioOrdenesCompra.RecuperarSalidasDeOrdenesCompraPorFecha(fecha);
        }

        private double RecuperarSalidasGastosVarios(DateTime fecha)
        {
            ServicioGastosVariosClient servicioGastosVarios = new ServicioGastosVariosClient();
            return servicioGastosVarios.RecuperarSalidasGastosVarios(fecha);
        }

        private void MostrarIngresos(double ingresosPedidos)
        {
            lblIngresosPedidos.Content = "$" + ingresosPedidos.ToString("F2");
        }

        private void MostrarSalidas(double gastosVarios, double salidasOrdenesCompra)
        {
            lblSalidas.Content = "$" + (gastosVarios + salidasOrdenesCompra).ToString("F2");
        }

        private void CalcularYMostrarEfectivoEsperado()
        {
            _efectivoEsperado = _fondoInicial + _ingresosPedidos - (_salidasGastosVarios + _salidasOrdenesCompra);
            lblEfectivoEsperado.Content = "$" + _efectivoEsperado.ToString("F2");
        }

        private void CalcularYMostrarDiferencia()
        {
            _diferencia = _efectivoEsperado - _dineroCaja;
            lblDiferencia.Content = "$" + _diferencia.ToString("F2");
        }

        private void TbxFondoInicial_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCampoObligatorioFondoInicial.Visibility = Visibility.Hidden;
            TextBox campo = (TextBox)sender;   
            if (!string.IsNullOrWhiteSpace(campo.Text))
            {
                _fondoInicial = double.Parse(campo.Text);
            } else
            {
                _fondoInicial = 0;
            }
            CalcularYMostrarEfectivoEsperado();
            CalcularYMostrarDiferencia();
        }

        private void TbxDineroCaja_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCampoObligatorioDineroEnCaja.Visibility = Visibility.Hidden;
            TextBox campo = (TextBox)sender;
            if (!string.IsNullOrWhiteSpace(campo.Text))
            {
                _dineroCaja = double.Parse(campo.Text);
            } 
            else
            {
                _dineroCaja = 0;
            }
            CalcularYMostrarDiferencia();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            double result;
            TextBox textBox = sender as TextBox;
            string fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            bool isDecimal = Double.TryParse(fullText, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            if (!isDecimal || fullText.Equals(""))
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void BtnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            bool camposLlenos = validarCamposLlenos();
            if (camposLlenos)
            {
                GuardarCorteCaja();
            } else
            {
                MostrarMensajeCamposObligatorios();
            }
        }

        private void MostrarMensajeCamposObligatorios()
        {
            if (string.IsNullOrWhiteSpace(TbxDineroCaja.Text))
            {
                lblCampoObligatorioDineroEnCaja.Visibility = Visibility.Visible;
            }
            if (string.IsNullOrWhiteSpace(TbxFondoInicial.Text))
            {
                lblCampoObligatorioFondoInicial.Visibility = Visibility.Visible;
            }
        }

        bool validarCamposLlenos()
        {
            bool camposLlenos = false;
            camposLlenos = !(string.IsNullOrWhiteSpace(TbxDineroCaja.Text)) && !(string.IsNullOrWhiteSpace(TbxFondoInicial.Text));
            return camposLlenos;
        }

        private void GuardarCorteCaja()
        {
            CorteCaja corte = new CorteCaja()
            {
                fondo = _fondoInicial,
                ingresosRegistrados = _ingresosPedidos,
                salidasRegistradas = (_salidasGastosVarios + _salidasOrdenesCompra),
                dineroEnCaja = _dineroCaja,
                diferencia = _diferencia,
                fecha = _fechaSeleccionada,
                nombreUsuario = EmpleadoSingleton.getInstance().NombreUsuario
            };
            ServicioCorteCajaClient servicioCorteCajaCliente = new ServicioCorteCajaClient();
            int registroExitoso;
            if (_existeCorteCaja)
            {
                registroExitoso = servicioCorteCajaCliente.ActualizarCorteCaja(corte);
            } else
            {
                registroExitoso = servicioCorteCajaCliente.GuardarCorteCaja(corte);
            }

            if (registroExitoso > 0)
            {
                ManejarRegistroExitoso();
            }
        }

        private void ManejarRegistroExitoso()
        {
            string tituloExito = "Corte de caja registrado";
            string mensajeExito = "¡Corte de caja realizado con éxito!";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloExito, mensajeExito, Window.GetWindow(this), VENTANA_INFORMACION);
            ventanaEmergente.ShowDialog();

            this.Close();
        }

        private void FechaDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime fechaSeleccionada = (DateTime)(sender as DatePicker).SelectedDate;
            bool fechaValida = ValidarFechaSeleccionada(fechaSeleccionada);
            if (fechaValida == false)
            {

            } else
            {
                _fechaSeleccionada = fechaSeleccionada;
                PrepararDatos(sender, e);
            }
        }

        private bool ValidarFechaSeleccionada(DateTime fechaSeleccionada)
        {
            bool fechaValida = false;
            fechaValida = fechaSeleccionada.Date <= DateTime.Now.Date;
            return fechaValida;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfigurarVentana(Frame frameNavigator)
        {
            _frameNavigator = frameNavigator;
            this.Owner = _mainWindow;
            SetSizeWindow();
            SetCenterWindow();
        }

        private void SetSizeWindow()
        {
            this.Width = _mainWindow.Width;
            this.Height = _mainWindow.Height;
        }

        private void SetCenterWindow()
        {
            double centerX = _mainWindow.Left + (_mainWindow.Width - this.Width) / 2;
            double centerY = _mainWindow.Top + (_mainWindow.Height - this.Height) / 2;
            this.Left = centerX;
            this.Top = centerY;
        }
    }
}
