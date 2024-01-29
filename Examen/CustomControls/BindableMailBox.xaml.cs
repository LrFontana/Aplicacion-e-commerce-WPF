using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

namespace Examen.CustomControls
{
    /// <summary>
    /// Lógica de interacción para BindableMailBox.xaml
    /// </summary>
    public partial class BindableMailBox : UserControl
    {
        //Propiedad.
        public static readonly DependencyProperty MailProperty = DependencyProperty.Register("Mail", typeof(SecureString), typeof(BindableMailBox));

        public SecureString Mail
        {
            get { return (SecureString)GetValue(MailProperty); }
            set { SetValue(MailProperty, value); }
        }
        public BindableMailBox()
        {
            InitializeComponent();
            txtMailPass.PasswordChanged += OnMailChanged;
        }

        private void OnMailChanged(object sender, RoutedEventArgs e)
        {
            Mail = txtMailPass.SecurePassword;
        }
    }
}
