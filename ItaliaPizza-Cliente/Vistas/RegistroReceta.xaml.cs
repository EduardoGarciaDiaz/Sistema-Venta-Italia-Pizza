using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
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
        private int _numeroInsumosSeleccionados = 0;
        private ProductoSinReceta[] _productosSinReceta;
        private InsumoRegistroReceta[] _insumosDisponibles;

        private ElementoProductoSinReceta _elementoProductoSinRecetaSeleccionado;

        public RegistroReceta()
        {
            InitializeComponent();
            CargarProductosSinReceta();
            CargarInsumos();
        }

        private void CargarProductosSinReceta()
        {
            RecuperarProductosSinReceta();

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
                wrapPanelProductosSinReceta.Children.Clear();

                foreach (ProductoSinReceta productoSinReceta in _productosSinReceta)
                {
                    MostrarProductoSinReceta(productoSinReceta);
                }
            }
        }

        private void MostrarProductoSinReceta(ProductoSinReceta productoSinReceta)
        {
            ElementoProductoSinReceta elementoProductoSinReceta = CrearElementoProductoSinReceta(productoSinReceta);

            wrapPanelProductosSinReceta.Children.Add(elementoProductoSinReceta);
        }

        private ElementoProductoSinReceta CrearElementoProductoSinReceta(ProductoSinReceta productoSinReceta)
        {
            ElementoProductoSinReceta elementoProductoSinReceta = 
                new ElementoProductoSinReceta(productoSinReceta);

            elementoProductoSinReceta.gridProductoSinReceta_Click += GridProductoSinReceta_Click;

            return elementoProductoSinReceta;
        }

        private void GridProductoSinReceta_Click(object sender, EventArgs e)
        {
            LimpiarSeleccionProductosSinReceta();

            ElementoProductoSinReceta productoSinReceta = sender as ElementoProductoSinReceta;
            productoSinReceta.rectangleProducto.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(COLOR_HEXADECIMAL_PRODUCTO_SELECCIONADO);

            _elementoProductoSinRecetaSeleccionado = productoSinReceta;
            productoSinReceta.EsSeleccionado = true;
            lbNombreReceta.Content = productoSinReceta.tbkNombre.Text;
        }

        private void LimpiarSeleccionProductosSinReceta()
        {

            foreach (ElementoProductoSinReceta elementopPoducto in wrapPanelProductosSinReceta.Children)
            {
                elementopPoducto.EsSeleccionado = false;

                elementopPoducto.rectangleProducto.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(COLOR_HEXADECIMAL_ELEMENTO_DEFAULT);
            }
        }

        private void CargarInsumos()
        {
            RecuperarInsumos();

            if (_insumosDisponibles != null && _insumosDisponibles.Count() > 0)
            {
                MostrarInsumos();
            }
        }

        private void RecuperarInsumos()
        {
            ServicioProductosClient servicioProductosCliente = new ServicioProductosClient();

            _insumosDisponibles = servicioProductosCliente.RecuperarInsumos();
        }

        private void MostrarInsumos()
        {
            if (_insumosDisponibles != null && _insumosDisponibles.Count() > 0)
            {
                wrapPanelInsumos.Children.Clear();

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

            wrapPanelInsumos.Children.Add(elementoInsumoRegistroReceta);
        }

        private ElementoInsumoRegistroReceta CrearElementoInsumoRegistroReceta(InsumoRegistroReceta insumo)
        {
            ElementoInsumoRegistroReceta elementoInsumoRegistroReceta = 
                new ElementoInsumoRegistroReceta(insumo);

            elementoInsumoRegistroReceta.gridInsumoRegistroReceta_Click += GridInsumoRegistroReceta_Click;

            return elementoInsumoRegistroReceta;
        }

        private void GridInsumoRegistroReceta_Click(object sender, EventArgs e)
        {
            string mensajeErrorSinProducto = "Primero debes seleccionar un producto para guardarle la receta";
            lbErrorRegistro.Visibility = Visibility.Collapsed;

            if (ValidarProductoSeleccionado())
            {
                ElementoInsumoRegistroReceta elementoInsumoRegistroReceta = sender as ElementoInsumoRegistroReceta;

                if (!elementoInsumoRegistroReceta.EsSeleccionado)
                {
                    SeleccionarInsumo(elementoInsumoRegistroReceta);
                    InsumoReceta insumoReceta = CrearInsumoRecetaDesdeElementoInsumoRegistro(elementoInsumoRegistroReceta);
                    MostrarInsumoSeleccionado(insumoReceta);
                }
            }
            else
            {
                lbErrorRegistro.Text = mensajeErrorSinProducto;
                lbErrorRegistro.Visibility = Visibility.Visible;
            }
            
        }

        private void SeleccionarInsumo(ElementoInsumoRegistroReceta elementoInsumoRegistroReceta)
        {
            elementoInsumoRegistroReceta.EsSeleccionado = true;
            _numeroInsumosSeleccionados++;
            ActualizarContadorInsumos();

            elementoInsumoRegistroReceta.rectangleInsumoRegistro.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(COLOR_HEXADECIMAL_INSUMO_SELECCIONADO);
        }

        private void ActualizarContadorInsumos()
        {
            lbContadorInsumos.Content = _numeroInsumosSeleccionados;
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

            wrapPanelInsumosSeleccionados.Children.Add(elementoInsumoSeleccionado);
        }

        private ElementoInsumoSeleccionado CrearElementoInsumoSeleccionado(InsumoReceta insumoReceta)
        {
            ElementoInsumoSeleccionado elementoInsumoSeleccionado =
                new ElementoInsumoSeleccionado(insumoReceta);

            insumoReceta.Cantidad = ConvertirStringAFloat(elementoInsumoSeleccionado.tbxCantidadInsumo.Text.ToString(), null);
            elementoInsumoSeleccionado.InsumoAsignado = insumoReceta;

            elementoInsumoSeleccionado.btnDesasignarInsumo_Click += BtnDesasignarInsumo_Click;

            return elementoInsumoSeleccionado;
        }

        private void BtnDesasignarInsumo_Click(object sender, EventArgs e)
        {
            ElementoInsumoSeleccionado elementoInsumoSeleccionado = sender as ElementoInsumoSeleccionado;

            RemoverInsumoSeleccionado(elementoInsumoSeleccionado);
            HabilitarElementoInsumoRegistroReceta(elementoInsumoSeleccionado.InsumoAsignado.Codigo);
        }

        private void RemoverInsumoSeleccionado(ElementoInsumoSeleccionado elementoInsumoSeleccionado)
        {
            _numeroInsumosSeleccionados--;
            ActualizarContadorInsumos();

            foreach (ElementoInsumoSeleccionado insumoSeleccionado in wrapPanelInsumosSeleccionados.Children)
            {
                if (insumoSeleccionado.InsumoAsignado.Codigo == elementoInsumoSeleccionado.InsumoAsignado.Codigo)
                {
                    wrapPanelInsumosSeleccionados.Children.Remove(insumoSeleccionado);
                    break;
                }
            }
        }

        private void HabilitarElementoInsumoRegistroReceta(string codigo)
        {
            foreach (ElementoInsumoRegistroReceta insumo in wrapPanelInsumos.Children)
            {
                if (insumo.InsumoAsignado.Codigo == codigo)
                {
                    insumo.EsSeleccionado = false;
                    insumo.rectangleInsumoRegistro.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(COLOR_HEXADECIMAL_ELEMENTO_DEFAULT);
                }
            }
        }

        private bool ValidarProductoSeleccionado()
        {
            bool hayProductoSeleccionado = false;

            foreach (ElementoProductoSinReceta producto in wrapPanelProductosSinReceta.Children)
            {
                if (producto.EsSeleccionado)
                {
                    hayProductoSeleccionado = true;
                }
            }

            return hayProductoSeleccionado;
        }

        private bool ValidarInsumoSeleccionado()
        {
            bool hayInsumoSeleccionado = false;
            int sinInsumos = 0;

            if (wrapPanelInsumosSeleccionados.Children.Count > sinInsumos)
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

                InsumoReceta[] insumosSeleccionados = new InsumoReceta[_numeroInsumosSeleccionados];

                for (int i = 0 ; i < _numeroInsumosSeleccionados; i++)
                {
                    ElementoInsumoSeleccionado insumoSeleccionado = wrapPanelInsumosSeleccionados.Children[i] as ElementoInsumoSeleccionado;

                    insumoSeleccionado.InsumoAsignado.Cantidad = ConvertirStringAFloat(insumoSeleccionado.tbxCantidadInsumo.Text, null);

                    InsumoReceta insumoReceta = new InsumoReceta()
                    {
                        Codigo = insumoSeleccionado.InsumoAsignado.Codigo,
                        Cantidad = insumoSeleccionado.InsumoAsignado.Cantidad
                    };

                    insumosSeleccionados[i] = insumoReceta;
                }

                RecetaProducto nuevaReceta = new RecetaProducto(){
                    CodigoProducto = _elementoProductoSinRecetaSeleccionado.lbCodigo.Content.ToString(),
                    InsumosReceta = insumosSeleccionados
                };


                ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();

                filasAfectadas = servicioRecetasCliente.GuardarReceta(nuevaReceta);
            }
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
                lbErrorRegistro.Text = mensajeErrorSinProducto;
                lbErrorRegistro.Visibility = Visibility.Visible;
            }

            if (!ValidarInsumoSeleccionado())
            {
                esGuardadoValido = false;
                lbErrorRegistro.Text = mensajeErrorSinInsumo;
                lbErrorRegistro.Visibility = Visibility.Visible;
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

            foreach(ElementoInsumoSeleccionado insumoSeleccionado in wrapPanelInsumosSeleccionados.Children)
            {
                string cantidad = insumoSeleccionado.tbxCantidadInsumo.Text.Trim();

                float cantidadInsumo = ConvertirStringAFloat(cantidad, insumoSeleccionado.lbErrorInsumoSeleccionado);

                string unidadMedida = (string)insumoSeleccionado.lbUnidadMedida.Content;
                sonCantidadesValidas = ValidarCantidadInsumo(cantidadInsumo, unidadMedida, insumoSeleccionado.lbErrorInsumoSeleccionado);
            }

            return sonCantidadesValidas;
        }

        private bool ValidarCantidadInsumo(float cantidadInsumo, string unidadMedida, Label lbError)
        {
            bool esCantidadValida = true;

            string medidaEnteros = "Unidad";
            string mensajeErrorCantidad = "Cantidad no válida.";

            if (cantidadInsumo > 0 && unidadMedida == medidaEnteros)
            {
                esCantidadValida = ValidarCantidadUnitaria(cantidadInsumo, lbError);
            }
            else if (cantidadInsumo <= 0)
            {
                lbError.Content = mensajeErrorCantidad;
                lbError.Visibility = Visibility.Visible;
                esCantidadValida = false;
            }

            return esCantidadValida;
        }

        private bool ValidarCantidadUnitaria(float cantidadInsumo, Label lbError)
        {
            bool esCantidadValida = true;
            string mensajeErrorCantidad = "Datos no válidos. Deben ser números enteros";

            if (cantidadInsumo % 1 != 0)
            {
                lbError.Content = mensajeErrorCantidad;
                lbError.Visibility = Visibility.Visible;
                esCantidadValida = false;
            }

            return esCantidadValida;
        }

        private float ConvertirStringAFloat(string numeroEnString, Label lbError)
        {
            float numeroConvertido = -1;

            try
            {
                numeroConvertido = Convert.ToSingle(numeroEnString);
            }
            catch (FormatException)
            {
                string mensajeErrorFormato = "Solo puede incluir números. Ej. 2, .5";
                lbError.Content = mensajeErrorFormato;
                lbError.Visibility = Visibility.Visible;
            }
            catch (OverflowException)
            {
                string mensajeErrorOverFlow = "Cantidad no permitida, por favor corrigelo.";
                lbError.Content = mensajeErrorOverFlow;
                lbError.Visibility = Visibility.Visible;
            }

            return numeroConvertido;
        }

        private void LimpiarMensajesError()
        {
            lbErrorRegistro.Visibility = Visibility.Collapsed;
            foreach(ElementoInsumoSeleccionado insumoSeleccionado in wrapPanelInsumosSeleccionados.Children)
            {
                insumoSeleccionado.lbErrorInsumoSeleccionado.Visibility = Visibility.Collapsed;
            }
        }


        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }
    }
}
