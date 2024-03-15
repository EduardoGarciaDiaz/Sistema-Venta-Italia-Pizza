using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para Recetas.xaml
    /// </summary>
    public partial class Recetas : Page
    {
        public Recetas()
        {
            InitializeComponent();
            CargarRecetas();
        }

        private void CargarRecetas()
        {
            Receta[] recetas = RecuperarRecetas();

            if (recetas != null)
            {
                MostrarRecetas(recetas);
            }
            else
            {
                //Mostrar mensaje sin recetas
            }
        }

        private Receta[] RecuperarRecetas()
        {
            ServicioRecetasClient servicioRecetasCliente = new ServicioRecetasClient();

            return servicioRecetasCliente.RecuperarRecetas();
        }

        private void MostrarRecetas(Receta[] recetas)
        {
            foreach (Receta receta in recetas)
            {
                ElementoReceta elementoReceta = new ElementoReceta();
                elementoReceta.lblNombreReceta.Content = receta.Nombre;

                BitmapImage foto = ConvertidorBytes.ConvertirBytesABitmapImage(receta.FotoProducto);

                if (foto != null)
                {
                    elementoReceta.imgFotoProductoReceta.Source = foto;
                }

                wrapPanelRecetas.Children.Add(elementoReceta);
            }
        }

        
    }
}
