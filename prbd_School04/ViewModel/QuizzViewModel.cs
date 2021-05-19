using System;
using System.Collections;
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
        public ICommand AddQuestion { get; set; }
        public ICommand RemoveQuestion { get; set; }
        public ICommand ChangeWeight { get; set; }

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

        private ObservableCollectionFast<Question> availableQuestions = new ObservableCollectionFast<Question>();
        public ObservableCollectionFast<Question> AvailableQuestions {
            get { return availableQuestions; }
            set {
                availableQuestions = value;
                RaisePropertyChanged(nameof(AvailableQuestions), nameof(QuestionsQuizz));
            }
        }
        public ICollectionView QuestionsBank => AvailableQuestions.GetCollectionView(nameof(Question.Enonce), ListSortDirection.Ascending);

        private ObservableCollectionFast<QuestionQuizz> currentQuestions = new ObservableCollectionFast<QuestionQuizz>();
        public ObservableCollectionFast<QuestionQuizz> CurrentQuestions {
            get { return currentQuestions; }
            set {
                currentQuestions = value;
                RaisePropertyChanged(nameof(CurrentQuestions), nameof(QuestionsQuizz));
            }
        }
        public ICollectionView QuestionsQuizz => CurrentQuestions.GetCollectionView(nameof(QuestionQuizz.PosQuestionInQuizz), ListSortDirection.Ascending);

        public void Init(Quizz quizz, bool isNew) {
            Quizz = isNew ? quizz : Quizz.GetById(quizz.QuizzId);
            IsNew = isNew;
            CurrentQuestions = new ObservableCollectionFast<QuestionQuizz>(QuestionQuizz.GetQuestionsFromQuizz(Quizz));
            AvailableQuestions = new ObservableCollectionFast<Question>(Question.GetAvailableQuestionsForQuizz(Quizz));

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
            get { return Quizz?.Course?.Description; }
        }

        private int weight;
        public int Weight {
            get => weight;
            set => SetProperty(ref weight, value);
        }

        private QuestionQuizz selectedQuestionQuizz;
        public QuestionQuizz SelectedQuestionQuizz {
            get => selectedQuestionQuizz;
            set {
                SetProperty(ref selectedQuestionQuizz, value);
                if(value != null)
                    Weight = value.NbPoint;
            }
        }

        private Question selectedQuestion;
        public Question SelectedQuestion {
            get => selectedQuestion;
            set {
                SetProperty(ref selectedQuestion, value);
                Weight = 0;
            }
        }

        public QuizzViewModel() : base() {
            Save = new RelayCommand(SaveAction, CanSaveAction);
            Cancel = new RelayCommand(CancelAction, CanCancelAction);
            Delete = new RelayCommand(DeleteAction, () => !IsNew);
            ChangeWeight = new RelayCommand(ChangeWeightAction, () => {
                return !Context.ChangeTracker.HasChanges() && selectedQuestionQuizz != null
                    && Weight > 0;
            });
            AddQuestion = new RelayCommand(AddQuestionAction, () => {
                return !Context.ChangeTracker.HasChanges() && selectedQuestion != null
                    && Weight > 0 && !IsNew;
            });
            RemoveQuestion = new RelayCommand(RemoveQuestionAction, () => {
                return !Context.ChangeTracker.HasChanges() && selectedQuestionQuizz != null && !IsNew;
            });
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
        private void ChangeWeightAction() {
            SelectedQuestionQuizz.NbPoint = Weight;
            Context.SaveChanges();
            RaisePropertyChanged(SelectedQuestionQuizz, nameof(QuestionQuizz.NbPoint));
            //OnRefreshData();
            //NotifyColleagues(AppMessages.MSG_QUIZZ_CHANGED, Quizz);
        }
        private void AddQuestionAction() {
            var qq = new QuestionQuizz {
                Quizz = quizz,
                Question = SelectedQuestion,
                NbPoint = Weight,
                PosQuestionInQuizz = quizz.QuestionsCount + 1
            };
            Context.Add(qq);
            Context.SaveChanges();
            //RaisePropertyChanged(SelectedQuestionQuizz, nameof(QuestionQuizz.NbPoint));
            NotifyColleagues(AppMessages.MSG_QUIZZ_CHANGED, Quizz);
            OnRefreshData();
            //NotifyColleagues(AppMessages.MSG_QUIZZ_CHANGED, Quizz);
        }
        private void RemoveQuestionAction() {
            int pos = SelectedQuestionQuizz.PosQuestionInQuizz;
            var NextQuestions = QuestionQuizz.GetQuestionsFromQuizzAfterPos(quizz, pos);
            SelectedQuestionQuizz.Delete();
            foreach (var q in NextQuestions) {
                q.PosQuestionInQuizz--;
            }
            Context.SaveChanges();
            //RaisePropertyChanged(SelectedQuestionQuizz, nameof(QuestionQuizz.NbPoint));
            NotifyColleagues(AppMessages.MSG_QUIZZ_CHANGED, Quizz);
            OnRefreshData();
            //NotifyColleagues(AppMessages.MSG_QUIZZ_CHANGED, Quizz);
        }

        public override void Dispose() {
            base.Dispose();
        }
        protected override void OnRefreshData() {
            if (IsNew || Quizz == null) return;
            Quizz = Quizz.GetById(Quizz.QuizzId);
            CurrentQuestions.Reset(QuestionQuizz.GetQuestionsFromQuizz(Quizz));
            AvailableQuestions.Reset(Question.GetAvailableQuestionsForQuizz(Quizz));
            Weight = 0;
            RaisePropertyChanged();
        }
    }
}
