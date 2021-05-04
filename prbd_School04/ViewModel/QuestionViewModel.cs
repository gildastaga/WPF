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

        public Question Question { get; set; }
        private bool listCategotyChange = false;

        private ObservableCollection<CheckCategory> categories;  // toute liste qu'on doit affiché ds la vue doit etre observable et donc on doit pouvoir remplir cette liste avec la methode LoadTagsChecked()
        public ObservableCollection<CheckCategory> Categories {
            get => categories;
            set {
                categories = value;
                RaisePropertyChanged(nameof(Categories));
            }
        }

        public ICommand CheckCategory { get; set; }
        public QuestionViewModel(Question question) : base() {
            Question = question;
            LoadCategoryChecked();
            CheckCategory = new RelayCommand<CheckCategory>(checkCategory => {
                listCategotyChange = true;
            });

        }

        private void LoadCategoryChecked() {
            Categories = new ObservableCollection<CheckCategory>();   // Pour le moment ma liste est vide,
            foreach (var category in App.Context.Categories)               
            {
                var p = new CheckCategory()                   
                {
                    Name = category.Name,                     
                    Checked = Question.Categories.Contains(category)
                };
                Categories.Add(p);                              
            }
        }
    }
}
