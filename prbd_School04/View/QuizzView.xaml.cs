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
    /// Logique d'interaction pour QuizzView.xaml
    /// </summary>
    public partial class QuizzView : UserControlBase {
        private Quizz quizz { get; set; }
        public QuizzView() {
            InitializeComponent();
        }
        public QuizzView(Quizz quizz, bool isNew) {
            this.quizz = quizz;
            InitializeComponent();
            vm.Init(quizz, isNew);
        }

        private void lsQuestionsQuizz_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count == 1)
                vm.Weight = e.AddedItems.Cast<QuestionQuizz>().First().NbPoint;
            else
                vm.Weight = 0;
            Weight.Text = ((int)vm.Weight).ToString();
        }
    }
}
