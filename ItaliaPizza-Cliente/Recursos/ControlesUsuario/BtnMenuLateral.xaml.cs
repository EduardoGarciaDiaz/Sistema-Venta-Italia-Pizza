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
    /// Lógica de interacción para BtnMenuLateral.xaml
    /// </summary>
    public partial class BtnMenuLateral : UserControl
    {
        public event RoutedEventHandler BtnMenuLateralClicked;

        public BtnMenuLateral()
        {
            InitializeComponent();
        }

        private void UcBtnMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BtnMenuLateralClicked?.Invoke(this, e);
        }
    }
}
