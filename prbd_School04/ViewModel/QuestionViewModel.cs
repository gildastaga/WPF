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
            throw new NotImplementedException();
        }
        // ATTRIBUT BINDER AVEC TOUS LES CHECKBOUTTON avec leur attribut checked pour cocher si le bouton est sélectionné

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

        private bool checkAllCategory;
        public bool CheckAllCategory {
            get {
                return checkAllCategory;
            }
            set {
                checkAllCategory = value;
                RaisePropertyChanged(nameof(CheckAllCategory));
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

        public ICommand CheckCategory { get; set; }
        public ICommand None { get; set; }
        public ICommand All { get; set; }
        public QuestionViewModel() : base() {
            Questions = new ObservableCollection<Question>(App.Context.Questions);
            LoadCategoryChecked();
           
            CheckCategory = new RelayCommand<CheckCategory>(checkCategory => {});
            All = new RelayCommand(CheckedAllCategoryAction);
        }

        private void LoadCategoryChecked() {
            Categories = new ObservableCollection<CheckCategory>();   // Pour le moment ma liste est vide,
            
            foreach (var category in App.Context.Categories)               
            {
                Console.WriteLine(category.Name);
                var p = new CheckCategory() {
                    Name = category.Name,
                    //Checked = Question.Categories.Contains(category)
                };
                Categories.Add(p);                              
            }
        }

        private void CheckedAllCategoryAction() {
            var Categs = new ObservableCollection<CheckCategory>();   // Pour le moment ma liste est vide,

            CheckAllCategory = true;
        }


    }
}
