using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Examen.Views.VentasView
{
    /// <summary>
    /// Lógica de interacción para VentasMianView.xaml
    /// </summary>
    public partial class VentasMianView : Window
    {
        public VentasMianView()
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            //verifica si la venta esta en estado normal, de lo contralo la maximiza.
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }
        private void OpenClientesWindow(object sender, RoutedEventArgs e)
        {
            Views.ClientesView.ClientesMainView clientesMainView = new Views.ClientesView.ClientesMainView();
            clientesMainView.ShowDialog(); // Esto abrirá la ventana secundaria de forma modal
        }

        private void OpenHomeWindow(object sender, RoutedEventArgs e)
        {
            // Abre la ventana de MainWindow sin cerrar la aplicación actual
            var homeMainView = new MainWindow();
            homeMainView.Show();
            this.Hide(); // Opcional: oculta la ventana actual en lugar de cerrarla
        }
    }
}
