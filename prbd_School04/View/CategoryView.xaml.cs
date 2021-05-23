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
    /// Logique d'interaction pour CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControlBase {
        private Course course;

        public CategoryView() {
            //InitializeComponent();
            //vmc.Init(category, isNew);
        }

        public CategoryView(Course course) {
            this.course = course;
            InitializeComponent();
        }

        private void save_Click(object sender, RoutedEventArgs e) {
            string Name = tb_name.Text;
            string enonce = tb_question.Text;
            Question question = App.Context.Questions.Where(q => q.Enonce == enonce).FirstOrDefault();
            // App.NavigateTo<MainView>();

        }

        private void delete_Click(object sender, RoutedEventArgs e) {
            dataGrid.Items.Clear();
        }

        private void Vm_OnCategorySuccess(object sender, RoutedEventArgs e) {

        }
    }
}
