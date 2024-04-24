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
    public partial class BarraDeBusquedaMini : UserControl
    {
        public EventHandler TbxBusquedaTextChanged;
        public EventHandler ImgBuscarClicked;
        public EventHandler EnterPressed;

        public BarraDeBusquedaMini()
        {
            InitializeComponent();
        }

        public void TbxBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            TbxBusquedaTextChanged?.Invoke(this, e);
        }

        public void ImgBuscar_Click(object sender, RoutedEventArgs e)
        {
            ImgBuscarClicked?.Invoke(this, e);
        }

        public void Enter_Pressed(object sender, KeyEventArgs e)
        {
            EnterPressed?.Invoke(this, e);
        }
    }
}
