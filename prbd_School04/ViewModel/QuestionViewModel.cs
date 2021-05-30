using PRBD_Framework;
using School04.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using Type = School04.Model.Type;

namespace School04.ViewModel {
    public class QuestionViewModel : ViewModelBase<ModelSchool04> {
        public event Action<Question> CloseTab;

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
        public ObservableCollection<Question> Questions {
            get {
                return questions;
            }
            set {
                questions = value;
                RaisePropertyChanged(nameof(Questions));
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
        public Question QuestionSelect {
            get { return questionSelect; }
            set
            {
                questionSelect = question = value;
                typeQuestion = question.typeQuestion;
                LoardAnswers();
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

        public QuestionViewModel() : base() {

            //Questions = new ObservableCollection<Question>(App.Context.Questions);
            //LoadCategoryChecked();

            //CheckCategory = new RelayCommand<CheckCategory>(checkCategory => {});
            //None = new RelayCommand(CheckedNoneCategoryAction);
            //All = new RelayCommand(CheckedAllCategoryAction);

            Save = new RelayCommand(
                SaveAction,
                () => { return true; }
            );

            Delete = new RelayCommand(
                DeleteAction, () => !IsNew);

            Cancel = new RelayCommand(
                CancelAction);

            NewQuestion = new RelayCommand(() => {
                Question = new Question("", Course);
                isNew = true;
            });

            Register(this, AppMessages.MSG_QUESTION_CHANGED, () => {
                Questions = new ObservableCollection<Question>(Course.QuestionList);
            });

            Register(this, AppMessages.MSG_CLOSE_QUESTION_TAB, () => {
                CloseTab?.Invoke(question);
            });
        }

        public void Init(Course course) {
            Course = course;
            Questions = new ObservableCollection<Question>(Course.QuestionList);
            //LoadCategoryChecked();
            RaisePropertyChanged();
        }

        public void LoardAnswers()
        {
            List<string> AnswersQuestion = new List<string>();
            foreach (var answer in question.Propositions) {
                if (answer.IsChecked) {
                    var bodyRemoveStart = answer.Body.Insert(0, "*");
                    AnswersQuestion.Add(bodyRemoveStart);
                } else {
                    AnswersQuestion.Add(answer.Body);
                }
            }
            Answers = String.Join(System.Environment.NewLine, AnswersQuestion);
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

        private void CancelAction()
        {
            if (IsNew)
            {
                NotifyColleagues(AppMessages.MSG_CLOSE_QUESTION_TAB, Question);
            }
            else
            {
                Context.Reload(Question);
                RaisePropertyChanged();
            }
        }

        private void DeleteAction()
        {
            CancelAction();
            Question.Delete();
            NotifyColleagues(AppMessages.MSG_QUESTION_CHANGED, Question);
            NotifyColleagues(AppMessages.MSG_CLOSE_QUESTION_TAB, Question);
        }

        //private void LoadCategoryChecked() {
        //    Categories = new ObservableCollection<CheckCategory>();// je cree une liste vide, ensuite je parcoure ma BD, je récupère les noms de chaque catégorie et j'ajoute à ma nouvelle liste

        //    foreach (var category in App.Context.Categories)               
        //    {
        //        var p = new CheckCategory() {
        //            Name = category.Name,
        //            //Checked = Question.Categories.Contains(category)
        //        };
        //        Categories.Add(p);                              
        //    }
        //}

        ////implémentation du bouton All
        //private void CheckedAllCategoryAction() {
        //    var Categs = new ObservableCollection<CheckCategory>(); //je creer une nouvelle liste de catégorie que je mets à jour avec ma liste de catégorie.

        //    foreach (var category in Categories) {
        //        category.Checked = true;
        //        Categs.Add(category);
        //    }
        //    Categories = new ObservableCollection<CheckCategory>(Categs);
        //}

        ////implémentation du bouton None
        //private void CheckedNoneCategoryAction() {
        //    var Categs = new ObservableCollection<CheckCategory>();   

        //    foreach (var category in Categories) {
        //        category.Checked = false;
        //        Categs.Add(category);
        //    }
        //    Categories = new ObservableCollection<CheckCategory>(Categs);
        //}

        protected override void OnRefreshData()
        {
        }
    }
}
