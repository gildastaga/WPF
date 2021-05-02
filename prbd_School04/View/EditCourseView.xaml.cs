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
    /// Logique d'interaction pour EditCourseView.xaml
    /// </summary>
    public partial class EditCourseView : UserControlBase {
        public EditCourseView() {
            InitializeComponent();
        }

        public EditCourseView(Course course, bool isNew) {
            InitializeComponent();
        }
    }
}
