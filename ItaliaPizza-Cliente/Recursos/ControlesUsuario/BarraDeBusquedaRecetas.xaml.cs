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
    /// Lógica de interacción para BarraDeBusquedaRecetas.xaml
    /// </summary>
    public partial class BarraDeBusquedaRecetas : UserControl
    {
        public EventHandler tbxBusqueda_TextChanged;
        public EventHandler imgBuscar_Click;
        public EventHandler enter_Pressed;

        public BarraDeBusquedaRecetas()
        {
            InitializeComponent();
        }

        public void TbxBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbxBusqueda_TextChanged?.Invoke(this, e);
        }

        public void ImgBuscar_Click(object sender, RoutedEventArgs e)
        {
            imgBuscar_Click?.Invoke(this, e);
        }

        public void Enter_Pressed(object sender, KeyEventArgs e)
        {
            enter_Pressed?.Invoke(this, e);
        }
    }
}
