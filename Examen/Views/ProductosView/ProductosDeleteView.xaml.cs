using Examen.ViewModels.ProductosViewModels;
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

namespace Examen.Views.ProductosView
{
    /// <summary>
    /// Lógica de interacción para ProductosDeleteView.xaml
    /// </summary>
    public partial class ProductosDeleteView : UserControl
    {
        public ProductosDeleteView()
        {
            InitializeComponent();
            DataContext = new ProductosDeleteViewModel();
        }
    }
}
