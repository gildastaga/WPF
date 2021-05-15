using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    class QuizzViewModel : ViewModelCommon {
        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }

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
                RaisePropertyChanged(nameof(Questions), nameof(QuestionsView));
            }
        }
        public ICollectionView QuestionsView => Questions.GetCollectionView(nameof(QuestionQuizz.PosQuestionInQuizz), ListSortDirection.Ascending);


        public void Init(Quizz quizz, bool isNew) {
            Quizz = isNew ? quizz : Quizz.GetById(quizz.QuizzId);
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
                NotifyColleagues(AppMessages.MSG_TITLE_QUIZZ_CHANGED, quizz);
            }
        }

        public DateTime? StartDate {
            get { return Quizz?.ExaminationStartDate; }
            set {
                Quizz.ExaminationStartDate = null;
                if (value != null)
                    Quizz.ExaminationStartDate = value.Value;
                RaisePropertyChanged(nameof(StartDate));
            }
        }

        public DateTime? EndDate {
            get { return Quizz?.ExaminationEndDate; }
            set {
                Quizz.ExaminationEndDate = null;
                if (value != null)
                    Quizz.ExaminationEndDate = value.Value;
                RaisePropertyChanged(nameof(EndDate));
            }
        }

        public string Course {
            get { return Quizz?.Course.Description; }
        }

        public QuizzViewModel() : base() {
            Save = new RelayCommand(SaveAction, CanSaveAction);
            Cancel = new RelayCommand(CancelAction, CanCancelAction);
            Delete = new RelayCommand(DeleteAction, () => !IsNew);
        }

        private void SaveAction() {
            if (IsNew) {
                // Un petit raccourci ;-)
                Quizz.Title = Title;
                Context.Add(Quizz);
                IsNew = false;
            }
            Context.SaveChanges();
            OnRefreshData();
            NotifyColleagues(AppMessages.MSG_QUIZZ_CHANGED, Quizz);
        }

        private bool CanSaveAction() {
            if (IsNew)
                return !string.IsNullOrEmpty(Title);
            if(StartDate != null || EndDate != null)
                return Quizz != null && StartDate != null && EndDate != null && StartDate < EndDate && (Context?.Entry(Quizz)?.State == EntityState.Modified);
            return Quizz != null && (Context?.Entry(Quizz)?.State == EntityState.Modified);
        }

        private void CancelAction() {
            if (IsNew) {
                NotifyColleagues(AppMessages.MSG_CLOSE_QUIZZ_TAB, Quizz);
            } else {
                Context.Reload(Quizz);
                RaisePropertyChanged();
            }
        }

        private bool CanCancelAction() {
            return Quizz != null && (IsNew || Context?.Entry(Quizz)?.State == EntityState.Modified);
        }

        private void DeleteAction() {
            CancelAction();
            Quizz.Delete();
            NotifyColleagues(AppMessages.MSG_QUIZZ_CHANGED, Quizz);
            NotifyColleagues(AppMessages.MSG_CLOSE_QUIZZ_TAB, Quizz);
        }

        public override void Dispose() {
            base.Dispose();
        }
        protected override void OnRefreshData() {
            if (IsNew || Quizz == null) return;
            Quizz = Quizz.GetById(Quizz.QuizzId);
            RaisePropertyChanged();
        }
    }
}
