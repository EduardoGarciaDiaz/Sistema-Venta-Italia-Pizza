using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para Productos.xaml
    /// </summary>
    public partial class Productos : Page
    {
        private const string SIMBOLO_MONEDA = "$";
        private const string FOREGROUND_FILTRO_SELECCIONADO = "#FFD6B400";
        private const string BACKGROUND_FILTRO_SELECCIONADO = "#49F8D72A";
        private const string BORDERBRUSH_FILTRO_SELECCIONADO = "#FFF8D72A";
        private const string FOREGROUND_FILTRO_DESSELECCIONADO = "#656565";
        private const string FILTRO_TODOS = "TODAS";
        private const int VENTANA_ERROR = 1;
        private const int VENTANA_CONFIRMACION = 3;

        private Producto[] _insumos;
        private Producto[] _productosVenta;
        private List<Categoria> _categoriasInsumo = new List<Categoria>();
        private List<Categoria> _categoriasProductoVenta = new List<Categoria>();

        public Productos()
        {
            InitializeComponent();
            this.Loaded += Productos_Loaded;
        }

        private void Productos_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CargarProductos();
                CargarFiltrosCategorias();
                AgregarEventos();
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

        private void AgregarEventos()
        {
            barraDeBusqueda.TxbBusquedaTextChanged += TbxBusqueda_TextChanged;
            barraDeBusqueda.ImgBuscarClicked += ImgBuscar_Click;
            barraDeBusqueda.EnterPressed += Enter_Pressed;
        }

        private void CargarProductos()
        {
            CargarProductosTipoInsumo();
            CargarProductosTipoVenta();
        }

        private void CargarFiltrosCategorias()
        {
            LimpiarStackPanelCategorias();
            CargarCategoriasInsumo();
            CargarCategoriasProductoVenta();
        }

        private void LimpiarStackPanelCategorias()
        {
            _categoriasInsumo.Clear();
            _categoriasProductoVenta.Clear();
            stackPanelCategoriasInsumo.Children.Clear();
            stackPanelCategoriasProductoVenta.Children.Clear();
        }

        private void CargarCategoriasInsumo()
        {
            RecuperarCategoriasInsumo();
            MostrarFiltrosCategoriasInsumo();
        }

        private void RecuperarCategoriasInsumo()
        {
            ServicioProductosClient servicioProductos = new ServicioProductosClient();
            _categoriasInsumo.Add(new Categoria() { Id = 0, Nombre = FILTRO_TODOS });
            _categoriasInsumo.AddRange(servicioProductos.RecuperarCategoriasInsumo());
        }

        private void MostrarFiltrosCategoriasInsumo()
        {
            if (_categoriasInsumo != null && _categoriasInsumo.Count() > 0)
            {
                foreach (Categoria categoria in _categoriasInsumo)
                {
                    MostrarFiltroCategoriasInsumo(categoria);
                }
            }
        }

        private void MostrarFiltroCategoriasInsumo(Categoria categoria)
        {
            BtnFiltro btnFiltroCategoria = new BtnFiltro();
            btnFiltroCategoria.btnFiltro.Content = categoria.Nombre;
            btnFiltroCategoria.BtnFiltroClicked += BtnFiltroCategoriasInsumo_Click;

            stackPanelCategoriasInsumo.Children.Add(btnFiltroCategoria);
        }

        private void CargarCategoriasProductoVenta()
        {
            RecuperarCategoriasProductoVenta();
            MostrarFiltrosCategoriasProductoVenta();
        }

        private void RecuperarCategoriasProductoVenta()
        {
            ServicioProductosClient servicioProductos = new ServicioProductosClient();
            _categoriasProductoVenta.Add(new Categoria() { Id = 0, Nombre = FILTRO_TODOS });
            _categoriasProductoVenta.AddRange(servicioProductos.RecuperarCategoriasProductoVenta());
        }

        private void MostrarFiltrosCategoriasProductoVenta()
        {
            if (_categoriasProductoVenta != null && _categoriasProductoVenta.Count() > 0)
            {
                foreach (Categoria categoria in _categoriasProductoVenta)
                {
                    MostrarFiltroCategoriasProductoVenta(categoria);
                }
            }
        }

        private void MostrarFiltroCategoriasProductoVenta(Categoria categoria)
        {
            BtnFiltro btnFiltroCategoria = new BtnFiltro();
            btnFiltroCategoria.btnFiltro.Content = categoria.Nombre;
            btnFiltroCategoria.BtnFiltroClicked += BtnFiltroCategoriasProductoVenta_Click;

            stackPanelCategoriasProductoVenta.Children.Add(btnFiltroCategoria);
        }


        private void BtnFiltroCategoriasInsumo_Click(object sender, EventArgs e)
        {
            LimpiarFiltrosCategorias(stackPanelCategoriasInsumo);

            BtnFiltro btnFiltroCategoria = (BtnFiltro)sender;
            MostrarFiltroCategoriaSeleccionado(btnFiltroCategoria);

            string categoriaSeleccionada = btnFiltroCategoria.btnFiltro.Content.ToString().ToUpper();
            MostrarCoincidenciasInsumos(categoriaSeleccionada);
        }

        private void MostrarFiltroCategoriaSeleccionado(BtnFiltro btnFiltroCategoria)
        {
            btnFiltroCategoria.btnFiltro.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(FOREGROUND_FILTRO_SELECCIONADO));
            btnFiltroCategoria.btnFiltro.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(BACKGROUND_FILTRO_SELECCIONADO));
            btnFiltroCategoria.btnFiltro.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(BORDERBRUSH_FILTRO_SELECCIONADO));
        }

        private void LimpiarFiltrosCategorias(StackPanel stackPanelFiltros)
        {
            foreach(BtnFiltro btnFiltroCategoria in stackPanelFiltros.Children)
            {
                btnFiltroCategoria.btnFiltro.Foreground = new SolidColorBrush((Color)ColorConverter.
                    ConvertFromString(FOREGROUND_FILTRO_DESSELECCIONADO));
                btnFiltroCategoria.btnFiltro.Background = new SolidColorBrush(Colors.White);
                btnFiltroCategoria.btnFiltro.BorderBrush = new SolidColorBrush(Colors.White);
            }
        }

        private void MostrarCoincidenciasInsumos(string textoABuscar)
        {
            stackPanelInsumos.Children.Clear();
            lblSinInsumos.Visibility = Visibility.Visible;

            foreach (Producto producto in _insumos)
            {
                string nombre = producto.Nombre.ToUpper();
                string codigo = producto.Codigo.ToUpper();
                string categoria = producto.Insumo.Categoria.Nombre.ToUpper();

                if (nombre.Contains(textoABuscar) || codigo.Contains(textoABuscar) || categoria.Contains(textoABuscar))
                {
                    MostrarProductoTipoInsumo(producto);
                    lblSinInsumos.Visibility = Visibility.Collapsed;
                }
                
                if (textoABuscar == FILTRO_TODOS)
                {
                    MostrarProductoTipoInsumo(producto);
                    lblSinInsumos.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void BtnFiltroCategoriasProductoVenta_Click(object sender, EventArgs e)
        {
            LimpiarFiltrosCategorias(stackPanelCategoriasProductoVenta);

            BtnFiltro btnFiltroCategoria = (BtnFiltro)sender;
            MostrarFiltroCategoriaSeleccionado(btnFiltroCategoria);

            string categoriaSeleccionada = btnFiltroCategoria.btnFiltro.Content.ToString().ToUpper();
            MostrarCoincidenciasProductosVenta(categoriaSeleccionada);
        }

        private void MostrarCoincidenciasProductosVenta(string textoABuscar)
        {
            wrapPanelProductosVenta.Children.Clear();
            lblSinProductosVenta.Visibility = Visibility.Visible;

            foreach (Producto producto in _productosVenta)
            {
                string nombre = producto.Nombre.ToUpper();
                string codigo = producto.Codigo.ToUpper();
                string categoria = producto.ProductoVenta.Categoria.Nombre.ToUpper();

                if (nombre.Contains(textoABuscar) || codigo.Contains(textoABuscar) || categoria.Contains(textoABuscar))
                {
                    MostrarProductoTipoVenta(producto);
                    lblSinProductosVenta.Visibility = Visibility.Collapsed;
                }

                if (textoABuscar == FILTRO_TODOS)
                {
                    MostrarProductoTipoVenta(producto);
                    lblSinProductosVenta.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CargarProductosTipoInsumo()
        {
            _insumos = RecuperarProductosTipoInsumo();
            bool hayExistencia = ValidarExistenciaProductos(_insumos);
            MostrarMensajeSinProductos(lblSinInsumos);

            if (hayExistencia)
            {
                MostrarProductosTipoInsumo();
            } 
        }

        private Producto[] RecuperarProductosTipoInsumo()
        {
            ServicioProductosClient servicioProductos = new ServicioProductosClient();
            Producto[] insumos = servicioProductos.RecuperarProductosTipoInsumo();

            return insumos;
        }

        private bool ValidarExistenciaProductos<T>(T[] productos)
        {
            bool hayExistencia = false;

            if (productos != null && productos.Count() > 0)
            {
                hayExistencia = true;
            }

            return hayExistencia;
        }

        private void MostrarMensajeSinProductos(Label labelSinProductos)
        {
            labelSinProductos.Visibility = Visibility.Visible;
        }

        private void MostrarProductosTipoInsumo()
        {
            lblSinInsumos.Visibility = Visibility.Collapsed;

            if (_insumos != null && _insumos.Count() > 0)
            {
                stackPanelInsumos.Children.Clear();

                foreach (Producto insumo in _insumos)
                {
                    MostrarProductoTipoInsumo(insumo);
                } 
            }
        }

        private void MostrarProductoTipoInsumo(Producto insumo)
        {
            ElementoConsultaInsumo elementoConsultaInsumo = CrearElementoConsultaInsumo(insumo);

            stackPanelInsumos.Children.Add(elementoConsultaInsumo);
        }

        private ElementoConsultaInsumo CrearElementoConsultaInsumo(Producto insumo)
        {
            ElementoConsultaInsumo elementoConsultaInsumo = new ElementoConsultaInsumo(insumo);
            elementoConsultaInsumo.GridInsumoClicked += ElementoProductoInsumo_Click;
            elementoConsultaInsumo.ImgModificarInsumoClicked += ImgModificar_Click;
            elementoConsultaInsumo.BtnDesactivarActivarProductoClicked += BtnDesactivarActivarInsumo_Click;

            return elementoConsultaInsumo;
        }

        private void ElementoProductoInsumo_Click(object sender, EventArgs e)
        {
            ElementoConsultaInsumo elementoConsultaInsumo = (ElementoConsultaInsumo)sender;
            gridDetallesProducto.Visibility = Visibility.Visible;

            MostrarDetalles(elementoConsultaInsumo.ProductoAsignado);
        }

        private void BtnDesactivarActivarInsumo_Click(object sender, EventArgs e)
        {
            ElementoConsultaInsumo elementoConsultaInsumo = (ElementoConsultaInsumo)sender;
            string codigoProducto = elementoConsultaInsumo.ProductoAsignado.Codigo;
            string nombre = elementoConsultaInsumo.ProductoAsignado.Nombre; 
            bool esActivo = elementoConsultaInsumo.ProductoAsignado.EsActivo;
            bool esProductoVenta = false;

            int filasAfectadas = ActivarDesactivarProducto(esActivo, codigoProducto, nombre, esProductoVenta);
            if (filasAfectadas > 0)
            {
                elementoConsultaInsumo.ProductoAsignado.EsActivo = !esActivo;
                elementoConsultaInsumo.ActualizarEstadoActivo(elementoConsultaInsumo.ProductoAsignado.EsActivo);
            }
        }

        private void BtnDesactivarActivarProductoVenta_Click(object sender, EventArgs e)
        {
            ElementoConsultaProductoVenta elementoConsultaProductoVenta = (ElementoConsultaProductoVenta)sender;
            string codigoProducto = elementoConsultaProductoVenta.ProductoAsignado.Codigo;
            string nombre = elementoConsultaProductoVenta.ProductoAsignado.Nombre;
            bool esActivo = elementoConsultaProductoVenta.ProductoAsignado.EsActivo;
            bool esProductoVenta = true;

            int filasAfectadas = ActivarDesactivarProducto(esActivo, codigoProducto, nombre, esProductoVenta);
            if (filasAfectadas > 0)
            {
                elementoConsultaProductoVenta.ProductoAsignado.EsActivo = !esActivo;
                elementoConsultaProductoVenta.ActualizarEstadoActivo(elementoConsultaProductoVenta.ProductoAsignado.EsActivo);
            }
        }

        private int ActivarDesactivarProducto(bool esActivo, string codigoProducto, string nombre, bool esProductoVenta)
        {
            int filasAfectadas = -1;

            try
            {
                filasAfectadas = AdministrarActivacionDesactivacionProducto(esActivo, codigoProducto, nombre, esProductoVenta);
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

        private int AdministrarActivacionDesactivacionProducto(bool esActivo, string codigoProducto, string nombre, bool esProductoVenta)
        {
            int filasAfectadas = -1;

            if (esActivo)
            {
                filasAfectadas = ManejarDesactivacion(codigoProducto, nombre, esProductoVenta);
            }
            else
            {
                filasAfectadas = ConfirmarActivacionProducto(codigoProducto, nombre);
            }

            return filasAfectadas;
        }

        private int ManejarDesactivacion(string codigoProducto, string nombre, bool esProductoVenta)
        {
            int filasAfectadas = -1;

            if (esProductoVenta)
            {
                filasAfectadas = ManejarDesactivacionProductoVenta(codigoProducto, nombre);
            }
            else
            {
                filasAfectadas = ManejarDesactivacionInsumo(codigoProducto, nombre);
            }

            return filasAfectadas;
        }

        private int ManejarDesactivacionProductoVenta(string codigoProducto, string nombre)
        {
            int filasAfectadas = -1;

            if (ValidarDesactivacionProductoVenta(codigoProducto))
            {
                filasAfectadas = ConfirmarDesactivacionProducto(codigoProducto, nombre);
            }
            else
            {
                MostrarMensajeDesactivacionInvalida();
            }

            return filasAfectadas;
        }

        private int ManejarDesactivacionInsumo(string codigoProducto, string nombre)
        {
            int filasAfectadas = -1;

            if (ValidarDesactivacionInsumo(codigoProducto))
            {
                filasAfectadas = ConfirmarDesactivacionProducto(codigoProducto, nombre);
            }
            else
            {
                MostrarMensajeDesactivacionInsumoInvalida();
            }

            return filasAfectadas;
        }

        private bool ValidarDesactivacionProductoVenta(string codigoProducto)
        {
            bool esDesactivacionValida = false;
            ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
            esDesactivacionValida = servicioProductosClient.ValidarDesactivacion(codigoProducto);

            return esDesactivacionValida;
        }

        private bool ValidarDesactivacionInsumo(string codigoProducto)
        {
            bool esDesactivacionValida = false;
            ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
            esDesactivacionValida = servicioProductosClient.ValidarDesactivacionInsumo(codigoProducto);

            return esDesactivacionValida;
        }

        private int ConfirmarDesactivacionProducto(string codigoProducto, string nombreProducto)
        {
            int filasAfectadas = -1;
            string titulo = "Desactivar producto";
            string mensaje = $"¿Estás seguro de que deseas Desactivar el producto {nombreProducto}?";
            VentanaEmergente ventanaEmergente = new VentanaEmergente(titulo, mensaje , "Sí", "No", Window.GetWindow(this), VENTANA_CONFIRMACION);
            ventanaEmergente.ShowDialog();

            if (ventanaEmergente.AceptarAccion)
            {
                filasAfectadas = DesactivarProducto(codigoProducto);
            }

            return filasAfectadas;
        }

        private int DesactivarProducto(string codigoProducto)
        {
            ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
            int filasAfectadas = servicioProductosClient.DesactivarProducto(codigoProducto);

            return filasAfectadas;
        }

        private int ConfirmarActivacionProducto(string codigoProducto, string nombreProducto)
        {
            int filasAfectadas = -1;
            string titulo = "Activar producto";
            string mensaje = $"¿Estás seguro de que deseas Activar el producto {nombreProducto}?";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(titulo, mensaje, "Sí", "No", Window.GetWindow(this), VENTANA_CONFIRMACION);
            ventanaEmergente.ShowDialog();
            if (ventanaEmergente.AceptarAccion)
            {
                filasAfectadas = ActivarProducto(codigoProducto);
            }

            return filasAfectadas;
        }

        private int ActivarProducto(string codigoProducto)
        {
            ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
            int filasAfectadas = servicioProductosClient.ActivarProducto(codigoProducto);

            return filasAfectadas;
        }

        private void MostrarMensajeDesactivacionInvalida()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("No se puede desactivar", "No puedes desactivar este producto porque hay un pedido pendiente que hace uso de este", Window.GetWindow(this), VENTANA_ERROR);
            ventanaEmergente.ShowDialog();
        }

        private void MostrarMensajeDesactivacionInsumoInvalida()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("No se puede desactivar", "No puedes desactivar este insumo porque hay un producto en venta que hace uso de este", Window.GetWindow(this), VENTANA_ERROR);
            ventanaEmergente.ShowDialog();
        }

        private void CargarProductosTipoVenta()
        {
            _productosVenta = RecuperarProductosTipoVenta();
            bool hayExistencia = ValidarExistenciaProductos(_productosVenta);
            MostrarMensajeSinProductosVenta(lblSinProductosVenta);

            if (hayExistencia)
            {
                MostrarProductosTipoVenta();
            }
        }

        private Producto[] RecuperarProductosTipoVenta()
        {
            ServicioProductosClient servicioProductos = new ServicioProductosClient();
            Producto[] productosVenta = servicioProductos.RecuperarProductosTipoVenta();

            return productosVenta;
        }

        private void MostrarMensajeSinProductosVenta(Label labelSinProductos)
        {
            labelSinProductos.Visibility = Visibility.Visible;
        }

        private void MostrarProductosTipoVenta()
        {
            lblSinProductosVenta.Visibility = Visibility.Collapsed;

            if (_productosVenta != null && _productosVenta.Count() > 0)
            {
                wrapPanelProductosVenta.Children.Clear();

                foreach (Producto productoVenta in _productosVenta)
                {
                    MostrarProductoTipoVenta(productoVenta);
                }
            }
        }

        private void MostrarProductoTipoVenta(Producto productoVenta)
        {
            ElementoConsultaProductoVenta elementoConsultaProductoVenta = CrearElementoConsultaProductoVenta(productoVenta);

            wrapPanelProductosVenta.Children.Add(elementoConsultaProductoVenta);
        }

        private ElementoConsultaProductoVenta CrearElementoConsultaProductoVenta(Producto productoVenta)
        {
            ElementoConsultaProductoVenta elementoConsultaProductoVenta = new ElementoConsultaProductoVenta(productoVenta);
            elementoConsultaProductoVenta.GridProductoVentaClicked += ElementoProductoVenta_Click;
            elementoConsultaProductoVenta.ImgModificarProductoVentaClicked += ImgModificar_Click;
            elementoConsultaProductoVenta.BtnDesactivarActivarProductoClicked += BtnDesactivarActivarProductoVenta_Click;

            return elementoConsultaProductoVenta;
        }

        private void ElementoProductoVenta_Click(object sender, EventArgs e)
        {
            gridDetallesProducto.Visibility = Visibility.Visible;
            LimpiarDetallesProducto();
            ElementoConsultaProductoVenta elementoConsultaProductoVenta = (ElementoConsultaProductoVenta)sender;
            MostrarDetalles(elementoConsultaProductoVenta.ProductoAsignado);
        }

        private void MostrarDetalles(Producto productoSeleccionado)
        {
            Insumo insumo = productoSeleccionado.Insumo;
            ProductoVenta productoVenta = productoSeleccionado.ProductoVenta;

            lblCodigo.Content = productoSeleccionado.Codigo;
            lblNombre.Content = productoSeleccionado.Nombre;
            tbkDescripcion.Text = productoSeleccionado.Descripcion;

            if (insumo != null)
            {
                MostrarDetallesInsumo(insumo);
            }

            if (productoVenta != null)
            {
                MostrarDetallesProductoVenta(productoVenta);
            }
        }

        private void MostrarDetallesProductoVenta(ProductoVenta productoVenta)
        {
            lblPrecio.Content = SIMBOLO_MONEDA + productoVenta.Precio;
            lblCategoria.Content = productoVenta.Categoria.Nombre;
            if (productoVenta.Foto != null)
            {
                BitmapImage foto = ConvertidorBytes.ConvertirBytesABitmapImage(productoVenta.Foto);
                if (foto != null)
                {
                    imgFoto.Source = foto;
                }
            }
        }

        private void MostrarDetallesInsumo(Insumo insumo)
        {
            string cantidad = insumo.Cantidad.ToString();
            string unidadMedida = insumo.UnidadMedida.Nombre;
            lblCantidad.Content = cantidad + unidadMedida;
            lblCosto.Content = SIMBOLO_MONEDA + insumo.CostoUnitario;
            tbkRestricciones.Text = insumo.Restriccion;
            lblCategoria.Content = insumo.Categoria.Nombre;
        }


        private void ImgCerrarDetalles_Click(object sender, RoutedEventArgs e)
        {
            LimpiarDetallesProducto();
            gridDetallesProducto.Visibility = Visibility.Collapsed;
        }

        private void LimpiarDetallesProducto()
        {
            lblCodigo.Content = string.Empty;
            lblNombre.Content = string.Empty;
            lblCategoria.Content = string.Empty;
            tbkDescripcion.Text = string.Empty;
            lblCantidad.Content = string.Empty;
            lblCosto.Content = string.Empty;
            tbkRestricciones.Text = string.Empty;
            lblPrecio.Content = string.Empty;
            imgFoto.Source = null;
        }

        private void ImgBuscar_Click(object sender, EventArgs e)
        {
            BuscarProductos();
        }

        private void Enter_Pressed(object sender, EventArgs e)
        {
            if (e is KeyEventArgs keyEventArgs && keyEventArgs.Key == Key.Enter)
            {
                BuscarProductos();
            }
        }

        private void BuscarProductos()
        {
            if (barraDeBusqueda.tbxBusqueda.Text != string.Empty)
            {
                LimpiarFiltrosCategorias(stackPanelCategoriasInsumo);
                LimpiarFiltrosCategorias(stackPanelCategoriasProductoVenta);
                wrapPanelProductosVenta.Children.Clear();
                stackPanelInsumos.Children.Clear();

                string textoABuscar = barraDeBusqueda.tbxBusqueda.Text.Trim().ToUpper();
                MostrarCoincidenciasInsumos(textoABuscar);
                MostrarCoincidenciasProductosVenta(textoABuscar);
            }
        }

        private void TbxBusqueda_TextChanged(object sender, EventArgs e)
        {
            if (barraDeBusqueda.tbxBusqueda.Text.Trim() == string.Empty)
            {
                MostrarProductosTipoInsumo();
                MostrarProductosTipoVenta();
            }
        }

        private void BtnRegistrarProducto_Click(object sender, RoutedEventArgs e)
        {
            RegistroProducto registroProducto = new RegistroProducto();
            NavigationService.Navigate(registroProducto);
        }

        private void ImgModificar_Click(object sender, EventArgs e)
        {
            Producto productoEdicion;

            if (sender is ElementoConsultaInsumo)
            {
                ElementoConsultaInsumo elementoConsultaInsumo = (ElementoConsultaInsumo)sender;
                productoEdicion = elementoConsultaInsumo.ProductoAsignado;
                Console.WriteLine("Insumo a modificar");

            } 
            else
            {
                ElementoConsultaProductoVenta elementoConsultaProductoVenta = (ElementoConsultaProductoVenta)sender;
                productoEdicion = elementoConsultaProductoVenta.ProductoAsignado;
                Console.WriteLine("Producto Venta a modificar");

            }

            EdicionProducto edicionProducto = new EdicionProducto(_categoriasProductoVenta, _categoriasInsumo, productoEdicion);
            NavigationService.Navigate(edicionProducto);
        }

        private void BtnValidarInventario_Click(object sender, RoutedEventArgs e)
        {
            List<Categoria> categorias = new List<Categoria>();
            categorias.AddRange(_categoriasInsumo);
            var bebidas = _categoriasProductoVenta.FirstOrDefault(c => c.Nombre == "Bebidas");
            if (bebidas != null)
            {
                categorias.Add(bebidas);
            }
            var postres = _categoriasProductoVenta.FirstOrDefault(c => c.Nombre == "Postres");
            if (postres != null)
            {
                categorias.Add(postres);
            }

            NavigationService.Navigate(new ValidacionInventario(categorias));
        }
    }
}
