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
    class ResponseQuizzViewModel : ViewModelCommon {
        public ICommand PreviousQuestion { get; set; }
        public ICommand NextQuestion { get; set; }
        public ICommand ValidateResponse { get; set; }
        public ICommand ChangeResponse { get; set; }
        public ICommand CloseQuizz { get; set; }

        private Quizz quizz;
        public Quizz Quizz { get => quizz; set => SetProperty(ref quizz, value); }

        private Question question;
        public Question Question { get => question; set => SetProperty(ref question, value); }

        private QuestionQuizz questionQuizz;
        public QuestionQuizz QuestionQuizz { get => questionQuizz; set => SetProperty(ref questionQuizz, value); }

        private Answer answer;
        public Answer Answer { get => answer; set => SetProperty(ref answer, value); }

        private bool isNew;
        public bool IsNew {
            get { return isNew; }
            set {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
            }
        }
        public void Init(Quizz quizz, bool isNew) {
            Quizz = isNew ? quizz : Quizz.GetById(quizz.QuizzId);
            IsNew = isNew;
            Question = quizz.getQuestionInPosition(1);
            QuestionQuizz = QuestionQuizz.GetByQuizzQuestion(Quizz, Question);
            Answer = Answer.GetByStudentQuestionquizz((Student)CurrentUser, QuestionQuizz);

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

        private int? total;
        public int? Total {
            get { return Quizz?.QuestionsCount; }
            set => SetProperty(ref total, value);
        }

        private int? currentPos;
        public int? CurrentPos {
            get { return QuestionQuizz?.PosQuestionInQuizz; }
            set => SetProperty(ref currentPos, value);
        }

        public string CurrentAnswers {
            get { if (Answer == null)
                    return null;
                  else {
                    var str = "Current choice : ";
                    foreach (var choosedProp in Answer.ChoosedProposition)
                        str += choosedProp.Body + ", ";
                    return str.Substring(0, str.Length-2);
                  }
            }
        }

        public string NbGoodProps {
            get {
                if (Question != null)
                    return Question.GoodPropositionsCount + " good response(s)";
                return null;
            }
        }

        public ResponseQuizzViewModel() : base() {
            PreviousQuestion = new RelayCommand(PreviousQuestionAction, () => {
                return !Context.ChangeTracker.HasChanges() && CurrentPos > 1
                    && (EndDate == null || EndDate < DateTime.Now);
            });
            NextQuestion = new RelayCommand(NextQuestionAction, () => {
                return !Context.ChangeTracker.HasChanges() && CurrentPos < Total
                    && (EndDate == null || EndDate < DateTime.Now);
            });
            ValidateResponse = new RelayCommand<IList>(ValidateResponseAction,
                selected => { return selected?.Count == Question?.GoodPropositionsCount && Answer == null; });
            ChangeResponse = new RelayCommand<IList>(ChangeResponseAction,
                selected => { return selected?.Count == Question?.GoodPropositionsCount && Answer != null; });
            CloseQuizz = new RelayCommand(CloseAction);
            Register<Course>(this, AppMessages.MSG_COURSE_CHANGED, course => RaisePropertyChanged(nameof(Course)));
        }

        private void PreviousQuestionAction() {
            Question = quizz.getQuestionInPosition(QuestionQuizz.PosQuestionInQuizz - 1);
            QuestionQuizz = QuestionQuizz.GetByQuizzQuestion(Quizz, Question);
            Answer = Answer.GetByStudentQuestionquizz((Student)CurrentUser, QuestionQuizz);
            RaisePropertyChanged();
            //OnRefreshData();
            //NotifyColleagues(AppMessages.MSG_QUIZZ_CHANGED, Quizz);
        }
        private void NextQuestionAction() {
            Question = quizz.getQuestionInPosition(QuestionQuizz.PosQuestionInQuizz + 1);
            QuestionQuizz = QuestionQuizz.GetByQuizzQuestion(Quizz, Question);
            Answer = Answer.GetByStudentQuestionquizz((Student)CurrentUser, QuestionQuizz);
            RaisePropertyChanged();
            //OnRefreshData();
        }

        private void ValidateResponseAction(IList selectedItems) {
            var selectedPropositions = selectedItems.Cast<Proposition>().ToArray();
            var answer = new Answer {Student = (Student)CurrentUser, QuestionQuizz = QuestionQuizz};
            Context.Add(answer);
            Context.SaveChanges();
            foreach (var propositions in selectedPropositions) {
                Console.WriteLine(propositions.Body);
                answer.ChoosedProposition.Add(propositions);
            }
            Context.SaveChanges();
            if (CurrentPos < Total)
                NextQuestionAction();
            else {
                Answer = answer;
                RaisePropertyChanged(nameof(CurrentAnswers));
            }
            //OnRefreshData();
            // notifie le reste de l'application que les messages de ce membre ont été modifiés
            //NotifyColleagues(AppMessages.MSG_REFRESH_REGISTRATIONS, null);
        }

        private void ChangeResponseAction(IList selectedItems) {
            var selectedPropositions = selectedItems.Cast<Proposition>().ToArray();
            var answer = Answer;
            answer.ChoosedProposition.Clear();
            foreach (var propositions in selectedPropositions) {
                Console.WriteLine(propositions.Body);
                answer.ChoosedProposition.Add(propositions);
            }
            Context.SaveChanges();
            RaisePropertyChanged(nameof(CurrentAnswers));
            //OnRefreshData();
            // notifie le reste de l'application que les messages de ce membre ont été modifiés
            //NotifyColleagues(AppMessages.MSG_REFRESH_REGISTRATIONS, null);
        }
        private void CloseAction() {
            NotifyColleagues(AppMessages.MSG_CLOSE_QUIZZ_TAB, Quizz);
        }

        public override void Dispose() {
            base.Dispose();
        }
        protected override void OnRefreshData() {
            if (IsNew || Quizz == null) return;
            Quizz = Quizz.GetById(Quizz.QuizzId);
            Question = Question.GetById(Question.QuestionId);
            QuestionQuizz = QuestionQuizz.GetByQuizzQuestion(Quizz, Question);
            Answer = Answer.GetByStudentQuestionquizz((Student)CurrentUser, QuestionQuizz);

            RaisePropertyChanged();
        }
    }
}
