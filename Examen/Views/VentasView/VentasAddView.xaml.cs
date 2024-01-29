using System;
using System.Collections.Generic;
using Examen.ViewModels.VentasViewModels;
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

namespace Examen.Views.VentasView
{
    /// <summary>
    /// Lógica de interacción para VentasAddView.xaml
    /// </summary>
    public partial class VentasAddView : UserControl
    {
        public VentasAddView()
        {
            InitializeComponent();
            DataContext = new VentasAddViewModel();
        }

        
    }
}
