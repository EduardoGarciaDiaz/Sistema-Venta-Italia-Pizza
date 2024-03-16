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

        private ObservableCollection<ClienteBusqueda> _clientes;
        private string MENSAJE_CAMPO_VACIO = "Por favor, ingresa algo en la barra para realizar la busqueda.";
        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_CLIENTE = "El nombre ingresado no corresponde a ningun cliente.";
        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO = "El nombre ingresado no corresponde a ningun cliente.";
        private string MENSAJE_BUSQUEDA_OTRO_CLIENTE = "Cambiar cliente...";
        private TipoServicio _tipoServicioSeleccionado = new TipoServicio();
        private List<TipoServicio> _tiposServicio;
        SolidColorBrush _colorBrushAmarillo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6B400"));
        SolidColorBrush _colorBrushGris = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        SolidColorBrush _colorBrushWhite = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        SolidColorBrush _colorBrushGrisTexto = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF918C8C"));
        List<ProductoVentaPedidos> _productosVenta;
        Dictionary<string, int> _productosEnPedido = new Dictionary<string, int>();
        ClienteBusqueda _clienteSeleccionado = new ClienteBusqueda();
        private double _total = 0;
        private double _subtotal = 0;
        private double _iva = 0;

        public RegistroPedido()
        {
            _clientes = new ObservableCollection<ClienteBusqueda>();
            InitializeComponent();

            this.BarraBusquedaClientes.DataContext = _clientes;
            this.BarraBusquedaClientes.ImgBuscar_EventHandler += ImgBuscarClienteClicked;
            this.BarraBusquedaClientes.TxtBusqueda_EventHandler += TxtBusquedaClienteChanged;
            this.BarraBusquedaClientes.Lista_SelectionChanged_EventHandler += ListaSelectionChanged;
            this.BarraBusquedaProductos.ImgBuscarClicked += ImgBuscarProductosClicked;
            this.BarraBusquedaProductos.TxtBusquedaChanged_EventHandler += TxtBusquedaProductoChanged;
            _clienteSeleccionado = null;

            ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
            ServicioPedidosClient servicioPedidosClient = new ServicioPedidosClient();
            try
            {
                List<Categoria> categoriasProductoVenta = servicioProductosClient.RecuperarCategoriasProductoVenta().ToList();
                MostrarCategoriasProductoVenta(categoriasProductoVenta);
                _productosVenta = servicioProductosClient.RecuperarProductosVenta().ToList();
                MostrarProductosVenta(_productosVenta);
                _tiposServicio = servicioPedidosClient.RecuperarTiposServicio().ToList();
                _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EnEstablecimiento);
            }
            catch (EndpointNotFoundException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (TimeoutException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (FaultException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (CommunicationException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            MostrarCantidades();
    }

        public void MostrarCategoriasProductoVenta(List<Categoria> categorias)
        {
            foreach (Categoria categoria in categorias)
            {
                Label label = new Label();
                label.Content = categoria.Nombre.ToString();
                label.FontSize = 18;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.Padding = new Thickness(10);
                label.Foreground = _colorBrushGrisTexto;
                label.Tag = categoria.Id;
                label.MouseLeftButtonDown += FiltrarProductosPorCategoria;
                SkpCategoriasProductoVenta.Children.Add(label);
            }
        }

        public void MostrarProductosVenta(List<ProductoVentaPedidos> productosVenta)
        {
            SkpContenedorProductos.Children.Clear();
            List<ProductoVentaPedidos>.Enumerator iterator = productosVenta.GetEnumerator();
            iterator.MoveNext();
            for (int i = 0; i <= productosVenta.Count/4;  i++)
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                for (int j = 0; j < 4; j++)
                {
                    ProductoVentaPedidos productoVenta = iterator.Current;
                    ElementoProductoVenta elementoProductoVenta = new ElementoProductoVenta();                    
                    elementoProductoVenta.ImgProducto.Source = ConvertidorBytes.ConvertirBytesABitmapImage(productoVenta.Foto);
                    elementoProductoVenta.LblNombreProducto.Content = productoVenta.Nombre;
                    elementoProductoVenta.LblCodigo.Content = productoVenta.Codigo;
                    elementoProductoVenta.LblDescripcionProducto.Text = productoVenta.Descripcion;
                    elementoProductoVenta.LblPrecioProducto.Content = "$" + productoVenta.Precio.ToString();
                    elementoProductoVenta.Click += ElementoProductoVenta_Click;

                    stackPanel.Children.Add(elementoProductoVenta);

                    iterator.MoveNext();
                    if (iterator.Current == null)
                    {
                        break;
                    }
                }
                this.SkpContenedorProductos.Children.Add(stackPanel);
            }
            iterator.Dispose();
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
                        _productosEnPedido.Add(productoSeleccionado.Codigo, 1);
                        MostrarDatosProductoSeleccionado(productoSeleccionado);
                        CalcularCantidades();
                        MostrarCantidades();
                    }
                    else
                    {
                        MostrarLabelDuranteSegundos(LblProductoNoDisponible, 2);
                    }
                }
                catch (EndpointNotFoundException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (TimeoutException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (FaultException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (CommunicationException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        private void SumarCantidadAProducto(string codigoProducto)
        {
            int cantidadActualProductos = _productosEnPedido[codigoProducto];
            int cantidadNuevaRequeridaProductos = cantidadActualProductos + 1;
            ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
            try
            {
                bool productoDisponibleParaVenta = servicioProductosClient.ValidarDisponibilidadDeProducto(codigoProducto, cantidadNuevaRequeridaProductos);
                if (productoDisponibleParaVenta)
                {
                    _productosEnPedido[codigoProducto]++;
                    ActualizarEnInterfazCantidadRequeridaProductos(codigoProducto);
                    CalcularCantidades();
                    MostrarCantidades();
                }
                else
                {
                    MostrarLabelDuranteSegundos(LblProductoNoDisponible, 2);
                }
            }
            catch (EndpointNotFoundException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (TimeoutException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (FaultException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (CommunicationException ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                // TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void MostrarDatosProductoSeleccionado(ProductoVentaPedidos productoSeleccionado)
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

        private void BtnRestarProductoClicked(object sender, EventArgs e)
        {
            ElementoPedido elementoPedido = sender as ElementoPedido;
            string codigoProducto = elementoPedido.ProductoVentaPedidos.Codigo;
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
            string cantidadIngresada = elementoPedido.TbxCantidadProducto.Text;
            if (!string.IsNullOrWhiteSpace(cantidadIngresada))
            {
                ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
                try
                {
                    bool productoDisponible = servicioProductosClient.ValidarDisponibilidadDeProducto(elementoPedido.ProductoVentaPedidos.Codigo, int.Parse(elementoPedido.TbxCantidadProducto.Text));
                    if (productoDisponible)
                    {
                        _productosEnPedido[elementoPedido.ProductoVentaPedidos.Codigo] = int.Parse(cantidadIngresada);
                        elementoPedido.LblMensajeInsumosInsuficientes.Visibility = Visibility.Collapsed;
                        CalcularCantidades();
                        MostrarCantidades();
                    }
                    else
                    {
                        elementoPedido.LblMensajeInsumosInsuficientes.Visibility = Visibility.Visible;
                        _productosEnPedido[elementoPedido.ProductoVentaPedidos.Codigo] = 0;
                        CalcularCantidades();
                        MostrarCantidades();
                    }
                }
                catch (EndpointNotFoundException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (TimeoutException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (FaultException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (CommunicationException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
            }
            else
            {
                elementoPedido.LblMensajeInsumosInsuficientes.Visibility = Visibility.Collapsed;
                _productosEnPedido[elementoPedido.ProductoVentaPedidos.Codigo] = 0;
                CalcularCantidades();
                MostrarCantidades();
            }
        }



        private void ActualizarEnInterfazCantidadRequeridaProductos(string codigoProducto)
        {
            ElementoPedido elementoPedido = SkpContenedorProductosPedido.Children
                                                        .OfType<ElementoPedido>()
                                                        .FirstOrDefault(ep => ep.ProductoVentaPedidos.Codigo == codigoProducto);
            if (elementoPedido != null)
            {
                elementoPedido.TbxCantidadProducto.Text = _productosEnPedido[codigoProducto].ToString();
            }
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

        public void ImgBuscarClienteClicked(object sender, EventArgs e)
        {
            string textoIngresado = this.BarraBusquedaClientes.TxtBusqueda.Text;
            if (!ValidarCamposVacios(textoIngresado))
            {
                try
                {
                    _clientes.Clear();
                    ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
                    List<ClienteBusqueda> clientesBusqueda = servicioUsuariosClient.BuscarCliente(this.BarraBusquedaClientes.TxtBusqueda.Text).ToList();
                    if (!(clientesBusqueda.Count == 0))
                    {
                        foreach (ClienteBusqueda clienteBusqueda in clientesBusqueda)
                        {
                            _clientes.Add(clienteBusqueda);
                        }
                        this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        LblMensajeAdvertenciaCliente.Content = MENSAJE_SIN_RESULTADOS_BUSQUEDA_CLIENTE;
                    }
                }
                catch (EndpointNotFoundException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (TimeoutException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (FaultException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (CommunicationException ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    // TODO: Manejar excepcion
                    Console.WriteLine(ex.StackTrace);
                }
            } else
            {
                LblMensajeAdvertenciaCliente.Content = MENSAJE_CAMPO_VACIO;
                MostrarLabelDuranteSegundos(LblMensajeAdvertenciaCliente, 2);
            }
        }

        private bool ValidarCamposVacios (string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        private void TxtBusquedaClienteChanged(object sender, EventArgs e)
        {
            this.LblMensajeAdvertenciaCliente.Content = "";
            this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Collapsed;
        }

        private void TxtBusquedaProductoChanged(object sender, EventArgs e)
        {
            this.LblMensajeAdvertenciaProducto.Content = "";
            if (string.IsNullOrWhiteSpace(BarraBusquedaProductos.TxtBusqueda.Text))
            {
                MostrarProductosVenta(_productosVenta);
            }
        }

        private void ListaSelectionChanged(Object sender, EventArgs e)
        {
            if (BarraBusquedaClientes.ListaClientes.SelectedItem is ClienteBusqueda cliente)
            {
                _clienteSeleccionado = cliente;
                LblNombreCliente.Content = cliente.Nombre;
                LblCorreoElectronicoCliente.Content = cliente.Correo;
                this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Collapsed;
                this.BarraBusquedaClientes.TxtBusqueda.Text = "";
                this.BarraBusquedaClientes.Placeholder.Text = MENSAJE_BUSQUEDA_OTRO_CLIENTE;
            }
        }

        private void ImgBuscarProductosClicked(object sender, EventArgs e)
        {
            string valorBusqueda = BarraBusquedaProductos.TxtBusqueda.Text.ToString();
            if (!(ValidarCamposVacios(valorBusqueda)))
            {
                List<ProductoVentaPedidos> resultadoBusquedaProductos = new List<ProductoVentaPedidos>();
                resultadoBusquedaProductos = _productosVenta.Where(producto =>
                    producto.Nombre.ToLower().Contains(valorBusqueda.ToLower())
                    || producto.Codigo.ToLower().Contains(valorBusqueda.ToLower())
                ).ToList();
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
                MostrarLabelDuranteSegundos(LblMensajeAdvertenciaProducto, 2);
            }
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
            BtnEntregaDomicilio.Foreground= _colorBrushGrisTexto;
            BtnComerEstablecimiento.Foreground= _colorBrushWhite;
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EnEstablecimiento);
        }

        private void MostrarLabelDuranteSegundos(Label label, int segundos)
        {
            label.Visibility = Visibility.Visible;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(segundos);
            timer.Tick += (sender, e) =>
            {
                label.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
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

        private void LblTodasCategorias_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CambiarColorFiltroCategoria((Label)sender);
            MostrarProductosVenta(_productosVenta);
        }

        private void CambiarColorFiltroCategoria(Label labelSeleccionado)
        {
            foreach (Label label in SkpCategoriasProductoVenta.Children.OfType<Label>().ToList())
            {
                label.Foreground = _colorBrushGrisTexto;
            }
            labelSeleccionado.Foreground = _colorBrushAmarillo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSeleccionado != null && _productosEnPedido.Count > 0)
            {
                Pedido pedido = new Pedido
                {
                    CantidadProductos = _productosEnPedido.Count,
                    Fecha = DateTime.Now,
                    productosIncluidos = _productosEnPedido,
                    IdCliente = _clienteSeleccionado.IdCliente,
                    IdTipoServicio = _tipoServicioSeleccionado.Id,
                    Total = _total,
                    IdEstadoPedido = (int) EnumEstadosPedido.EnProceso
                };

                RegistroPagoPedido registroPagoPedido = new RegistroPagoPedido(pedido);
                NavigationService.Navigate(registroPagoPedido);
            } 
            else
            {
                LblMensajeSeleccionClienteProductoObligatoria.Visibility = Visibility.Visible;
                MostrarLabelDuranteSegundos(LblMensajeSeleccionClienteProductoObligatoria, 3);
            }
        }

        private void BrdEliminarDatosPedido_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _clienteSeleccionado = null;
            LblNombreCliente.Content = "";
            LblCorreoElectronicoCliente.Content = "";
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EnEstablecimiento);
            BtnComerEstablecimiento_Click(this, e);
            _productosEnPedido.Clear();
            SkpContenedorProductosPedido.Children.Clear();
            _total = 0;
            _subtotal = 0;
            _iva = 0;
            MostrarCantidades();
        }
    }
}
