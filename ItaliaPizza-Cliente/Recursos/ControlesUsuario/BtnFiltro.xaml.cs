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

namespace ItaliaPizza_Cliente.Recursos.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para BtnFiltro.xaml
    /// </summary>
    public partial class BtnFiltro : UserControl
    {
        public EventHandler BtnFiltroClicked;


        public BtnFiltro()
        {
            InitializeComponent();
        }

        private void BtnFiltro_Click(object sender, RoutedEventArgs e)
        {
            BtnFiltroClicked?.Invoke(this, e);
        }
    }
}
