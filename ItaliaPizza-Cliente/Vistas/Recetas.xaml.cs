using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para Recetas.xaml
    /// </summary>
    public partial class Recetas : Page
    {
        private Receta[] _recetas;

        public Recetas()
        {
            InitializeComponent();
            CargarRecetas();
            AgregarEventos();
        }

        private void AgregarEventos()
        {
            barraDeBusquedaRecetas.tbxBusqueda_TextChanged += TbxBusqueda_TextChanged;
            barraDeBusquedaRecetas.imgBuscar_Click += ImgBuscar_Click;
            barraDeBusquedaRecetas.enter_Pressed += Enter_Pressed;
        }

        private void CargarRecetas()
        {
            RecuperarRecetas();
            int sinRecetas = 0;

            if (_recetas != null && _recetas.Count() > sinRecetas)
            {
                lbSinRecetas.Visibility = Visibility.Collapsed;
                MostrarRecetas();
            }
            else
            {
                lbSinRecetas.Visibility = Visibility.Visible;
            }
        }

        private void RecuperarRecetas()
        {
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();

            try
            {
                _recetas = servicioRecetasCliente.RecuperarRecetas();
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

        private void MostrarRecetas()
        {
            if (_recetas != null && _recetas.Count() > 0)
            {
                wrapPanelRecetas.Children.Clear();

                foreach (Receta receta in _recetas)
                {
                    MostrarReceta(receta);
                }
            }
        }

        private void MostrarReceta(Receta receta)
        {
            ElementoReceta elementoReceta = CrearElementoReceta(receta);

            wrapPanelRecetas.Children.Add(elementoReceta);
        }

        private ElementoReceta CrearElementoReceta(Receta receta)
        {
            ElementoReceta elementoReceta = new ElementoReceta();
            elementoReceta.lbNombreReceta.Content = receta.Nombre;
            elementoReceta.Tag = receta.Id;

            BitmapImage foto = ConvertidorBytes.ConvertirBytesABitmapImage(receta.FotoProducto);

            if (foto != null)
            {
                elementoReceta.imgFotoProductoReceta.Source = foto;
            }

            elementoReceta.gridReceta_Click += ElementoReceta_Click;
            elementoReceta.imgEditar_Click += ImgEditarReceta_Click;

            return elementoReceta;
        }

        private void ElementoReceta_Click(object sender, EventArgs e)
        {
            ElementoReceta recetaSeleccionada = sender as ElementoReceta;
            gridInsumosReceta.Visibility = Visibility.Visible;
            int idReceta = (int)recetaSeleccionada.Tag;

            lbNombreReceta.Content = recetaSeleccionada.lbNombreReceta.Content;

            CargarInsumosReceta(idReceta);
        }

        private void CargarInsumosReceta(int idReceta)
        {
            InsumoReceta[] insumosReceta = RecuperarInsumosReceta(idReceta);
            MostrarInsumosReceta(insumosReceta);
        }

        private InsumoReceta[] RecuperarInsumosReceta(int idReceta)
        {
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();
            InsumoReceta[] insumosReceta = new InsumoReceta[0];

            if (idReceta > 0)
            {
                try
                {
                    insumosReceta = servicioRecetasCliente.RecuperarInsumosReceta(idReceta);
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

            return insumosReceta;
        }

        private void MostrarInsumosReceta(InsumoReceta[] insumosReceta)
        {
            stackPanelInsumos.Children.Clear();

            if (insumosReceta != null && insumosReceta.Count() > 0)
            {
                foreach (InsumoReceta insumo in insumosReceta)
                {
                    ElementoInsumoReceta elementoInsumoReceta = CrearElementoInsumoReceta(insumo);

                    stackPanelInsumos.Children.Add(elementoInsumoReceta);
                }
            }
        }

        private ElementoInsumoReceta CrearElementoInsumoReceta(InsumoReceta insumo)
        {
            ElementoInsumoReceta elementoInsumoReceta = new ElementoInsumoReceta();

            elementoInsumoReceta.lbNombreInsumo.Content = insumo.Nombre;
            elementoInsumoReceta.lbCantidadInsumo.Content = insumo.Cantidad;
            elementoInsumoReceta.lbUnidadMedidaInsumo.Content = insumo.UnidadMedida.Nombre;

            return elementoInsumoReceta;
        }

        private void ImgBuscar_Click(object sender, EventArgs e)
        {
            BuscarReceta();
        }

        private void Enter_Pressed(object sender, EventArgs e)
        {
            if (e is KeyEventArgs keyEventArgs && keyEventArgs.Key == Key.Enter)
            {
                BuscarReceta();
            }
        }

        private void BuscarReceta()
        {
            if (barraDeBusquedaRecetas.tbxBusqueda.Text != string.Empty)
            {
                wrapPanelRecetas.Children.Clear();

                string textoABuscar = barraDeBusquedaRecetas.tbxBusqueda.Text.Trim().ToUpper();
                MostrarCoincidencias(textoABuscar);
            }
        }

        private void MostrarCoincidencias(string textoABuscar)
        {
            foreach (Receta receta in _recetas)
            {
                if (receta.Nombre.ToUpper().Contains(textoABuscar))
                {
                    MostrarReceta(receta);
                }
            }
        }

        private void TbxBusqueda_TextChanged(object sender, EventArgs e)
        {
            if (barraDeBusquedaRecetas.tbxBusqueda.Text.Trim() == string.Empty)
            {
                MostrarRecetas();
            }
        }

        private void ImgCerrarInsumos_Click(object sender, RoutedEventArgs e)
        {
            stackPanelInsumos.Children.Clear();
            gridInsumosReceta.Visibility = Visibility.Collapsed;
        }

        private void BtnRegistrarReceta_Click(object sender, RoutedEventArgs e)
        {
            // TODO
            //NavigationService.Navigate(new RegistroReceta());
        }

        private void ImgEditarReceta_Click(object sender, EventArgs e)
        {
            // TODO
        }


    }
}
