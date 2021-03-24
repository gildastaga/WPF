using System.Windows;
using PRBD_Framework;

namespace Msn.View {
    public partial class LoginView : WindowBase {
        public LoginView() {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Vm_OnLoginSuccess() {
            App.NavigateTo<MainView>();
        }

        private void txtPseudo_GotFocus(object sender, RoutedEventArgs e) {
            txtPseudo.SelectAll();
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e) {
            txtPassword.SelectAll();
        }
    }
}
