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
using System.Windows.Navigation;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para ValidacionInventario.xaml
    /// </summary>
    public partial class ValidacionInventario : Page
    {
        private const string FOREGROUND_FILTRO_SELECCIONADO = "#FFD6B400";
        private const string BACKGROUND_FILTRO_SELECCIONADO = "#49F8D72A";
        private const string BORDERBRUSH_FILTRO_SELECCIONADO = "#FFF8D72A";
        private const string FOREGROUND_FILTRO_DESSELECCIONADO = "#656565";
        private const string FILTRO_TODOS = "TODAS";

        private Producto[] _productos;
        private List<Categoria> _categorias;

        public ValidacionInventario()
        {
            InitializeComponent();
        }

        public ValidacionInventario(List<Categoria> categorias)
        {
            InitializeComponent();
            _categorias = categorias;
            CargarFiltrosCategoria();
            CargarProductos();
            AgregarEventos();
        }

        private void AgregarEventos() { 
            barraDeBusqueda.ImgBuscarClicked += ImgBuscar_Click;
            barraDeBusqueda.TxbBusquedaTextChanged += TbxBusqueda_TextChanged;
            barraDeBusqueda.EnterPressed += Enter_Pressed;
        }

        private void CargarFiltrosCategoria()
        {
            if (_categorias != null)
            {
                foreach (Categoria categoria in _categorias)
                {
                    BtnFiltro btnFiltroCategoria = new BtnFiltro();
                    btnFiltroCategoria.btnFiltro.Content = categoria.Nombre;
                    btnFiltroCategoria.BtnFiltroClicked += BtnFiltroCategoriasInsumo_Click;

                    stackPanelFiltrosCategoria.Children.Add(btnFiltroCategoria);
                }
            }
        }

        private void CargarProductos()
        {
            try
            {
                ServicioProductosClient servicioProductos = new ServicioProductosClient();
                _productos = servicioProductos.RecuperarProductosInventariados();

                MostrarMensajeSinProductos(lblSinProductos);

                bool hayExistencia = ValidarExistenciaProductos(_productos);

                if (hayExistencia)
                {
                    MostrarProductos();
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

        private void MostrarMensajeSinProductos(Label labelSinProductos)
        {
            labelSinProductos.Visibility = Visibility.Visible;
        }

        private bool ValidarExistenciaProductos(Producto[] productos)
        {
            bool hayExistencia = false;

            if (productos != null && productos.Count() > 0)
            {
                hayExistencia = true;
            }

            return hayExistencia;
        }

        private void BtnFiltroCategoriasInsumo_Click(object sender, EventArgs e)
        {
            LimpiarFiltrosCategorias();

            BtnFiltro btnFiltroCategoria = (BtnFiltro)sender;
            MostrarFiltroCategoriaSeleccionado(btnFiltroCategoria);

            string categoriaSeleccionada = btnFiltroCategoria.btnFiltro.Content.ToString().ToUpper();
            MostrarCoincidencias(categoriaSeleccionada);
        }

        private void LimpiarFiltrosCategorias()
        {
            foreach (BtnFiltro btnFiltroCategoria in stackPanelFiltrosCategoria.Children)
            {
                btnFiltroCategoria.btnFiltro.Foreground = new SolidColorBrush((Color)ColorConverter.
                    ConvertFromString(FOREGROUND_FILTRO_DESSELECCIONADO));
                btnFiltroCategoria.btnFiltro.Background = new SolidColorBrush(Colors.White);
                btnFiltroCategoria.btnFiltro.BorderBrush = new SolidColorBrush(Colors.White);
            }
        }

        private void MostrarFiltroCategoriaSeleccionado(BtnFiltro btnFiltroCategoria)
        {
            btnFiltroCategoria.btnFiltro.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(FOREGROUND_FILTRO_SELECCIONADO));
            btnFiltroCategoria.btnFiltro.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(BACKGROUND_FILTRO_SELECCIONADO));
            btnFiltroCategoria.btnFiltro.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(BORDERBRUSH_FILTRO_SELECCIONADO));
        }

        private void MostrarCoincidencias(string textoABuscar)
        {
            stackPanelProductos.Children.Clear();

            foreach (Producto producto in _productos)
            {
                string nombre = producto.Nombre.ToUpper();
                string codigo = producto.Codigo.ToUpper();
                string categoria = producto.Insumo.Categoria.Nombre.ToUpper();
                string categoriaProductoVenta = string.Empty;

                if (producto.ProductoVenta != null)
                {
                    categoriaProductoVenta = producto.ProductoVenta.Categoria.Nombre.ToUpper();
                }

                if (nombre.Contains(textoABuscar) || codigo.Contains(textoABuscar) || categoria.Contains(textoABuscar) 
                    || categoriaProductoVenta.Contains(textoABuscar))
                {
                    MostrarProducto(producto);
                }

                if (textoABuscar == FILTRO_TODOS)
                {
                    MostrarProducto(producto);
                }
            }
        }

        private void MostrarProductos()
        {
            lblSinProductos.Visibility = Visibility.Collapsed;

            if (_productos != null && _productos.Count() > 0)
            {
                stackPanelProductos.Children.Clear();

                foreach (Producto producto in _productos)
                {
                    MostrarProducto(producto);
                }
            }
        }

        private void MostrarProducto(Producto producto)
        {
            ElementoValidacionProducto elementoValidacionProducto = CrearElementoValidacionProducto(producto);

            stackPanelProductos.Children.Add(elementoValidacionProducto);
        }

        private ElementoValidacionProducto CrearElementoValidacionProducto(Producto producto)
        {
            ElementoValidacionProducto elementoValidacionProducto = new ElementoValidacionProducto(producto);
            elementoValidacionProducto.TbxCantidadFisicaEnterPressed += TbxCantidadFisicaEnter_Pressed;

            return elementoValidacionProducto;
        }

        private void TbxCantidadFisicaEnter_Pressed(object sender, EventArgs e)
        {
            if (e is KeyEventArgs keyEventArgs && keyEventArgs.Key == Key.Enter)
            {
                ElementoValidacionProducto elementoValidacionProducto = (ElementoValidacionProducto)sender;
                ValidarCantidadProducto(elementoValidacionProducto);
            }
        }

        private void ValidarCantidadProducto(ElementoValidacionProducto elementoValidacionProducto)
        {
            float cantidadRegistrada = (float)elementoValidacionProducto.ProductoAsignado.Insumo.Cantidad;
            float cantidadFisica = float.Parse(elementoValidacionProducto.tbxCantidadFisica.Text);
            float residuo = cantidadRegistrada - cantidadFisica;

            MostraraResultadoValidacion(residuo, elementoValidacionProducto);
        }

        private void MostraraResultadoValidacion(float residuo, ElementoValidacionProducto elementoValidacionProducto)
        {
            TextBlock tbkRespuestaValidacion = elementoValidacionProducto.tbkRespuestaValidacion;
            string unidadMedida = " " + elementoValidacionProducto.lblUnidadMedida.Content.ToString();
            string mensajeCantidadCorrecta = "Cantidad correcta";
            string mensajeSobran = "Sobran ";
            string mensajeFaltan = "Faltan ";
            int convertidorAPositivo = -1;

            switch (residuo)
            {
                case 0:
                    MostrarMensajeValidacion(tbkRespuestaValidacion, mensajeCantidadCorrecta, Colors.Green);
                    break;
                case float n when n > 0:
                    string mensajeSobranCantidad = mensajeSobran + residuo + unidadMedida;
                    MostrarMensajeValidacion(tbkRespuestaValidacion, mensajeSobranCantidad, Colors.Orange);
                    break;
                case float n when n < 0:
                    string mensajeFaltanCantidad = mensajeFaltan + residuo * convertidorAPositivo + unidadMedida;
                    MostrarMensajeValidacion(tbkRespuestaValidacion, mensajeFaltanCantidad, Colors.Red);
                    break;
                default:
                    break;
            }
        }

        private void MostrarMensajeValidacion(TextBlock tbkRespuestaValidacion, string mensaje, Color color)
        {
            tbkRespuestaValidacion.Text = mensaje;
            tbkRespuestaValidacion.Foreground = new SolidColorBrush(color);
        }

        private void TbxBusqueda_TextChanged(object sender, EventArgs e)
        {
            if (barraDeBusqueda.txbBusqueda.Text.Trim() == string.Empty)
            {
                MostrarProductos();
            }
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
            if (barraDeBusqueda.txbBusqueda.Text != string.Empty)
            {
                LimpiarFiltrosCategorias();
                stackPanelProductos.Children.Clear();

                string textoABuscar = barraDeBusqueda.txbBusqueda.Text.Trim().ToUpper();
                MostrarCoincidencias(textoABuscar);
            }
        }

        private void ImgRegresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}