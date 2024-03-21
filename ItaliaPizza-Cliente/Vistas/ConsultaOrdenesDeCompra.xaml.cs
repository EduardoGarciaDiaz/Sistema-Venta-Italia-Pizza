﻿using System;
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

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Interaction logic for ConsultaOrdenesDeCompra.xaml
    /// </summary>
    public partial class ConsultaOrdenesDeCompra : Page
    {
        public ConsultaOrdenesDeCompra()
        {
            InitializeComponent();
        }

        private void BtnRegistrarOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            RegistroOrdenCompra paginaRegistrarOrdenCompra = new RegistroOrdenCompra();
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
            ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaRegistrarOrdenCompra);
        }
    }
}