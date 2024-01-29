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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Examen.Views.ClientesView;
using Examen.ViewModels;
using Examen.Views.ProductosView;
using Examen.Views.VentasView;
using Examen.Views.VentasItemsView;

namespace Examen
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Constructor.
        public MainWindow()
        {
            InitializeComponent();            
        }

        //Metodos.

        [DllImport("user32.dll")] //me permite en este casu capturar la señal del mouse para mover/arrastrar la ventana.
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam); //requiere como parametros : identificador de la ventana, un mensaje y dos parametros mas para info adicional del mensaje.

        private void pnlControlbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0); //con estos valores notifica al controlador de la ventana que tiene que ser arrastrado cuando se mueva el mouse sin soltar el click izquierdo.
        }

        private void pnlControlbar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        //Boton Close.
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Boton Min.
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //Boton Max.
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            //verifica si la venta esta en estado normal, de lo contralo la maximiza.
            if (this.WindowState==WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        //Boton Clientes View.
        private void OpenClientesWindow(object sender, RoutedEventArgs e)
        {            
            // Abre la ventana de ClientesMainView
            var clientesMainView = new ClientesMainView();
            this.Close();
            clientesMainView.ShowDialog();
        }

        //Boton Producto.
        private void OpenProductosWindow(object sender, RoutedEventArgs e)
        {
            // Abre la ventana de ProductosMainView
            var productosMainView = new ProductosMainView();
            this.Close();
            productosMainView.ShowDialog();
        }

        //Boton Ventas.
        private void OpenVentasWindow(object sender, RoutedEventArgs e)
        {
            // Abre la ventana de VentasMainView
            var ventasMainView = new VentasMianView();
            this.Close();
            ventasMainView.ShowDialog();
        }

        //Boton Ventas Items.
        private void OpenVentasItemsView(object sender, RoutedEventArgs e)
        {
            // Abre la ventana de VentasItemsMainView
            var ventasItemsMainView = new VentasItemsMainView();
            this.Close();
            ventasItemsMainView.ShowDialog();
        }
    }
}
