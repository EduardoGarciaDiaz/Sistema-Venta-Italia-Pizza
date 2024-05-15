using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para RegistroReceta.xaml
    /// </summary>
    public partial class EdicionReceta : Page
    {
        private const string COLOR_HEXADECIMAL_ELEMENTO_DEFAULT = "#F6F6F6";
        private const string COLOR_HEXADECIMAL_INSUMO_SELECCIONADO = "#FF7B7B7B";
        private const int VENTANA_INFORMACION = 2;
        private const int VENTANA_CONFIRMACION = 3;
        private int _numeroInsumosSeleccionados = 0;
        private Receta _recetaEdicion;
        private InsumoRegistroReceta[] _insumosDisponibles;
        private List<InsumoReceta> _insumosSeleccionados = new List<InsumoReceta>();

        public EdicionReceta(Receta recetaEdicion)
        {
            InitializeComponent();
            _recetaEdicion = recetaEdicion;
            this.Loaded += EdicionReceta_Loaded;
        }

        private void EdicionReceta_Loaded(object sender, RoutedEventArgs e)
        {
            AgregarEventos();

            try
            {
                CargarRecetaEdicion();
                CargarInsumos();
                CargarInsumosReceta();
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

        private void TbxBusquedaInsumo_TextChanged(object sender, EventArgs e)
        {
            if (barraDeBusquedaInsumo.tbxBusqueda.Text.Trim() == string.Empty)
            {
                MostrarInsumos();
            }
        }

        private void ImgBuscarInsumo_Click(object sender, EventArgs e)
        {
            BuscarInsumo();
        }

        private void EnterInsumo_Pressed(object sender, EventArgs e)
        {
            if (e is KeyEventArgs keyEventArgs && keyEventArgs.Key == Key.Enter)
            {
                BuscarInsumo();
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            string tituloCancelar = "Cancelar Registro";
            string mensajeCancelar = "¿Estás seguro de que deseas cancelar el registro de la receta?";
            string contenidoBtnAceptar = "Sí";
            string contenidBtnCancelar = "No";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloCancelar, mensajeCancelar, contenidoBtnAceptar,
                contenidBtnCancelar, Window.GetWindow(this), VENTANA_CONFIRMACION);

            ventanaEmergente.ShowDialog();

            if (ventanaEmergente.AceptarAccion)
            {
                LimpiarCampos();
                NavigationService.GoBack();
            }
        }

        private void BtnDesasignarInsumo_Click(object sender, EventArgs e)
        {
            ElementoInsumoSeleccionado elementoInsumoSeleccionado = sender as ElementoInsumoSeleccionado;
            RemoverInsumoSeleccionado(elementoInsumoSeleccionado);
            HabilitarElementoInsumoRegistroReceta(elementoInsumoSeleccionado.InsumoAsignado.Codigo);
        }

        private void AgregarEventos()
        {
            barraDeBusquedaInsumo.Placeholder.Text = "Buscar insumo...";
            barraDeBusquedaInsumo.TbxBusquedaTextChanged += TbxBusquedaInsumo_TextChanged;
            barraDeBusquedaInsumo.ImgBuscarClicked += ImgBuscarInsumo_Click;
            barraDeBusquedaInsumo.EnterPressed += EnterInsumo_Pressed;
        }

        private void CargarRecetaEdicion()
        {
            lblCodigo.Content = _recetaEdicion.Codigo;
            tbkNombreReceta.Text = _recetaEdicion.Nombre;
            lblNombreReceta.Content = _recetaEdicion.Nombre;
            if (_recetaEdicion.FotoProducto != null)
            {
                BitmapImage foto = ConvertidorBytes.ConvertirBytesABitmapImage(_recetaEdicion.FotoProducto);
                imgFoto.Source = foto;
            }
        }

        private void CargarInsumos()
        {
            RecuperarInsumos();

            if (_insumosDisponibles != null && _insumosDisponibles.Count() > 0)
            {
                MostrarInsumos();

                foreach (ElementoInsumoRegistroReceta insumo in wrpPanelInsumos.Children)
                {
                    insumo.EsSeleccionado = false;
                }
            }
        }

        private void RecuperarInsumos()
        {
            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();

            _insumosDisponibles = servicioProductosCliente.RecuperarInsumos();
        }

        private void MostrarInsumos()
        {
            wrpPanelInsumos.Children.Clear();

            if (_insumosDisponibles != null && _insumosDisponibles.Count() > 0)
            {
                foreach (InsumoRegistroReceta insumo in _insumosDisponibles)
                {
                    MostrarInsumo(insumo);
                }
            }
        }

        private void MostrarInsumo(InsumoRegistroReceta insumo)
        {
            ElementoInsumoRegistroReceta elementoInsumoRegistroReceta = 
                CrearElementoInsumoRegistroReceta(insumo);

            if (_insumosSeleccionados != null)
            {
                if (_insumosSeleccionados.Any(insumobuscar => insumobuscar.Codigo == elementoInsumoRegistroReceta.InsumoAsignado.Codigo))
                {
                    elementoInsumoRegistroReceta.EsSeleccionado = true;
                    elementoInsumoRegistroReceta.rectangleInsumoRegistro.Fill = (SolidColorBrush) new BrushConverter()
                        .ConvertFromString(COLOR_HEXADECIMAL_INSUMO_SELECCIONADO);
                }
            }

            wrpPanelInsumos.Children.Add(elementoInsumoRegistroReceta);
        }

        private ElementoInsumoRegistroReceta CrearElementoInsumoRegistroReceta(InsumoRegistroReceta insumo)
        {
            ElementoInsumoRegistroReceta elementoInsumoRegistroReceta = 
                new ElementoInsumoRegistroReceta(insumo);

            elementoInsumoRegistroReceta.GrdInsumoRegistroRecetaClicked += GridInsumoRegistroReceta_Click;

            return elementoInsumoRegistroReceta;
        }

        private void GridInsumoRegistroReceta_Click(object sender, EventArgs e)
        {
            tbkErrorRegistro.Visibility = Visibility.Collapsed;

            ElementoInsumoRegistroReceta elementoInsumoRegistroReceta = sender as ElementoInsumoRegistroReceta;

            if (!elementoInsumoRegistroReceta.EsSeleccionado)
            {
                SeleccionarInsumo(elementoInsumoRegistroReceta);
                InsumoReceta insumoReceta = CrearInsumoRecetaDesdeElementoInsumoRegistro(elementoInsumoRegistroReceta);
                _insumosSeleccionados.Add(insumoReceta);
                MostrarInsumoSeleccionado(insumoReceta);
            }
        }

        private void CargarInsumosReceta()
        {
            RecuperarInsumosReceta();

            if (_insumosSeleccionados != null && _insumosSeleccionados.Count() > 0)
            {
                foreach (InsumoReceta insumoReceta in _insumosSeleccionados)
                {
                    MostrarInsumoSeleccionado(insumoReceta);
                    DeshabilitarElementoInsumoRegistroReceta(insumoReceta.Codigo);
                }
            }
        }

        private void DeshabilitarElementoInsumoRegistroReceta(string codigoInsumo)
        {
            ElementoInsumoRegistroReceta elementoInsumoRegistroReceta = null;

            foreach (var child in wrpPanelInsumos.Children)
            {
                if (child is ElementoInsumoRegistroReceta elemento && elemento.InsumoAsignado.Codigo == codigoInsumo)
                {
                    elementoInsumoRegistroReceta = elemento;
                    break; 
                }
            }

            if (elementoInsumoRegistroReceta != null)
            {
                SeleccionarInsumo(elementoInsumoRegistroReceta);
            }
            else
            {
                NavigationService.GoBack();
            }

        }

        private void RecuperarInsumosReceta()
        {
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();

            _insumosSeleccionados = servicioRecetasCliente.RecuperarInsumosReceta(_recetaEdicion.Id).ToList();
        }

        private void SeleccionarInsumo(ElementoInsumoRegistroReceta elementoInsumoRegistroReceta)
        {
            elementoInsumoRegistroReceta.EsSeleccionado = true;
            _numeroInsumosSeleccionados++;
            ActualizarContadorInsumos();

            elementoInsumoRegistroReceta.rectangleInsumoRegistro.Fill = (SolidColorBrush)new BrushConverter()
                .ConvertFromString(COLOR_HEXADECIMAL_INSUMO_SELECCIONADO);
        }

        private void ActualizarContadorInsumos()
        {
            lblContadorInsumos.Content = _numeroInsumosSeleccionados;
        }

        private InsumoReceta CrearInsumoRecetaDesdeElementoInsumoRegistro(ElementoInsumoRegistroReceta elementoInsumoRegistroReceta)
        {
            InsumoReceta insumoReceta = new InsumoReceta()
            {
                Codigo = elementoInsumoRegistroReceta.InsumoAsignado.Codigo,
                Nombre = elementoInsumoRegistroReceta.InsumoAsignado.Nombre,
                UnidadMedida = elementoInsumoRegistroReceta.InsumoAsignado.UnidadMedida
            };

            return insumoReceta;
        }

        private void MostrarInsumoSeleccionado(InsumoReceta insumoReceta)
        {
            ElementoInsumoSeleccionado elementoInsumoSeleccionado = CrearElementoInsumoSeleccionado(insumoReceta);

            wrpPanelInsumosSeleccionados.Children.Add(elementoInsumoSeleccionado);
        }

        private ElementoInsumoSeleccionado CrearElementoInsumoSeleccionado(InsumoReceta insumoReceta)
        {
            ElementoInsumoSeleccionado elementoInsumoSeleccionado = new ElementoInsumoSeleccionado(insumoReceta);
            elementoInsumoSeleccionado.InsumoAsignado = insumoReceta;
            string cantidad = insumoReceta.Cantidad.ToString();
            elementoInsumoSeleccionado.tbxCantidadInsumo.Text = cantidad;
            if (cantidad == "0")
            {
                elementoInsumoSeleccionado.tbxCantidadInsumo.Text = "1";
            }

            elementoInsumoSeleccionado.BtnDesasignarInsumoClicked += BtnDesasignarInsumo_Click;
            elementoInsumoSeleccionado.tbxCantidadInsumo.PreviewKeyDown += UtilidadValidacion.EntradaTextl_KeyDown;
            elementoInsumoSeleccionado.tbxCantidadInsumo.PreviewMouseRightButtonUp += UtilidadValidacion.MouseClicDerecho_Click;

            return elementoInsumoSeleccionado;
        }

        private void RemoverInsumoSeleccionado(ElementoInsumoSeleccionado elementoInsumoSeleccionado)
        {
            _numeroInsumosSeleccionados--;
            ActualizarContadorInsumos();

            foreach (ElementoInsumoSeleccionado insumoSeleccionado in wrpPanelInsumosSeleccionados.Children)
            {
                if (insumoSeleccionado.InsumoAsignado.Codigo == elementoInsumoSeleccionado.InsumoAsignado.Codigo)
                {
                    wrpPanelInsumosSeleccionados.Children.Remove(insumoSeleccionado);
                    break;
                }
            }
        }

        private void HabilitarElementoInsumoRegistroReceta(string codigo)
        {
            foreach (ElementoInsumoRegistroReceta insumo in wrpPanelInsumos.Children)
            {
                if (insumo.InsumoAsignado.Codigo == codigo)
                {
                    InsumoReceta insumoExistente = _insumosSeleccionados.FirstOrDefault(
                        i => i.Codigo == (string)insumo.lblCodigo.Content);

                    if (insumoExistente != null)
                    {
                        _insumosSeleccionados.Remove(insumoExistente);
                    }

                    insumo.EsSeleccionado = false;
                    insumo.rectangleInsumoRegistro.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(COLOR_HEXADECIMAL_ELEMENTO_DEFAULT);
                }
            }
        }

        private bool ValidarInsumoSeleccionado()
        {
            bool hayInsumoSeleccionado = false;
            int sinInsumos = 0;

            if (wrpPanelInsumosSeleccionados.Children.Count > sinInsumos)
            {
                hayInsumoSeleccionado = true;
            }

            return hayInsumoSeleccionado;
        }

        private void BtnGuardarReceta_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarActualizacionReceta())
            {
                int filasAfectadas = -1;

                RecetaProducto nuevaReceta = CrearReceta();

                filasAfectadas = ActualizarReceta(nuevaReceta);

                if (filasAfectadas > 0)
                {
                    ManejarEdicionExitosa();
                    NavigationService.GoBack();
                }
            }
        }

        private RecetaProducto CrearReceta()
        {
            InsumoReceta[] insumosSeleccionados = new InsumoReceta[_numeroInsumosSeleccionados];
            insumosSeleccionados = RecopilarInsumosSeleccionados(insumosSeleccionados);
            string codigoProductoReceta = _recetaEdicion.Codigo;

            RecetaProducto nuevaReceta = new RecetaProducto()
            {
                IdReceta = _recetaEdicion.Id,
                CodigoProducto = codigoProductoReceta,
                InsumosReceta = insumosSeleccionados
            };

            return nuevaReceta;
        }

        private InsumoReceta[] RecopilarInsumosSeleccionados(InsumoReceta[] insumosSeleccionados)
        {
            for (int i = 0; i < _numeroInsumosSeleccionados; i++)
            {
                ElementoInsumoSeleccionado insumoSeleccionado = wrpPanelInsumosSeleccionados.Children[i] as ElementoInsumoSeleccionado;

                insumoSeleccionado.InsumoAsignado.Cantidad = Utilidad
                    .ConvertirStringAFloat(insumoSeleccionado.tbxCantidadInsumo.Text, null);

                InsumoReceta insumoReceta = new InsumoReceta()
                {
                    Codigo = insumoSeleccionado.InsumoAsignado.Codigo,
                    Cantidad = insumoSeleccionado.InsumoAsignado.Cantidad
                };

                var insumoExistente = _insumosSeleccionados.FirstOrDefault(insumo => insumo.Codigo == insumoSeleccionado.InsumoAsignado.Codigo);
                if (insumoExistente != null)
                {
                    insumoReceta.Id = insumoExistente.Id;
                }

                insumosSeleccionados[i] = insumoReceta;
            }

            return insumosSeleccionados;
        }

        private int ActualizarReceta(RecetaProducto nuevaReceta)
        {
            int filasAfectadas = -1;
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();

            try
            {
                filasAfectadas = servicioRecetasCliente.ActualizarReceta(nuevaReceta);
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

            return filasAfectadas;
        }

        private void ManejarEdicionExitosa()
        {
            string tituloExito = "Receta modificada";
            string mensajeExito = "Receta modificada con éxito";

            LimpiarCampos();

            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloExito, mensajeExito, Window.GetWindow(this), VENTANA_INFORMACION);
            ventanaEmergente.ShowDialog();
        }

        private void LimpiarCampos()
        {
            _numeroInsumosSeleccionados = 0;
            _insumosSeleccionados = new List<InsumoReceta>();
            lblNombreReceta.Content = string.Empty;
            _recetaEdicion = null;
            ActualizarContadorInsumos();
            MostrarInsumos();
            wrpPanelInsumosSeleccionados.Children.Clear();
        }

        private bool ValidarActualizacionReceta()
        {
            bool esGuardadoValido = true;
            string mensajeErrorSinInsumo = "No puedes guardar una receta sin insumos";

            LimpiarMensajesError();

            if (!ValidarInsumoSeleccionado())
            {
                esGuardadoValido = false;
                Utilidad.MostrarTextoError(tbkErrorRegistro, mensajeErrorSinInsumo);
            }

            if (!ValidarCantidades())
            {
                esGuardadoValido = false;
            }

            return esGuardadoValido;
        }


        private bool ValidarCantidades()
        {
            bool sonCantidadesValidas = true;

            foreach(ElementoInsumoSeleccionado insumoSeleccionado in wrpPanelInsumosSeleccionados.Children)
            {
                string cantidad = insumoSeleccionado.tbxCantidadInsumo.Text.Trim();

                float cantidadInsumo = Utilidad.ConvertirStringAFloat(cantidad, insumoSeleccionado.lblErrorInsumoSeleccionado);

                string unidadMedida = (string)insumoSeleccionado.lblUnidadMedida.Content;
                sonCantidadesValidas = UtilidadValidacion.ValidarCantidadInsumo(
                    cantidadInsumo, unidadMedida, insumoSeleccionado.lblErrorInsumoSeleccionado);

                if (!sonCantidadesValidas)
                {
                    break;
                }
            }

            return sonCantidadesValidas;
        }

        private void LimpiarMensajesError()
        {
            tbkErrorRegistro.Visibility = Visibility.Collapsed;

            foreach(ElementoInsumoSeleccionado insumoSeleccionado in wrpPanelInsumosSeleccionados.Children)
            {
                insumoSeleccionado.lblErrorInsumoSeleccionado.Visibility = Visibility.Collapsed;
            }
        }

        private void BuscarInsumo()
        {
            if (barraDeBusquedaInsumo.tbxBusqueda.Text.Trim() != string.Empty)
            {
                wrpPanelInsumos.Children.Clear();

                string textoABuscar = barraDeBusquedaInsumo.tbxBusqueda.Text.Trim().ToUpper();
                MostrarCoincidenciasInsumo(textoABuscar);
            }
        }

        private void MostrarCoincidenciasInsumo(string textoABuscar)
        {
            foreach (InsumoRegistroReceta insumo in _insumosDisponibles)
            {
                string nombre = insumo.Nombre.ToUpper();
                string categoria = insumo.Categoria.Nombre.ToUpper();
                string codigo = insumo.Codigo;

                if (nombre.Contains(textoABuscar) || categoria.Contains(textoABuscar) || codigo.Contains(textoABuscar))
                {
                    MostrarInsumo(insumo);
                }
            }
        }
    }
}
