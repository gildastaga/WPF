using PRBD_Framework;
using School04.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using Type = School04.Model.Type;
using Microsoft.EntityFrameworkCore;

namespace School04.ViewModel {
    public class QuestionViewModel : ViewModelBase<ModelSchool04> {

        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
            }
        }

        private bool editMode = false;

        private Course course;
        public Course Course { get => course; set => SetProperty(ref course, value); }

        private ObservableCollection<Question> questions;
        public ObservableCollection<Question> Questions
        {
            get
            {
                return questions;
            }
            set
            {
                questions = value;
                RaisePropertyChanged(nameof(Questions));
            }
        }

        private ObservableCollection<Category> categs;
        public ObservableCollection<Category> Categs
        {
            get => categs;
            set
            {
                categs = value;
                RaisePropertyChanged(nameof(Categs));
            }
        }

        private ObservableCollection<CheckCategory> categories;  
        public ObservableCollection<CheckCategory> Categories {
            get => categories;
            set {
                categories = value;
                RaisePropertyChanged(nameof(Categories));
            }
        }

        private Question question { get; set; }
        public Question Question {
            get => question;
            set
            {
                question = value;
                RaisePropertyChanged(nameof(Question));
            }
        }

        public string Enonce {
            get { return Question?.Enonce; }
            set
            {
                Question.Enonce = value;
                editMode = true;
                RaisePropertyChanged(nameof(Enonce));
            }
        }

        public string answers;
        public string Answers {
            get { return answers; }
            set
            {
                answers = value;
                editMode = true;
                RaisePropertyChanged(nameof(Answers));
            }
        }

        public TypeQuestion typeQuestion;
        public TypeQuestion TypeQuestion {
            get { return typeQuestion; }
            set
            {
                Question.typeQuestion = value;
                editMode = true;
                RaisePropertyChanged(nameof(TypeQuestion));
            }
        }

        private Question questionSelect;
        public Question QuestionSelect
        {
            get { return questionSelect; }
            set
            {
                questionSelect = question = value;
                if (question != null)
                {
                    typeQuestion = question.typeQuestion;
                    LoardAnswers();
                }

                RaisePropertyChanged(nameof(QuestionSelect));
                RaisePropertyChanged();
            }
        }

        public ICommand None { get; set; }
        public ICommand All { get; set; }
        public ICommand NewQuestion { get; set; }
        public ICommand Save { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Cancel { get; set; }

        public ICommand Check { get; set; }
        public ICommand CheckCategory { get; set; }
        

        public QuestionViewModel() : base() {

            Check = new RelayCommand<Category>(
               c => {

                   loardQuestionByCategs();
               }
           );

            None = new RelayCommand(CheckedNoneCategoryAction);
            All = new RelayCommand(CheckedAllCategoryAction);
            Save = new RelayCommand(
                SaveAction,
                () => { return true; }
            );

            Delete = new RelayCommand(
                DeleteAction, () => !IsNew);

            Cancel = new RelayCommand(
                CancelActionQuestion, CanCancelActionNewCourse);

            NewQuestion = new RelayCommand(() => {

                Question = new Question("", Course);
                isNew = true;

                //if(!isNew || Context?.Entry(Question)?.State == EntityState.Modified)
                {
                    this.Enonce = "";
                    this.Answers = "";
                    isNew = false;
                }
                this.Enonce = "";
                this.Answers = "";
            });

            Register(this, AppMessages.MSG_QUESTION_CHANGED, () => {
                Questions = new ObservableCollection<Question>(Course.QuestionList);
            });
        }

        public void Init(Course course)
        {
            Course = course;
            Questions = new ObservableCollection<Question>(Course.QuestionList);
            Categs = new ObservableCollection<Category>(App.Context.Categories);
            RaisePropertyChanged();
        }

        public void LoardAnswers()
        {
            List<string> AnswersQuestion = new List<string>();
            foreach (var answer in question.Propositions)
            {
                if (answer.IsChecked)
                {
                    var bodyRemoveStart = answer.Body.Insert(0, "*");
                    AnswersQuestion.Add(bodyRemoveStart);
                }
                else
                {
                    AnswersQuestion.Add(answer.Body);
                }
            }
            Answers = String.Join(System.Environment.NewLine, AnswersQuestion);
            LoadCategoryChecked();
        }

        private void SaveAction()
        {
            if (isNew) {
                Question.Enonce = Enonce;
                Question.typeQuestion = TypeQuestion;
                Question.Course = Course;

                App.Context.Questions.Add(Question);
                App.Context.SaveChanges();
                isNew = false;
            } else {
                App.Context.Propositions.RemoveRange(Question.Propositions);
            }

            List<string> addAnswers = new List<string>(Answers.Split(Environment.NewLine));
            
            foreach (var answer in addAnswers) {
                Proposition proposition = null;
                if (answer.Contains("*")) {
                    proposition = new Proposition(answer.Replace("*", ""), Type.True, Question);
                } else {
                    proposition = new Proposition(answer, Type.False, Question);
                }

                App.Context.Propositions.Add(proposition);
            }

            Context.SaveChanges();
            NotifyColleagues(AppMessages.MSG_QUESTION_CHANGED);
        }  

        private void CancelActionQuestion()
        {
            if (IsNew && QuestionSelect != null)
            {
                Context.Reload(QuestionSelect);
                RaisePropertyChanged();
            }      
            
        }

        private bool CanCancelActionNewCourse()
        {
            return Question != null && (IsNew || Context?.Entry(Question)?.State == EntityState.Modified);
        }

        private void DeleteAction()
        {
            if (QuestionSelect != null)
            {
                CancelActionQuestion();
                QuestionSelect.Delete();
                Questions.Remove(QuestionSelect);
                answers = "";
                RaisePropertyChanged();
            }
           
        }

        private void LoadCategoryChecked()
        {
            Categories = new ObservableCollection<CheckCategory>();// je cree une liste vide, ensuite je parcoure ma BD, je récupère les noms de chaque catégorie et j'ajoute à ma nouvelle liste
            var lsCateg = new List<Category>(Question.Categories);
            foreach (var category in App.Context.Categories)
            {
                var c = new CheckCategory()
                {
                    Name = category.Name,
                    Checked = lsCateg.Contains(category)
                };
                Categories.Add(c);
            }
        }

        //implémentation du bouton All
        private void CheckedAllCategoryAction()
        {
            var Categss = new ObservableCollection<Category>();

            foreach (var category in Categs)
            {
                category.IsChecked = true;
                Categss.Add(category);
            }
            Categs = new ObservableCollection<Category>(Categss);
            loardQuestionByCategs();
        }

        ////implémentation du bouton None
        private void CheckedNoneCategoryAction()
        {
            var Categss = new ObservableCollection<Category>();

            foreach (var category in Categs)
            {
                category.IsChecked = false;
                Categss.Add(category);
            }

            Categs = new ObservableCollection<Category>(Categss);

            if (QuestionSelect != null)
            {
                ResetInput();
            }

            loardQuestionByCategs();
        }


        private void loardQuestionByCategs()
        {
            var qs = new ObservableCollection<Question>();
            //var myQuestions = (from mq in App.Context.Questions
            //                   where mq.Course.CourseId.Equals(Course.CourseId)
            //                   select mq).ToList();
            var myQuestions = new ObservableCollection<Question>(Course.QuestionList);

            foreach (var c in Categs)
            {
                if (c.IsChecked)
                {
                    foreach (var q in myQuestions)
                    {
                        var lscategs = new List<Category>(q.Categories);
                        if (lscategs.Contains(c))
                        {
                            qs.Add(q);
                        }
                    }
                }
            }
            Questions = new ObservableCollection<Question>(qs);

            if (QuestionSelect != null && !Questions.Contains(QuestionSelect))
            {
                ResetInput();
            }
        }


        private void ResetInput()
        {
            // Enonce = "";
            Answers = "";
            TypeQuestion = TypeQuestion.OneAnswer;
            Categories = new ObservableCollection<CheckCategory>();
        }

        protected override void OnRefreshData()
        {
        }

    }
}
