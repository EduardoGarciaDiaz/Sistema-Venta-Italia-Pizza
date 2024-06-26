﻿using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
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

        private SolidColorBrush COLOR_BRUSH_AMARILLO = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6B400"));
        private SolidColorBrush COLOR_BRUSH_GRIS = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        private SolidColorBrush COLOR_BRUSH_GRIS_TEXTO = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF918C8C"));
        private SolidColorBrush COLOR_BRUSH_BLANCO = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        private string MENSAJE_BUSQUEDA_OTRO_CLIENTE = "Cambiar cliente...";
        private string MENSAJE_CAMPO_VACIO = "Por favor, ingresa algo en la barra para realizar la busqueda.";
        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_CLIENTE = "El nombre ingresado no corresponde a ningun cliente.";
        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO = "El código ingresado no corresponde a ningun producto.";

        private ObservableCollection<ClienteBusqueda> _clientes;
        private List<ProductoVentaPedidos> _productosVenta;
        private Dictionary<string, int> _productosEnPedido = new Dictionary<string, int>();
        private List<TipoServicio> _tiposServicio;
        private double _iva = 0;
        private double _subtotal = 0;
        private double _total = 0;
        private ClienteBusqueda _clienteSeleccionado = new ClienteBusqueda();
        private TipoServicio _tipoServicioSeleccionado = new TipoServicio();
        private bool _irAPago = false;

        public RegistroPedido()
        {
            _clientes = new ObservableCollection<ClienteBusqueda>();
            InitializeComponent();
            this.Loaded += RegistroPedido_Loaded;
            this.Unloaded += RegistroPedido_Unloaded;
        }

        private void RegistroPedido_Loaded(object sender, RoutedEventArgs e)
        {
            InicializarElementos();
            MostrarCantidades();
            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
            ServicioPedidosClient servicioPedidosCliente = new ServicioPedidosClient();
            try
            {
                List<Categoria> categorias = servicioProductosCliente.RecuperarCategoriasProductoVenta().ToList();
                MostrarCategorias(categorias);
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

        private void RegistroPedido_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!_irAPago)
            {
                DesapartarTodosLosProductosEnPedido();
            }
        }

        private void ElementoProductoVenta_Click(object sender, RoutedEventArgs e)
        {
            bool productoDisponibleParaVenta;
            ElementoProductoVenta elementoProductoVenta = (ElementoProductoVenta)sender;
            string codigoProducto = elementoProductoVenta.lblCodigo.Content.ToString();
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
                        Utilidad.MostrarMensaje(lblProductoNoDisponible, 2);
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

        private void BtnSumarProductoClicked(object sender, EventArgs e)
        {
            ElementoPedido elementoPedido = sender as ElementoPedido;
            SumarCantidadAProducto(elementoPedido.ProductoVentaPedidos.Codigo);
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
                            skpContenedorProductosPedido.Children.Remove(elementoPedido);
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

        private void TbxCantidadProductoLostFocus(object sender, EventArgs e)
        {
            ElementoPedido elementoPedido = sender as ElementoPedido;
            string cantidadIngresada = elementoPedido.tbxCantidadProducto.Text;
            if (string.IsNullOrWhiteSpace(cantidadIngresada) || cantidadIngresada == "0")
            {
                skpContenedorProductosPedido.Children.Remove(elementoPedido);
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
            string cantidadIngresada = elementoPedido.tbxCantidadProducto.Text;
            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();
            try
            {
                if (!string.IsNullOrWhiteSpace(cantidadIngresada))
                {
                    if (servicioProductosCliente.DesapartarInsumosDeProducto(codigoProducto, cantidadActual))
                    {
                        bool productoDisponible = servicioProductosCliente.ValidarDisponibilidadDeProducto(codigoProducto, int.Parse(elementoPedido.tbxCantidadProducto.Text));
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
                            elementoPedido.lblMensajeInsumosInsuficientes.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            elementoPedido.lblMensajeInsumosInsuficientes.Visibility = Visibility.Visible;
                            _productosEnPedido.Remove(elementoPedido.ProductoVentaPedidos.Codigo);
                        }
                    }
                }
                else
                {
                    elementoPedido.lblMensajeInsumosInsuficientes.Visibility = Visibility.Collapsed;
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

        private void ImgBuscarCliente_Click(object sender, EventArgs e)
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
                        lblMensajeAdvertenciaCliente.Content = MENSAJE_SIN_RESULTADOS_BUSQUEDA_CLIENTE;
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
                lblMensajeAdvertenciaCliente.Content = MENSAJE_CAMPO_VACIO;
                Utilidad.MostrarMensaje(lblMensajeAdvertenciaCliente, 2);
            }
        }

        private void TxtBusquedaClienteChanged(object sender, EventArgs e)
        {
            this.lblMensajeAdvertenciaCliente.Content = "";
            this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Collapsed;
        }

        private void TxtBusquedaProductoChanged(object sender, EventArgs e)
        {
            this.lblMensajeAdvertenciaProducto.Content = "";
            if (string.IsNullOrWhiteSpace(BarraBusquedaProductos.tbxBusqueda.Text))
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

        private void ImgBuscarProductos_Click(object sender, EventArgs e)
        {
            string valorBusqueda = BarraBusquedaProductos.tbxBusqueda.Text.ToString();
            if (!(ValidarCamposVacios(valorBusqueda)))
            {
                List<ProductoVentaPedidos> resultadoBusquedaProductos = BuscarProducto(valorBusqueda);
                if (resultadoBusquedaProductos.Count != 0)
                {
                    MostrarProductosVenta(resultadoBusquedaProductos);
                }
                else
                {
                    lblMensajeAdvertenciaProducto.Content = MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO;
                }
            }
            else
            {
                lblMensajeAdvertenciaProducto.Content = MENSAJE_CAMPO_VACIO;
                Utilidad.MostrarMensaje(lblMensajeAdvertenciaProducto, 2);
            }
        }

        private void BtnEntregaDomicilio_Click(object sender, RoutedEventArgs e)
        {
            btnEntregaDomicilio.Background = COLOR_BRUSH_AMARILLO;
            btnEntregaDomicilio.Foreground = COLOR_BRUSH_BLANCO;
            btnComerEstablecimiento.Background = COLOR_BRUSH_GRIS;
            btnComerEstablecimiento.Foreground = COLOR_BRUSH_GRIS_TEXTO;
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EntregaDomicilio);
        }

        private void BtnComerEstablecimiento_Click(object sender, RoutedEventArgs e)
        {
            btnComerEstablecimiento.Background = COLOR_BRUSH_AMARILLO;
            btnEntregaDomicilio.Background = COLOR_BRUSH_GRIS;
            btnEntregaDomicilio.Foreground = COLOR_BRUSH_GRIS_TEXTO;
            btnComerEstablecimiento.Foreground = COLOR_BRUSH_BLANCO;
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EnEstablecimiento);
        }

        private void LblTodasCategorias_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CambiarColorFiltroCategoria((Label)sender);
            MostrarProductosVenta(_productosVenta);
        }

        private void BtnProcederPago_Click(object sender, RoutedEventArgs e)
        {
            bool clienteProductoSeleccionados = ValidarSeleccionObligatoria();
            if (clienteProductoSeleccionados)
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
                _irAPago = true;
                RegistroPagoPedido registroPagoPedido = new RegistroPagoPedido(pedido, this);
                NavigationService.Navigate(registroPagoPedido);
            }
            else
            {
                lblMensajeSeleccionClienteProductoObligatoria.Visibility = Visibility.Visible;
                Utilidad.MostrarMensaje(lblMensajeSeleccionClienteProductoObligatoria, 3);
            }
        }

        private void BrdEliminarDatosPedido_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _clienteSeleccionado = null;
            lblNombreCliente.Content = "";
            lblCorreoElectronicoCliente.Content = "";
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EnEstablecimiento);
            BtnComerEstablecimiento_Click(this, e);
            DesapartarTodosLosProductosEnPedido();
            _productosEnPedido.Clear();
            skpContenedorProductosPedido.Children.Clear();
            _total = 0;
            _subtotal = 0;
            _iva = 0;
            MostrarCantidades();
        }

        private void InicializarElementos()
        {
            this.BarraBusquedaClientes.DataContext = _clientes;
            this.BarraBusquedaClientes.ImgBuscarClicked += ImgBuscarCliente_Click;
            this.BarraBusquedaClientes.TxtBusquedaTextChanged += TxtBusquedaClienteChanged;
            this.BarraBusquedaClientes.ListaSelectionChanged += ListaSelectionChanged;
            this.BarraBusquedaProductos.ImgBuscarClicked += ImgBuscarProductos_Click;
            this.BarraBusquedaProductos.TxbBusquedaTextChanged += TxtBusquedaProductoChanged;
            _clienteSeleccionado = null;

        }

        private bool ValidarSeleccionObligatoria()
        {
            return _clienteSeleccionado != null && _productosEnPedido.Count > 0;
        }

        private void AgregarProductoAPedido(ProductoVentaPedidos producto)
        {
            int cantidad = 1;
            _productosEnPedido.Add(producto.Codigo, cantidad);
        }

        private void MostrarCoincidencias(List<ClienteBusqueda> clientesCoincidentes)
        {

            foreach (ClienteBusqueda clienteBusqueda in clientesCoincidentes)
            {
                _clientes.Add(clienteBusqueda);
            }
            this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Visible;
        }

        private void MostrarDatosCliente(ClienteBusqueda cliente)
        {
            _clienteSeleccionado = cliente;
            lblNombreCliente.Content = cliente.Nombre;
            lblCorreoElectronicoCliente.Content = cliente.Correo;
            this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Collapsed;
            this.BarraBusquedaClientes.TxtBusqueda.Text = "";
            this.BarraBusquedaClientes.Placeholder.Text = MENSAJE_BUSQUEDA_OTRO_CLIENTE;

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

        private void MostrarCategorias(List<Categoria> categorias)
        {
            categorias?.ForEach(categoria =>
            {
                Label label = new Label();
                label.Content = categoria.Nombre.ToString();
                label.FontSize = 18;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.Padding = new Thickness(10);
                label.Foreground = COLOR_BRUSH_GRIS_TEXTO;
                label.Tag = categoria.Id;
                label.Cursor = Cursors.Hand;
                label.MouseLeftButtonDown += FiltrarProductosPorCategoria;
                skpCategoriasProductoVenta.Children.Add(label);
            }) ;
        }

        private void MostrarProductosVenta(List<ProductoVentaPedidos> productosVenta)
        {
            skpContenedorProductos.Children.Clear();

            StackPanel stackPanel = null;
            for (int i = 0; i < productosVenta.Count; i++)
            {
                if (i % 4 == 0)
                {
                    stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal
                    };
                    skpContenedorProductos.Children.Add(stackPanel);
                }
                ProductoVentaPedidos productoVenta = productosVenta[i];
                ElementoProductoVenta elementoProductoVenta = new ElementoProductoVenta
                {
                    imgProducto = { Source = ConvertidorBytes.ConvertirBytesABitmapImage(productoVenta.Foto) },
                    lblNombreProducto = { Content = productoVenta.Nombre },
                    lblCodigo = { Content = productoVenta.Codigo },
                    lblDescripcionProducto = { Text = productoVenta.Descripcion },
                    lblPrecioProducto = { Content = $"${productoVenta.Precio}" }
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
                    Utilidad.MostrarMensaje(lblProductoNoDisponible, 2);
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
                elementoPedido.lblNombreProducto.Content = productoSeleccionado.Nombre;
                elementoPedido.lblDescripcionProducto.Content = productoSeleccionado.Descripcion;
                elementoPedido.lblPrecioProducto.Content = "$" + productoSeleccionado.Precio;
                elementoPedido.ProductoVentaPedidos = productoSeleccionado;
                elementoPedido.tbxCantidadProducto.Text = _productosEnPedido[productoSeleccionado.Codigo].ToString();
                elementoPedido.TbxCantidadProductoTextChanged += TbxCantidadProductoTextChanged;
                elementoPedido.TbxLostFocusTbxCantidad += TbxCantidadProductoLostFocus;
                elementoPedido.BtnSumarClicked += BtnSumarProductoClicked;
                elementoPedido.BtnRestarClicked += BtnRestarProductoClicked;

                skpContenedorProductosPedido.Children.Add(elementoPedido);
            }
        }

        private void ActualizarEnInterfazCantidadRequeridaProductos(string codigoProducto)
        {
            ElementoPedido elementoPedido = skpContenedorProductosPedido.Children
                                                        .OfType<ElementoPedido>()
                                                        .FirstOrDefault(ep => ep.ProductoVentaPedidos.Codigo == codigoProducto);
            if (elementoPedido != null)
            {
                DesuscribirMetodoTextChanged(elementoPedido);
                elementoPedido.tbxCantidadProducto.Text = _productosEnPedido[codigoProducto].ToString();
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
            lblTotal.Content = _total.ToString("F2");
            lblSubtotal.Content = _subtotal.ToString("F2");
            lblIva.Content = _iva.ToString("F2");
        }

        private double ObtenerTotal()
        {   
            double total = 0;
            List<ProductoVentaPedidos> productoVentaPedidos = _productosVenta.Where(pv => _productosEnPedido.Keys.ToList().Any(k => k == pv.Codigo)).ToList();
            total = productoVentaPedidos.Sum(pv => (pv.Precio * _productosEnPedido[pv.Codigo]));
            return total;
        }

        private bool ValidarCamposVacios(string text)
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
                skpContenedorProductos.Children.Clear();
            }
        }

        private void CambiarColorFiltroCategoria(Label labelSeleccionado)
        {
            foreach (Label label in skpCategoriasProductoVenta.Children.OfType<Label>().ToList())
            {
                label.Foreground = COLOR_BRUSH_GRIS_TEXTO;
            }
            labelSeleccionado.Foreground = COLOR_BRUSH_AMARILLO;
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
