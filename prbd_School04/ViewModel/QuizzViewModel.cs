using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    class QuizzViewModel : ViewModelCommon {
        private Quizz quizz;
        public Quizz Quizz { get => quizz; set => SetProperty(ref quizz, value); }

        private bool isNew;
        public bool IsNew {
            get { return isNew; }
            set {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
            }
        }

        private ObservableCollectionFast<QuestionQuizz> questions = new ObservableCollectionFast<QuestionQuizz>();
        public ObservableCollectionFast<QuestionQuizz> Questions {
            get { return questions; }
            set {
                questions = value;
                RaisePropertyChanged(nameof(Questions), nameof(QuizzesView));
            }
        }
        public ICollectionView QuizzesView => Questions.GetCollectionView(nameof(DateTime), ListSortDirection.Descending);


        public void Init(Quizz quizz, bool isNew) {
            Quizz = quizz;
            IsNew = isNew;
            Questions = new ObservableCollectionFast<QuestionQuizz>(QuestionQuizz.GetQuestionsFromQuizz(Quizz));

            RaisePropertyChanged();
        }

        public int? QuizzId {
            get { return Quizz?.QuizzId; }
        }

        public string Title {
            get { return Quizz?.Title; }
            set {
                Quizz.Title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        public DateTime? StartDate {
            get { return Quizz?.ExaminationStartDate; }
            set {
                Quizz.ExaminationStartDate = value;
                RaisePropertyChanged(nameof(StartDate));
            }
        }

        public DateTime? EndDate {
            get { return Quizz?.ExaminationEndDate; }
            set {
                Quizz.ExaminationEndDate = value.Value;
                RaisePropertyChanged(nameof(EndDate));
            }
        }

        public string Course {
            get { return Quizz?.Course.Description; }
        }
    }
}
