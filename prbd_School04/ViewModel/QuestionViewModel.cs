using PRBD_Framework;
using School04.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace School04.ViewModel {
    public class QuestionViewModel : ViewModelBase<ModelSchool04> {
        protected override void OnRefreshData() {
        }

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

        private ObservableCollection<CheckCategory> categories;  // toute liste qu'on doit affiché ds la vue doit etre observable et donc on doit pouvoir remplir cette liste avec la methode LoadCategoryChecked()
        public ObservableCollection<CheckCategory> Categories {
            get => categories;
            set {
                categories = value;
                RaisePropertyChanged(nameof(Categories));
            }
        }

        //public ICommand CheckCategory { get; set; }
        public ICommand None { get; set; }
        public ICommand All { get; set; }

        public QuestionViewModel() : base() {
            Questions = new ObservableCollection<Question>(App.Context.Questions);
            LoadCategoryChecked();
           
            //CheckCategory = new RelayCommand<CheckCategory>(checkCategory => {});
            None = new RelayCommand(CheckedNoneCategoryAction);
            All = new RelayCommand(CheckedAllCategoryAction);
        }


        public void Init(Course course) {
            Course = course;
            /*foreach (var question in Course.QuestionList) {
                Console.WriteLine(question.Enonce);
            }*/
            Questions = new ObservableCollection<Question>(Course.QuestionList);

            RaisePropertyChanged();
        }

        private void LoadCategoryChecked() {
            Categories = new ObservableCollection<CheckCategory>();// je cree une liste vide, ensuite je parcoure ma BD, je récupère les noms de chaque catégorie et j'ajoute à ma nouvelle liste
            
            foreach (var category in App.Context.Categories)               
            {
                var p = new CheckCategory() {
                    Name = category.Name,
                    //Checked = Question.Categories.Contains(category)
                };
                Categories.Add(p);                              
            }
        }

        //implémentation du bouton All
        private void CheckedAllCategoryAction() {
            var Categs = new ObservableCollection<CheckCategory>(); //je creer une nouvelle liste de catégorie que je mets à jour avec ma liste de catégorie.

            foreach (var category in Categories) {
                category.Checked = true;
                Categs.Add(category);
            }
            Categories = new ObservableCollection<CheckCategory>(Categs);
        }

        //implémentation du bouton None
        private void CheckedNoneCategoryAction() {
            var Categs = new ObservableCollection<CheckCategory>();   

            foreach (var category in Categories) {
                category.Checked = false;
                Categs.Add(category);
            }
            Categories = new ObservableCollection<CheckCategory>(Categs);
        }


    }
}
