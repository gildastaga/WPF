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
                        new EditCourseView(course, isNew),
                        isNew ? "<new course>" : course.Title, course.Title //header, tag
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }
    }
}
