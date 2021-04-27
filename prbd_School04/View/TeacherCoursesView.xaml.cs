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
using System.Windows.Shapes;

namespace School04.View {
    /// <summary>
    /// Logique d'interaction pour MainTeacherViewxaml.xaml
    /// </summary>
    public partial class TeacherCoursesView : WindowBase {
        public List<Course> lsCourses = new List<Course>();
        public TeacherCoursesView() {
            InitializeComponent();
            //MakeList();
        }
        //initialiser la liste lors de la construction de la fenêtre
        /*private void Clear() {
            lvCourses.Items.Clear();
            txtBoxTitle.Clear();
            txtBoxDescription.Clear();
        }
        private void MakeList() {
            Clear();
            foreach (var c in lsCourses)
                lvCourses.Items.Add(c);
            lvCourses.SelectedIndex = 0; //On sélectionne le premier
        }*/
        private bool modAdd = true; 
        private void btcNew_Click( object sender, RoutedEventArgs e ) {
            txtBoxTitle.Clear();
            txtBoxDescription.Clear();
        }

        private void lvCourses_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            var c = (Course)lvCourses.SelectedItem; // on récupère la ligne sélectionnée de la ListView
            if (c == null)
                return;
            txtBoxTitle.Text = c.Title;
            txtBoxDescription.Text = c.Description;
        }

        private void btcAdd_Click( object sender, RoutedEventArgs e ) {
            if (modAdd) {
                lsCourses.Add(
                    new Course {
                        Title = txtBoxTitle.Text,
                        Description = txtBoxDescription.Text
                    });
                modAdd = false;
            } else {
                var c = (Course)lvCourses.SelectedItem;
                if (c == null)
                    return;
                c.Title = txtBoxTitle.Text;
                c.Description = txtBoxDescription.Text;
            }
        }

        private void btcSuppr_Click( object sender, RoutedEventArgs e ) {
            var c = (Course)lvCourses.SelectedItem;
            if (c == null)
                return;
            lsCourses.Remove(c);
        }
    }
}
