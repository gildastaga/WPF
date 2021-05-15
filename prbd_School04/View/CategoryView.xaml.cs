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
            InitializeComponent();
            //vmc.Init(category, isNew);
        }

        public CategoryView(Course course) {
            this.course = course;
            InitializeComponent();
        }

        private void bt_save_Click(object sender, RoutedEventArgs e) {
            string name = tb_name.Text;
            //int question = tb_question.CaretIndex;
           // Question question = tb_question;

            //Category category = new Category(name, question);
           // dataGrid.Items.Add(category);
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e) {
            
        }

        private void bt_delete_Click(object sender, RoutedEventArgs e) {
            string name = tb_name.Text;
            //int question = tb_question.CaretIndex;
            //Question question = tb_question;

            //Category category = new Category(name, question);
           // dataGrid.Items.Remove(category);
        }
    }
}
