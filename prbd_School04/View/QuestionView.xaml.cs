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

    public partial class QuestionView : UserControlBase {
        private Course course;

        public QuestionView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public QuestionView(Course course)
        {
            this.course = course;
            InitializeComponent();
            vm.Init(course);
        }

        private void charge_Question(object sender, RoutedEventArgs e)
        {
            vm.Check.Execute(listQuestions.SelectedItem);
        }

        private void Cancell_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
