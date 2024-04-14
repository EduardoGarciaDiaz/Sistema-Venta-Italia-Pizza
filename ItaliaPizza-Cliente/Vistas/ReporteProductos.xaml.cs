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
using System.Windows.Shapes;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Interaction logic for ReporteProductos.xaml
    /// </summary>
    public partial class ReporteProductos : Window
    {
        List<Categoria> categoriasInsumos = new List<Categoria>();
        List<Categoria> categoriasProductosVenta = new List<Categoria>();
        public ReporteProductos()
        {
            InitializeComponent();
            this.Loaded += PrepararVentana;

        }

        private void PrepararVentana(object sender, RoutedEventArgs e)
        {
            ServicioProductosClient prodcutosCliente = new ServicioProductosClient();
            categoriasInsumos = prodcutosCliente.RecuperarCategorias().ToList();
            categoriasProductosVenta = prodcutosCliente.RecuperarCategoriasProductoVenta().ToList();
        }

        private void CargarCheckBoxes(List<Categoria> categoriasInsumo, List<Categoria> categoriasVenta)
        {
            foreach (var item in categoriasInsumo)
            {
                CheckBox categoria = new CheckBox();
                categoria.Content = item.Nombre;
                categoria.Margin = new Thickness(0,5,0,5);
                categoria.HorizontalAlignment = HorizontalAlignment.Center;
                stpInsumos.Children.Add(categoria);
            }
            foreach (var item in categoriasProductosVenta)
            {
                CheckBox categoria = new CheckBox();
                categoria.Content = item.Nombre;
                categoria.Margin = new Thickness(0, 5, 0, 5);
                categoria.HorizontalAlignment = HorizontalAlignment.Center;
                stpProductosVenta.Children.Add(categoria);
            }
        }

        private void BtnTodos_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> categoriassCheckBox = ObtenerCheckBoxCategorias();
            bool almenosUnaCategoria = ValidarMinimoDeCategoriasSeleccionadas(categoriassCheckBox);
            CambiarSeleccionTodasLasCategorias(categoriassCheckBox, almenosUnaCategoria);
        }

        private List<CheckBox> ObtenerCheckBoxCategorias()
        {
            List<CheckBox> categoriassCh = new List<CheckBox>();
            foreach (var item in stpProductosVenta.Children)
            {
                if(item is CheckBox)
                {
                    categoriassCh.Add((CheckBox)item);
                }
            }
            foreach (var item in stpInsumos.Children)
            {
                if (item is CheckBox)
                {
                    categoriassCh.Add((CheckBox)item);
                }
            }
            return categoriassCh;
        }

        private bool ValidarMinimoDeCategoriasSeleccionadas(List<CheckBox> categoriassCheckBox)
        {
            bool validar = false;
            foreach (var item in categoriassCheckBox)
            {
                if ((bool)item.IsChecked)
                {
                    validar = true;
                    break;
                }
            }
            return validar;
        }

        private void CambiarSeleccionTodasLasCategorias(List<CheckBox> categoriassCheckBox, bool nuevoEstado)
        {
            foreach (var item in categoriassCheckBox)
            {
               item.IsChecked = nuevoEstado;
            }
        }      

        private void BtnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            List<Categoria> categoriasSeleccioandas = ObtenerCategoriasSelccionadas();
            if(categoriasSeleccioandas.Count != 0)
            {
                bool incluirAgotados = chbAgotados.IsChecked.Value;
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "No se puede generar un reporte vacio, por favor selecciona al menos una categoría", Window.GetWindow(this), 1);
            }

        }

        private List<Categoria> ObtenerCategoriasSelccionadas()
        {
            List<Categoria> categoriassCh = new List<Categoria>();
            foreach (var item in stpProductosVenta.Children)
            {
                if (item is CheckBox)
                {
                    CheckBox categoria = (CheckBox) item;
                    if ((bool)categoria.IsChecked)
                    {
                        categoriassCh.Add(categoriasProductosVenta.FirstOrDefault(cat => cat.Nombre.Equals(categoria.Name)));
                    }
                }
            }
            foreach (var item in stpInsumos.Children)
            {
                if (item is CheckBox)
                {
                    CheckBox categoria = (CheckBox)item;
                    if ((bool)categoria.IsChecked)
                    {
                        categoriassCh.Add(categoriasInsumos.FirstOrDefault(cat => cat.Nombre.Equals(categoria.Name)));
                    }
                }
            }
            return categoriassCh;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
