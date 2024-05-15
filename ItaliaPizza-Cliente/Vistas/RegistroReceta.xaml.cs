using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
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
    /// Lógica de interacción para RegistroReceta.xaml
    /// </summary>
    public partial class RegistroReceta : Page
    {
        private const string COLOR_HEXADECIMAL_PRODUCTO_SELECCIONADO = "#F8D72A";
        private const string COLOR_HEXADECIMAL_ELEMENTO_DEFAULT = "#F6F6F6";
        private const string COLOR_HEXADECIMAL_INSUMO_SELECCIONADO = "#FF7B7B7B";
        private const int VENTANA_INFORMACION = 2;
        private const int VENTANA_CONFIRMACION = 3;
        private int _numeroInsumosSeleccionados = 0;
        private ProductoSinReceta[] _productosSinReceta;
        private InsumoRegistroReceta[] _insumosDisponibles;
        private ElementoProductoSinReceta _elementoProductoSinRecetaSeleccionado;
        private List<InsumoReceta> _insumosSeleccionados = new List<InsumoReceta>();

        public RegistroReceta()
        {
            InitializeComponent();
            this.Loaded += RegistroReceta_Loaded;
        }

        private void RegistroReceta_Loaded(object sender, RoutedEventArgs e)
        {
            AgregarEventos();

            try
            {
                CargarProductosSinReceta();
                CargarInsumos();
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

        private void TbxBusquedaProducto_TextChanged(object sender, EventArgs e)
        {
            if (barraDeBusquedaProducto.tbxBusqueda.Text.Trim() == string.Empty)
            {
                wrpPanelProductosSinReceta.Children.Clear();
                MostrarProductosSinReceta();
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

        private void ImgBuscarProducto_Click(object sender, EventArgs e)
        {
            BuscarProducto();
        }

        private void EnterProducto_Pressed(object sender, EventArgs e)
        {
            if (e is KeyEventArgs keyEventArgs && keyEventArgs.Key == Key.Enter)
            {
                BuscarProducto();
            }
        }

        private void BtnDesasignarInsumo_Click(object sender, EventArgs e)
        {
            ElementoInsumoSeleccionado elementoInsumoSeleccionado = sender as ElementoInsumoSeleccionado;
            RemoverInsumoSeleccionado(elementoInsumoSeleccionado);
            HabilitarElementoInsumoRegistroReceta(elementoInsumoSeleccionado.InsumoAsignado.Codigo);
        }

        private void GrdProductoSinReceta_Click(object sender, EventArgs e)
        {
            LimpiarSeleccionProductosSinReceta();
            ElementoProductoSinReceta productoSinReceta = sender as ElementoProductoSinReceta;

            MostrarProductoSeleccionado(productoSinReceta);
        }

        private void AgregarEventos()
        {
            barraDeBusquedaInsumo.Placeholder.Text = "Buscar insumo...";
            barraDeBusquedaInsumo.TbxBusquedaTextChanged += TbxBusquedaInsumo_TextChanged;
            barraDeBusquedaInsumo.ImgBuscarClicked += ImgBuscarInsumo_Click;
            barraDeBusquedaInsumo.EnterPressed += EnterInsumo_Pressed;

            barraDeBusquedaProducto.Placeholder.Text = "Buscar producto sin receta...";
            barraDeBusquedaProducto.TbxBusquedaTextChanged += TbxBusquedaProducto_TextChanged;
            barraDeBusquedaProducto.ImgBuscarClicked += ImgBuscarProducto_Click;
            barraDeBusquedaProducto.EnterPressed += EnterProducto_Pressed;
        }

        private void CargarProductosSinReceta()
        {
            RecuperarProductosSinReceta();
            wrpPanelProductosSinReceta.Children.Clear();

            if (_productosSinReceta != null && _productosSinReceta.Count() > 0)
            {
                MostrarProductosSinReceta();
            }
        }

        private void RecuperarProductosSinReceta()
        {
            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();

            _productosSinReceta = servicioProductosCliente.RecuperarProductosSinReceta();
        }

        private void MostrarProductosSinReceta()
        {
            if (_productosSinReceta != null && _productosSinReceta.Count() > 0)
            {
                foreach (ProductoSinReceta productoSinReceta in _productosSinReceta)
                {
                    MostrarProductoSinReceta(productoSinReceta);
                }
            }
        }

        private void MostrarProductoSinReceta(ProductoSinReceta productoSinReceta)
        {
            ElementoProductoSinReceta elementoProductoSinReceta = CrearElementoProductoSinReceta(productoSinReceta);

            if (_elementoProductoSinRecetaSeleccionado != null)
            {
                if ((string)_elementoProductoSinRecetaSeleccionado.lblCodigo.Content == productoSinReceta.Codigo)
                {
                    elementoProductoSinReceta.rectangleProducto.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(COLOR_HEXADECIMAL_PRODUCTO_SELECCIONADO);
                    elementoProductoSinReceta.EsSeleccionado = true;
                }
            }

            wrpPanelProductosSinReceta.Children.Add(elementoProductoSinReceta);
        }

        private ElementoProductoSinReceta CrearElementoProductoSinReceta(ProductoSinReceta productoSinReceta)
        {
            ElementoProductoSinReceta elementoProductoSinReceta = 
                new ElementoProductoSinReceta(productoSinReceta);

            elementoProductoSinReceta.GrdProductoSinRecetaClicked += GrdProductoSinReceta_Click;

            return elementoProductoSinReceta;
        }

        private void MostrarProductoSeleccionado(ElementoProductoSinReceta productoSinReceta)
        {
            productoSinReceta.rectangleProducto.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(COLOR_HEXADECIMAL_PRODUCTO_SELECCIONADO);
            _elementoProductoSinRecetaSeleccionado = productoSinReceta;
            productoSinReceta.EsSeleccionado = true;
            lblNombreReceta.Content = productoSinReceta.tbkNombre.Text;
        }

        private void LimpiarSeleccionProductosSinReceta()
        {
            foreach (ElementoProductoSinReceta elementopProducto in wrpPanelProductosSinReceta.Children)
            {
                elementopProducto.EsSeleccionado = false;

                elementopProducto.rectangleProducto.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(COLOR_HEXADECIMAL_ELEMENTO_DEFAULT);
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
            string mensajeErrorSinProducto = "Primero debes seleccionar un producto para guardarle la receta";
            tbkErrorRegistro.Visibility = Visibility.Collapsed;

            if (ValidarProductoSeleccionado())
            {
                ElementoInsumoRegistroReceta elementoInsumoRegistroReceta = sender as ElementoInsumoRegistroReceta;

                if (!elementoInsumoRegistroReceta.EsSeleccionado)
                {
                    SeleccionarInsumo(elementoInsumoRegistroReceta);
                    InsumoReceta insumoReceta = CrearInsumoRecetaDesdeElementoInsumoRegistro(elementoInsumoRegistroReceta);
                    _insumosSeleccionados.Add(insumoReceta);
                    MostrarInsumoSeleccionado(insumoReceta);
                }
            }
            else
            {
                Utilidad.MostrarTextoError(tbkErrorRegistro, mensajeErrorSinProducto);
            }
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

        private bool ValidarProductoSeleccionado()
        {
            bool hayProductoSeleccionado = false;

            if (_elementoProductoSinRecetaSeleccionado != null)
            {
                hayProductoSeleccionado = true;
            }
            
            return hayProductoSeleccionado;
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
            if (ValidarGuardadoReceta())
            {
                int filasAfectadas = -1;

                RecetaProducto nuevaReceta = CrearReceta();

                filasAfectadas = GuardarReceta(nuevaReceta);

                if (filasAfectadas > 0)
                {
                    ManejarRegistroExitoso();
                }
            }
        }

        private RecetaProducto CrearReceta()
        {
            InsumoReceta[] insumosSeleccionados = new InsumoReceta[_numeroInsumosSeleccionados];
            insumosSeleccionados = RecopilarInsumosSeleccionados(insumosSeleccionados);
            string codigoProductoSinReceta = _elementoProductoSinRecetaSeleccionado.lblCodigo.Content.ToString();

            RecetaProducto nuevaReceta = new RecetaProducto()
            {
                CodigoProducto = codigoProductoSinReceta,
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

                insumosSeleccionados[i] = insumoReceta;
            }

            return insumosSeleccionados;
        }

        private int GuardarReceta(RecetaProducto nuevaReceta)
        {
            int filasAfectadas = -1;
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();

            try
            {
                filasAfectadas = servicioRecetasCliente.GuardarReceta(nuevaReceta);
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

        private void ManejarRegistroExitoso()
        {
            string tituloExito = "Receta guardada";
            string mensajeExito = "Receta guardada con éxito";

            LimpiarCampos();

            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloExito, mensajeExito, Window.GetWindow(this), VENTANA_INFORMACION);
            ventanaEmergente.ShowDialog();
        }

        private void LimpiarCampos()
        {
            _numeroInsumosSeleccionados = 0;
            _elementoProductoSinRecetaSeleccionado = null;
            _insumosSeleccionados = new List<InsumoReceta>();
            lblNombreReceta.Content = string.Empty;
            ActualizarContadorInsumos();
            MostrarInsumos();
            CargarProductosSinReceta();
            wrpPanelInsumosSeleccionados.Children.Clear();
        }

        private bool ValidarGuardadoReceta()
        {
            bool esGuardadoValido = true;
            string mensajeErrorSinProducto = "Primero debes seleccionar un producto para guardarle la receta";
            string mensajeErrorSinInsumo = "No puedes guardar una receta sin insumos";

            LimpiarMensajesError();

            if (!ValidarProductoSeleccionado())
            {
                esGuardadoValido = false;
                Utilidad.MostrarTextoError(tbkErrorRegistro, mensajeErrorSinProducto);
            }

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

        private void BuscarProducto()
        {
            if (barraDeBusquedaProducto.tbxBusqueda.Text.Trim() != string.Empty)
            {
                wrpPanelProductosSinReceta.Children.Clear();

                string textoABuscar = barraDeBusquedaProducto.tbxBusqueda.Text.Trim().ToUpper();
                MostrarCoincidenciasProducto(textoABuscar);
            }
        }

        private void MostrarCoincidenciasProducto(string textoABuscar)
        {
            foreach (ProductoSinReceta producto in _productosSinReceta)
            {
                string nombre = producto.Nombre.ToUpper();
                string codigo = producto.Codigo.ToUpper();

                if (nombre.Contains(textoABuscar) || codigo.Contains(textoABuscar))
                {
                    MostrarProductoSinReceta(producto);
                }
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
