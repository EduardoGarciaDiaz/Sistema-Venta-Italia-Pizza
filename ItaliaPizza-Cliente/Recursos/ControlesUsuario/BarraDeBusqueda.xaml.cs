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
        public EventHandler TxtBusquedaTextChanged;
        public EventHandler EnterPressed;

        public BarraDeBusqueda()
        {
            InitializeComponent();
        }

        private void ImgBuscar_Click(object sender, MouseButtonEventArgs e)
        {
            ImgBuscarClicked?.Invoke(this, e);
        }

        private void TxtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            TxtBusquedaTextChanged?.Invoke(this, e);
        }

        private void Enter_Pressed(object sender, KeyEventArgs e)
        {
            EnterPressed?.Invoke(this, e);
        }
    }
}
