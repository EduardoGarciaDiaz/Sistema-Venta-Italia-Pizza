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
        public EventHandler gridReceta_Click;
        public EventHandler imgEditar_Click;

        public ElementoReceta()
        {
            InitializeComponent();
        }


        public void GridReceta_Click(object sender, RoutedEventArgs e)
        {
            gridReceta_Click?.Invoke(this, e);
        }

        public void ImgEditar_Click(object sender, RoutedEventArgs e)
        {
            imgEditar_Click?.Invoke(this, e);
        }
    }
}
