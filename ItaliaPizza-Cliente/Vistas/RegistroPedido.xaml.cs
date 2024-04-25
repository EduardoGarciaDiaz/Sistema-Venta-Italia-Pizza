using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para RegistroPedido.xaml
    /// </summary>
    public partial class RegistroPedido : Page
    {

        private string MENSAJE_BUSQUEDA_OTRO_CLIENTE = "Cambiar cliente...";
        private string MENSAJE_CAMPO_VACIO = "Por favor, ingresa algo en la barra para realizar la busqueda.";
        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_CLIENTE = "El nombre ingresado no corresponde a ningun cliente.";
        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO = "El nombre ingresado no corresponde a ningun cliente.";

        private SolidColorBrush _colorBrushAmarillo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6B400"));
        private SolidColorBrush _colorBrushGris = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        private SolidColorBrush _colorBrushGrisTexto = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF918C8C"));
        private SolidColorBrush _colorBrushWhite = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));

        private ObservableCollection<ClienteBusqueda> _clientes;
        private List<ProductoVentaPedidos> _productosVenta;
        private Dictionary<string, int> _productosEnPedido = new Dictionary<string, int>();
        private List<TipoServicio> _tiposServicio;

        private double _iva = 0;
        private double _subtotal = 0;
        private double _total = 0;

        private ClienteBusqueda _clienteSeleccionado = new ClienteBusqueda();
        private TipoServicio _tipoServicioSeleccionado = new TipoServicio();

        public RegistroPedido()
        {
            _clientes = new ObservableCollection<ClienteBusqueda>();
            InitializeComponent();
            this.Loaded += RegistroPedido_Loaded;
        }

        private void RegistroPedido_Loaded(object sender, RoutedEventArgs e)
        {
            InicializarElementos();
            MostrarCantidades();
            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
            ServicioPedidosClient servicioPedidosCliente = new ServicioPedidosClient();
            try
            {
                List<Categoria> categoriasProductoVenta = servicioProductosCliente.RecuperarCategoriasProductoVenta().ToList();
                MostrarCategoriasProductoVenta(categoriasProductoVenta);
                _tiposServicio = servicioPedidosCliente.RecuperarTiposServicio().ToList();
                _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EnEstablecimiento);
                _productosVenta = servicioProductosCliente.RecuperarProductosVenta().ToList();
                MostrarProductosVenta(_productosVenta);
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
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this) );
            }
            catch (Exception ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
        }

        private void ElementoProductoVenta_Click(object sender, RoutedEventArgs e)
        {
            bool productoDisponibleParaVenta;
            ElementoProductoVenta elementoProductoVenta = (ElementoProductoVenta)sender;
            string codigoProducto = elementoProductoVenta.LblCodigo.Content.ToString();
            if (_productosEnPedido.ContainsKey(codigoProducto))
            {
                SumarCantidadAProducto(codigoProducto);
            }
            else
            {
                try
                {
                    ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
                    productoDisponibleParaVenta = servicioProductosClient.ValidarDisponibilidadDeProducto(codigoProducto, 1);
                    if (productoDisponibleParaVenta)
                    {
                        ProductoVentaPedidos productoSeleccionado = _productosVenta.Where(producto => producto.Codigo == codigoProducto).FirstOrDefault();
                        AgregarProductoAPedido(productoSeleccionado);
                        CalcularCantidades();
                        MostrarCantidades();
                        MostrarDatosProductoSeleccionado(productoSeleccionado);
                    }
                    else
                    {
                        Utilidad.MostrarMensaje(LblProductoNoDisponible, 2);
                    }
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
        }

        private void AgregarProductoAPedido(ProductoVentaPedidos producto)
        {
            int cantidad = 1;
            _productosEnPedido.Add(producto.Codigo, cantidad);
        }

        private void BtnRestarProductoClicked(object sender, EventArgs e)
        {
            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
            ElementoPedido elementoPedido = sender as ElementoPedido;
            string codigoProducto = elementoPedido.ProductoVentaPedidos.Codigo;
            try
            {
                if (_productosEnPedido.ContainsKey(codigoProducto))
                {
                    if (servicioProductosCliente.DesapartarInsumosDeProducto(codigoProducto, 1))
                    {
                        _productosEnPedido[codigoProducto]--;
                        if (_productosEnPedido[codigoProducto] == 0)
                        {
                            SkpContenedorProductosPedido.Children.Remove(elementoPedido);
                            _productosEnPedido.Remove(elementoPedido.ProductoVentaPedidos.Codigo);
                        }
                        else
                        {
                            ActualizarEnInterfazCantidadRequeridaProductos(codigoProducto);
                        }
                        CalcularCantidades();
                        MostrarCantidades();
                    }
                }
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

        private void BtnSumarProductoClicked(object sender, EventArgs e)
        {
            ElementoPedido elementoPedido = sender as ElementoPedido;
            SumarCantidadAProducto(elementoPedido.ProductoVentaPedidos.Codigo);
        }

        private void TbxCantidadProductoLostFocus(object sender, EventArgs e)
        {
            ElementoPedido elementoPedido = sender as ElementoPedido;
            string cantidadIngresada = elementoPedido.TbxCantidadProducto.Text;
            if (string.IsNullOrWhiteSpace(cantidadIngresada) || cantidadIngresada == "0")
            {
                SkpContenedorProductosPedido.Children.Remove(elementoPedido);
                _productosEnPedido.Remove(elementoPedido.ProductoVentaPedidos.Codigo);
            }
        }

        private void TbxCantidadProductoTextChanged(object sender, EventArgs e)
        {
            ElementoPedido elementoPedido = sender as ElementoPedido;
            string codigoProducto = elementoPedido.ProductoVentaPedidos.Codigo;
            int cantidadActual = 0;
            if (_productosEnPedido.ContainsKey(elementoPedido.ProductoVentaPedidos.Codigo))
            {
                cantidadActual = _productosEnPedido[elementoPedido.ProductoVentaPedidos.Codigo];
            }
            string cantidadIngresada = elementoPedido.TbxCantidadProducto.Text;
                ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
            try
            {
                if (!string.IsNullOrWhiteSpace(cantidadIngresada))
                {
                    if (servicioProductosCliente.DesapartarInsumosDeProducto(codigoProducto, cantidadActual))
                    {
                        bool productoDisponible = servicioProductosCliente.ValidarDisponibilidadDeProducto(codigoProducto, int.Parse(elementoPedido.TbxCantidadProducto.Text));
                        if (productoDisponible)
                        {
                            if (_productosEnPedido.ContainsKey(elementoPedido.ProductoVentaPedidos.Codigo))
                            {
                                _productosEnPedido[elementoPedido.ProductoVentaPedidos.Codigo] = int.Parse(cantidadIngresada);
                            }
                            else
                            {
                                _productosEnPedido.Add(elementoPedido.ProductoVentaPedidos.Codigo, int.Parse(cantidadIngresada));
                            }
                            elementoPedido.LblMensajeInsumosInsuficientes.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            elementoPedido.LblMensajeInsumosInsuficientes.Visibility = Visibility.Visible;
                            _productosEnPedido.Remove(elementoPedido.ProductoVentaPedidos.Codigo);
                        }
                    }
                }
                else
                {
                    elementoPedido.LblMensajeInsumosInsuficientes.Visibility = Visibility.Collapsed;
                    servicioProductosCliente.DesapartarInsumosDeProducto(elementoPedido.ProductoVentaPedidos.Codigo, _productosEnPedido[elementoPedido.ProductoVentaPedidos.Codigo]);
                    _productosEnPedido.Remove(elementoPedido.ProductoVentaPedidos.Codigo);
                }
                CalcularCantidades();
                MostrarCantidades();
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

        private void ImgBuscarClienteClicked(object sender, EventArgs e)
        {
            string textoIngresado = this.BarraBusquedaClientes.TxtBusqueda.Text;
            if (!ValidarCamposVacios(textoIngresado))
            {
                try
                {
                    _clientes.Clear();
                    ServicioUsuariosClient servicioUsuariosCliente = new ServicioUsuariosClient();
                    List<ClienteBusqueda> clientesCoincidentes = servicioUsuariosCliente.BuscarCliente(this.BarraBusquedaClientes.TxtBusqueda.Text).ToList();
                    if (clientesCoincidentes.Any())
                    {
                        MostrarCoincidencias(clientesCoincidentes);
                    }
                    else
                    {
                        LblMensajeAdvertenciaCliente.Content = MENSAJE_SIN_RESULTADOS_BUSQUEDA_CLIENTE;
                    }
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
            else
            {
                LblMensajeAdvertenciaCliente.Content = MENSAJE_CAMPO_VACIO;
                Utilidad.MostrarMensaje(LblMensajeAdvertenciaCliente, 2);
            }
        }

        private void MostrarCoincidencias(List<ClienteBusqueda> clientesCoincidentes)
        {

            foreach (ClienteBusqueda clienteBusqueda in clientesCoincidentes)
            {
                _clientes.Add(clienteBusqueda);
            }
            this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Visible;
        }

        private void TxtBusquedaClienteChanged(object sender, EventArgs e)
        {
            this.LblMensajeAdvertenciaCliente.Content = "";
            this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Collapsed;
        }

        private void TxtBusquedaProductoChanged(object sender, EventArgs e)
        {
            this.LblMensajeAdvertenciaProducto.Content = "";
            if (string.IsNullOrWhiteSpace(BarraBusquedaProductos.txbBusqueda.Text))
            {
                MostrarProductosVenta(_productosVenta);
            }
        }

        private void ListaSelectionChanged(Object sender, EventArgs e)
        {
            if (BarraBusquedaClientes.ListaClientes.SelectedItem is ClienteBusqueda cliente)
            {
                MostrarDatosCliente(cliente);
            }
        }

        private void MostrarDatosCliente(ClienteBusqueda cliente)
        {
            _clienteSeleccionado = cliente;
            LblNombreCliente.Content = cliente.Nombre;
            LblCorreoElectronicoCliente.Content = cliente.Correo;
            this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Collapsed;
            this.BarraBusquedaClientes.TxtBusqueda.Text = "";
            this.BarraBusquedaClientes.Placeholder.Text = MENSAJE_BUSQUEDA_OTRO_CLIENTE;

        }

        private void ImgBuscarProductosClicked(object sender, EventArgs e)
        {
            string valorBusqueda = BarraBusquedaProductos.txbBusqueda.Text.ToString();
            if (!(ValidarCamposVacios(valorBusqueda)))
            {
                List<ProductoVentaPedidos> resultadoBusquedaProductos = BuscarProducto(valorBusqueda);
                if (resultadoBusquedaProductos.Count != 0)
                {
                    MostrarProductosVenta(resultadoBusquedaProductos);
                }
                else
                {
                    LblMensajeAdvertenciaProducto.Content = MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO;
                }
            }
            else
            {
                LblMensajeAdvertenciaProducto.Content = MENSAJE_CAMPO_VACIO;
                Utilidad.MostrarMensaje(LblMensajeAdvertenciaProducto, 2);
            }
        }

        private List<ProductoVentaPedidos> BuscarProducto(string valorBusqueda)
        {
            List<ProductoVentaPedidos> resultadoBusquedaProductos = new List<ProductoVentaPedidos>();
            resultadoBusquedaProductos = _productosVenta.Where(producto =>
                producto.Nombre.ToLower().Contains(valorBusqueda.ToLower())
                || producto.Codigo.ToLower().Contains(valorBusqueda.ToLower())
            ).ToList();
            return resultadoBusquedaProductos;
        }

        private void BtnEntregaDomicilio_Click(object sender, RoutedEventArgs e)
        {
            BtnEntregaDomicilio.Background = _colorBrushAmarillo;
            BtnEntregaDomicilio.Foreground = _colorBrushWhite;
            BtnComerEstablecimiento.Background = _colorBrushGris;
            BtnComerEstablecimiento.Foreground = _colorBrushGrisTexto;
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EntregaDomicilio);
        }

        private void BtnComerEstablecimiento_Click(object sender, RoutedEventArgs e)
        {
            BtnComerEstablecimiento.Background = _colorBrushAmarillo;
            BtnEntregaDomicilio.Background = _colorBrushGris;
            BtnEntregaDomicilio.Foreground = _colorBrushGrisTexto;
            BtnComerEstablecimiento.Foreground = _colorBrushWhite;
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EnEstablecimiento);
        }

        private void LblTodasCategorias_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CambiarColorFiltroCategoria((Label)sender);
            MostrarProductosVenta(_productosVenta);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSeleccionado != null && _productosEnPedido.Count > 0)
            {
                Pedido pedido = new Pedido
                {
                    CantidadProductos = _productosEnPedido.Count,
                    Fecha = DateTime.Now,
                    ProductosIncluidos = ListarProductosEnPedido(),
                    IdCliente = _clienteSeleccionado.IdCliente,
                    TipoServicio = _tipoServicioSeleccionado,
                    Total = _total,
                    IdEstadoPedido = (int)EnumEstadosPedido.EnProceso
                };

                RegistroPagoPedido registroPagoPedido = new RegistroPagoPedido(pedido, this);
                NavigationService.Navigate(registroPagoPedido);
            }
            else
            {
                LblMensajeSeleccionClienteProductoObligatoria.Visibility = Visibility.Visible;
                Utilidad.MostrarMensaje(LblMensajeSeleccionClienteProductoObligatoria, 3);
            }
        }

        private void BrdEliminarDatosPedido_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _clienteSeleccionado = null;
            LblNombreCliente.Content = "";
            LblCorreoElectronicoCliente.Content = "";
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EnEstablecimiento);
            BtnComerEstablecimiento_Click(this, e);
            DesapartarTodosLosProductosEnPedido();
            _productosEnPedido.Clear();
            SkpContenedorProductosPedido.Children.Clear();
            _total = 0;
            _subtotal = 0;
            _iva = 0;
            MostrarCantidades();
        }

        public void DesapartarTodosLosProductosEnPedido()
        {
            ServicioProductosClient servicioProductos = new ServicioProductosClient();
            try
            {
                foreach (var producto in _productosEnPedido.Keys)
                {
                    servicioProductos.DesapartarInsumosDeProducto(producto, _productosEnPedido[producto]);
                }
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

        private void InicializarElementos()
        {
            this.BarraBusquedaClientes.DataContext = _clientes;
            this.BarraBusquedaClientes.ImgBuscarClicked += ImgBuscarClienteClicked;
            this.BarraBusquedaClientes.TxtBusquedaTextChanged += TxtBusquedaClienteChanged;
            this.BarraBusquedaClientes.ListaSelectionChanged += ListaSelectionChanged;
            this.BarraBusquedaProductos.ImgBuscarClicked += ImgBuscarProductosClicked;
            this.BarraBusquedaProductos.TxbBusquedaTextChanged += TxtBusquedaProductoChanged;
            _clienteSeleccionado = null;

        }

        private void MostrarCategoriasProductoVenta(List<Categoria> categorias)
        {
            categorias?.ForEach(categoria =>
            {
                Label label = new Label();
                label.Content = categoria.Nombre.ToString();
                label.FontSize = 18;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.Padding = new Thickness(10);
                label.Foreground = _colorBrushGrisTexto;
                label.Tag = categoria.Id;
                label.Cursor = Cursors.Hand;
                label.MouseLeftButtonDown += FiltrarProductosPorCategoria;
                SkpCategoriasProductoVenta.Children.Add(label);
            }) ;
        }

        private void MostrarProductosVenta(List<ProductoVentaPedidos> productosVenta)
        {
            SkpContenedorProductos.Children.Clear();

            StackPanel stackPanel = null;
            for (int i = 0; i < productosVenta.Count; i++)
            {
                if (i % 4 == 0)
                {
                    stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal
                    };
                    SkpContenedorProductos.Children.Add(stackPanel);
                }
                ProductoVentaPedidos productoVenta = productosVenta[i];
                ElementoProductoVenta elementoProductoVenta = new ElementoProductoVenta
                {
                    ImgProducto = { Source = ConvertidorBytes.ConvertirBytesABitmapImage(productoVenta.Foto) },
                    LblNombreProducto = { Content = productoVenta.Nombre },
                    LblCodigo = { Content = productoVenta.Codigo },
                    LblDescripcionProducto = { Text = productoVenta.Descripcion },
                    LblPrecioProducto = { Content = $"${productoVenta.Precio}" }
                };
                elementoProductoVenta.ProdcutoVentaClicked += ElementoProductoVenta_Click;
                stackPanel.Children.Add(elementoProductoVenta);
            }
        }

        private void SumarCantidadAProducto(string codigoProducto)
        {
            int cantidadNuevaRequeridaProductos = 1;
            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
            try
            {
                bool productoDisponibleParaVenta = servicioProductosCliente.ValidarDisponibilidadDeProducto(codigoProducto, cantidadNuevaRequeridaProductos);
                if (productoDisponibleParaVenta)
                {
                    _productosEnPedido[codigoProducto]++;
                    ActualizarEnInterfazCantidadRequeridaProductos(codigoProducto);
                    CalcularCantidades();
                    MostrarCantidades();
                }
                else
                {
                    Utilidad.MostrarMensaje(LblProductoNoDisponible, 2);
                }
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

        private void MostrarDatosProductoSeleccionado(ProductoVentaPedidos productoSeleccionado)
        {
            if (productoSeleccionado != null)
            {
                ElementoPedido elementoPedido = new ElementoPedido();
                elementoPedido.ProductoVentaPedidos = productoSeleccionado;
                elementoPedido.LblNombreProducto.Content = productoSeleccionado.Nombre;
                elementoPedido.LblDescripcionProducto.Content = productoSeleccionado.Descripcion;
                elementoPedido.LblPrecioProducto.Content = "$" + productoSeleccionado.Precio;
                elementoPedido.ProductoVentaPedidos = productoSeleccionado;
                elementoPedido.TbxCantidadProducto.Text = _productosEnPedido[productoSeleccionado.Codigo].ToString();
                elementoPedido.TbxCantidadProductoTextChanged += TbxCantidadProductoTextChanged;
                elementoPedido.TbxLostFocusTbxCantidad += TbxCantidadProductoLostFocus;
                elementoPedido.BtnSumarClicked += BtnSumarProductoClicked;
                elementoPedido.BtnRestarClicked += BtnRestarProductoClicked;

                SkpContenedorProductosPedido.Children.Add(elementoPedido);
            }
        }

        private void ActualizarEnInterfazCantidadRequeridaProductos(string codigoProducto)
        {
            ElementoPedido elementoPedido = SkpContenedorProductosPedido.Children
                                                        .OfType<ElementoPedido>()
                                                        .FirstOrDefault(ep => ep.ProductoVentaPedidos.Codigo == codigoProducto);
            if (elementoPedido != null)
            {
                DesuscribirMetodoTextChanged(elementoPedido);
                elementoPedido.TbxCantidadProducto.Text = _productosEnPedido[codigoProducto].ToString();
                SuscribirMetodoTextChanged(elementoPedido);
            }
        }

        private void SuscribirMetodoTextChanged(ElementoPedido elementoPedido)
        {
            elementoPedido.TbxCantidadProductoTextChanged += TbxCantidadProductoTextChanged;
        }

        private void DesuscribirMetodoTextChanged(ElementoPedido elementoPedido)
        {
            elementoPedido.TbxCantidadProductoTextChanged -= TbxCantidadProductoTextChanged;
        }

        private void CalcularCantidades()
        {
            _total = ObtenerTotal();
            _subtotal = (_total / 1.16);
            _iva = _total - _subtotal;
        }

        private void MostrarCantidades()
        {
            LblTotal.Content = _total.ToString("F2");
            LblSubtotal.Content = _subtotal.ToString("F2");
            LblIva.Content = _iva.ToString("F2");
        }

        private double ObtenerTotal()
        {   
            double total = 0;
            List<ProductoVentaPedidos> productoVentaPedidos = _productosVenta.Where(pv => _productosEnPedido.Keys.ToList().Any(k => k == pv.Codigo)).ToList();
            total = productoVentaPedidos.Sum(pv => (pv.Precio * _productosEnPedido[pv.Codigo]));
            return total;
        }

        private bool ValidarCamposVacios (string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        private void FiltrarProductosPorCategoria(object sender, MouseButtonEventArgs e) 
        {
            CambiarColorFiltroCategoria((Label)sender);
            int? idCategoria = ((Label)sender).Tag as int?;
            List<ProductoVentaPedidos> productosFiltrados = _productosVenta.Where(pv => pv.IdCategoria == idCategoria).ToList();
            if (productosFiltrados.Count > 0)
            {
                MostrarProductosVenta(productosFiltrados);
            }
            else
            {
                SkpContenedorProductos.Children.Clear();
            }
        }

        private void CambiarColorFiltroCategoria(Label labelSeleccionado)
        {
            foreach (Label label in SkpCategoriasProductoVenta.Children.OfType<Label>().ToList())
            {
                label.Foreground = _colorBrushGrisTexto;
            }
            labelSeleccionado.Foreground = _colorBrushAmarillo;
        }

        private Dictionary<ProductoVentaPedidos, int> ListarProductosEnPedido()
        {
            Dictionary<ProductoVentaPedidos, int> productosPedido = new Dictionary<ProductoVentaPedidos, int>();
            foreach (string key in _productosEnPedido.Keys)
            {
                ProductoVentaPedidos producto = _productosVenta.FirstOrDefault(p => p.Codigo == key);
                productosPedido.Add(producto, _productosEnPedido[key]);
            }
            return productosPedido;
        }

    }
}
