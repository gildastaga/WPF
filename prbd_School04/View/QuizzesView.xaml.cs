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
using PRBD_Framework;
using School04.Model;

namespace School04.View {
    /// <summary>
    /// Logique d'interaction pour QuizzesView.xaml
    /// </summary>
    public partial class QuizzesView : UserControlBase {
        private Course course;
        public QuizzesView() {
            InitializeComponent();
        }

        public QuizzesView(Course course) {
            this.course = course;
            InitializeComponent();
            vm.Init(course);
        }
        private void lsQuizzes_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            vm.DisplayQuizz.Execute(lsQuizzes.SelectedItem);
        }
    }
}
