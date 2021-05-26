﻿using System;
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
        private Quizz quizz;
        public Quizz Quizz { get => quizz; set => SetProperty(ref quizz, value); }

        private Question question;
        public Question Question { get => question; set => SetProperty(ref question, value); }

        private QuestionQuizz questionQuizz;
        public QuestionQuizz QuestionQuizz { get => questionQuizz; set => SetProperty(ref questionQuizz, value); }

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

        public ResponseQuizzViewModel() : base() {
            Register<Course>(this, AppMessages.MSG_COURSE_CHANGED, course => RaisePropertyChanged(nameof(Course)));
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
                return Quizz != null && StartDate != null && EndDate != null && StartDate > DateTime.Now && StartDate < EndDate && (Context?.Entry(Quizz)?.State == EntityState.Modified);
            return Quizz != null && (Context?.Entry(Quizz)?.State == EntityState.Modified);
        }

        private void CancelAction() {
            if (IsNew) {
                NotifyColleagues(AppMessages.MSG_CLOSE_QUIZZ_TAB, Quizz);
            } else {
                Context.Reload(Quizz);
                NotifyColleagues(AppMessages.MSG_TITLE_QUIZZ_CHANGED, quizz);
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

        private bool CanDeleteAction() {
            if (IsNew)
                return false;
            if (StartDate != null && StartDate < DateTime.Now)
                return false;
            return true;
        }

        public override void Dispose() {
            base.Dispose();
        }
        protected override void OnRefreshData() {
            if (IsNew || Quizz == null) return;
            Quizz = Quizz.GetById(Quizz.QuizzId);
            Question = Question.GetById(Question.QuestionId);
            QuestionQuizz = QuestionQuizz.GetById(QuestionQuizz.QuestionQuizzId);
            RaisePropertyChanged();
        }
    }
}
