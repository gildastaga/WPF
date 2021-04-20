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
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : WindowBase {
        public LoginView() {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void txtMail_GotFocus(object sender, RoutedEventArgs e) {
            txtMail.SelectAll();
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e) {
            txtPassword.SelectAll();
        }
        private void Vm_OnLoginSuccess() {
            if(App.CurrentUser != null && App.CurrentUser.IsTeacher())
                App.NavigateTo<TeacherCoursesView>();
            else if(App.CurrentUser != null && !App.CurrentUser.IsTeacher())
                App.NavigateTo<MainStudentView>();
        }
    }
}
