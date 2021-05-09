using PRBD_Framework;
using School04.Model;
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

namespace School04.View {
    /// <summary>
    /// Logique d'interaction pour MainView.xaml
    /// </summary>
    public partial class MainView : WindowBase {
        public MainView() {
            InitializeComponent();
        }

        private void MenuItem_Click( object sender, RoutedEventArgs e ) {
            Close();
        }

        private void Vm_DisplayCourse(Course course, bool isNew) {
            if (course != null) {
                var tab = tabControl.FindByTag(course.Title);
                if (tab == null)
                    tabControl.Add(
                        new CourseDetailsView(course, isNew),
                        isNew ? "<new course>" : course.Title, course.Title
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        private void Vm_DisplayQuizz(Quizz quizz, bool isNew) {
            if (quizz != null) {
                var tab = tabControl.FindByTag(quizz.QuizzId.ToString());
                if (tab == null)
                    tabControl.Add(
                        new QuizzView(quizz, isNew),
                        isNew ? "<new quizz>" : quizz.Title, quizz.QuizzId.ToString()
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        private void Vm_RenameTabQuizz(Quizz quizz, string header) {
            var tab = tabControl.SelectedItem as TabItem;
            if (tab != null) {
                tab.Header = tab.Tag = header = string.IsNullOrEmpty(header) ? "<new quizz>" : header;
            }
        }

        private void Vm_CloseTabQuizz(Quizz quizz) {
            var tab = tabControl.FindByTag(quizz.Title);
            tabControl.Items.Remove(tab);
        }
    }
}
