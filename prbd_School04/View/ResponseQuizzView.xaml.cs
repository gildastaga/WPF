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
    /// Logique d'interaction pour ResponseQuizzView.xaml
    /// </summary>
    public partial class ResponseQuizzView : UserControlBase {
        private Quizz quizz { get; set; }
        public ResponseQuizzView() {
            InitializeComponent();
        }

        public ResponseQuizzView(Quizz quizz, bool isNew) {
            this.quizz = quizz;
            InitializeComponent();
            vm.Init(quizz, isNew);
        }
    }
    public class MyDataTemplateSelector : DataTemplateSelector {
        public DataTemplate Radio { get; set; }
        public DataTemplate Check { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            Question question = ((Proposition)item).Question;
            if (question.typeQuestion == TypeQuestion.OneAnswer)
                return Radio;
            else
                return Check;
        }
    }
}
