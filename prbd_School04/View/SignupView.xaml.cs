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
using System.Windows.Shapes;
using PRBD_Framework;

namespace School04.View {
    /// <summary>
    /// Logique d'interaction pour SignupView.xaml
    /// </summary>
    public partial class SignupView : WindowBase {
        public SignupView() {
            InitializeComponent();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
        private void txtFirstName_GotFocus(object sender, RoutedEventArgs e) {
            txtFirstName.SelectAll();
        }

        private void txtLastName_GotFocus(object sender, RoutedEventArgs e) {
            txtLastName.SelectAll();
        }
        private void txtMail_GotFocus(object sender, RoutedEventArgs e) {
            txtMail.SelectAll();
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e) {
            txtPassword.SelectAll();
        }
        private void txtPasswordConfirm_GotFocus(object sender, RoutedEventArgs e) {
            txtPasswordConfirm.SelectAll();
        }
        private void Vm_OnSignupSuccess() {
            if (App.CurrentUser != null)
                App.NavigateTo<TeacherCoursesView>();
        }
    }
}
