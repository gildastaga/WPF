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
    /// Logique d'interaction pour NewQuestionView.xaml
    /// </summary>
    public partial class NewQuestionView : UserControlBase {
        private Question question1 { get; set; }

        public NewQuestionView() {
            InitializeComponent();
        }

        public NewQuestionView(Question question, bool isNew) {
            this.question1 = question;
            InitializeComponent();
            //vm.Init(question, isNew);
        }
    }

}
