using ItaliaPizza_Cliente.ServicioItaliaPizza;
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

namespace ItaliaPizza_Cliente.Recursos.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para ElementoReceta.xaml
    /// </summary>
    public partial class ElementoReceta : UserControl
    {
        public Receta RecetaAsignada { get; set; }
        public EventHandler GrdRecetaClicked;
        public EventHandler ImgEditarClicked;
        public EventHandler ImgEliminarClicked;


        public ElementoReceta()
        {
            InitializeComponent();
        }

        public void GrdReceta_Click(object sender, RoutedEventArgs e)
        {
            GrdRecetaClicked?.Invoke(this, e);
        }

        public void ImgEditar_Click(object sender, RoutedEventArgs e)
        {
            ImgEditarClicked?.Invoke(this, e);
        }

        public void ImgEliminar_Click(object sender, RoutedEventArgs e)
        {
            ImgEliminarClicked?.Invoke(this, e);
        }
    }
}
