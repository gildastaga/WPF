using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    public class QuizzesViewModel : ViewModelCommon {
        public QuizzesViewModel() : base() {
            Quizzes = new ObservableCollectionFast<Quizz>(Quizz.GetAll());
        }
        private ObservableCollectionFast<Quizz> quizzes;
        public ObservableCollectionFast<Quizz> Quizzes {
            get { return quizzes; }
            set {
                quizzes = value;
                RaisePropertyChanged(nameof(Quizzes), nameof(QuizzesView));
            }
        }
        public ICollectionView QuizzesView => Quizzes.GetCollectionView(nameof(DateTime), ListSortDirection.Descending);
    }
}
