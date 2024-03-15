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
    /// Lógica de interacción para BarraDeBusqueda.xaml
    /// </summary>
    public partial class BarraDeBusqueda : UserControl
    {
        public EventHandler ImgBuscarClicked;

        public BarraDeBusqueda()
        {
            InitializeComponent();
        }

        private void ImgBuscar_Click(object sender, MouseButtonEventArgs e)
        {
            ImgBuscarClicked?.Invoke(this, e);
        }
    }
}
