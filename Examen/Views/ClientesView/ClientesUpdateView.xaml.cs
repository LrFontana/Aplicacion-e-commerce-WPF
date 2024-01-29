﻿using Examen.ViewModels.ClientesViewModels;
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

namespace Examen.Views.ClientesView
{
    /// <summary>
    /// Lógica de interacción para ClientesUpdateView.xaml
    /// </summary>
    public partial class ClientesUpdateView : UserControl
    {
        public ClientesUpdateView()
        {
            InitializeComponent();
            DataContext = new ClientesUpdateViewModel();
        }
    }
}