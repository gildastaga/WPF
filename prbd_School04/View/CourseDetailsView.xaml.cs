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
    /// Logique d'interaction pour CourseDetailsView.xaml
    /// </summary>
    public partial class CourseDetailsView : UserControlBase {
        private Course course;
        public CourseDetailsView() {
            InitializeComponent();
        }

        public CourseDetailsView(Course course, bool isNew) {
            InitializeComponent();
            if (course != null) {
                this.course = course;
                tabControl.Add(
                       new RegistrationsView(course),
                       "Registrations", "registationsTab"
                );
                tabControl.Add(
                       new CategoryView(course),
                       "Categories", "categoriesTab"
                );
                tabControl.Add(
                       new QuestionView(course),
                       "Questions", "questionsTab"
                );
                tabControl.Add(
                       new QuizzesView(course),
                       "Quizzes", "quizzesTab"
                );
                /*tabControl.Add(
                       new registrationsView(course),
                       "Registrations", "registationsTab"
                );*/
            }
        }
    }
}
