using Microsoft.EntityFrameworkCore;
using PRBD_Framework;
using School04.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace School04.ViewModel {
    class NewQuestionViewModel : ViewModelCommon {

        private Question question;
        public Question Question { get => question; set => SetProperty(ref question, value); }

        private bool isNew;
        public bool IsNew {
            get { return isNew; }
            set {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
            }
        }


        public string Enonce {
            get { return Question?.Enonce; }
            set {
                Question.Enonce = value;
                RaisePropertyChanged(nameof(Enonce));
                NotifyColleagues(AppMessages.MSG_ENONCE_QUESTION_CHANGED, question);
            }
        }

        public ICommand Save { get; set; }     //TODO
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }

        public void Init(Question question, bool isNew) {
            Question = isNew ? question : Question.GetById(question.QuestionId);
            IsNew = isNew;
            RaisePropertyChanged();
        }

        public NewQuestionViewModel() : base() {
            Save = new RelayCommand(SaveAction, CanSaveAction);
            Cancel = new RelayCommand(CancelAction, CanCancelAction);
            Delete = new RelayCommand(DeleteAction, CanDeleteAction);
        }


        private void SaveAction() {
            if (IsNew) {
                Question.Enonce = Enonce;
                Context.Add(Question);
                IsNew = false;
            }
            Context.SaveChanges();
            OnRefreshData();
            NotifyColleagues(AppMessages.MSG_QUESTION_CHANGED, Question);
        }

        private bool CanSaveAction() {
            return !string.IsNullOrEmpty(Enonce);

        }

        private void CancelAction() {
            if (IsNew) {
                NotifyColleagues(AppMessages.MSG_CLOSE_QUESTION_TAB, Question);
            } else {
                Context.Reload(Question);
                NotifyColleagues(AppMessages.MSG_ENONCE_QUESTION_CHANGED, question);
                RaisePropertyChanged();
            }
        }

        private bool CanCancelAction() {
            return Question != null && (IsNew || Context?.Entry(Question)?.State == EntityState.Modified);
        }

        private void DeleteAction() {
            CancelAction();
            Question.Delete();
            NotifyColleagues(AppMessages.MSG_QUESTION_CHANGED, Question);
            NotifyColleagues(AppMessages.MSG_CLOSE_QUESTION_TAB, Question);
        }

        private bool CanDeleteAction() {
            if (IsNew)
                return false;

            return true;
        }
    }
}
