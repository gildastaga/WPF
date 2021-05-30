using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {

    public class CategoryViewModel : ViewModelCommon {

        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }

        //public event Action OnCategorySuccess;
        //public event Action SaveClick;
        //public event Action delete_Click;

        private Category category;
        public Category Category { get => category; set => SetProperty(ref category, value); }

        private QuestionCateg questionCateg;
        public QuestionCateg QuestionCateg { get => questionCateg; set => SetProperty(ref questionCateg, value); }


        private ObservableCollection<Category> categories;
        public ObservableCollection<Category> Categories
        {
            get {
                return categories;
            }
            set {
                categories = value;
                RaisePropertyChanged(nameof(Categories));
            }
        }

        private bool isNew;
        public bool IsNew {
            get { return isNew; }
            set {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
            }
        }

        //public bool IsExisting { get => !isNew; }

        public string Name {
            get { return Category?.Name; }
            set {
                Category.Name = value;
                RaisePropertyChanged(nameof(Name));
                //NotifyColleagues(AppMessages.MSG_NAME_CHANGED, Category);
            }
        }

        public Question Question {
            get { return Category?.Question; }
            set {
                Category.Question = value;
                RaisePropertyChanged(nameof(Question));
            }
        }

        public CategoryViewModel() : base() {
            Save = new RelayCommand(SaveAction);
            Cancel = new RelayCommand(CancelAction, CanCancelActionNewCourse);
            Delete = new RelayCommand(DeleteAction, () => !IsNew);

           /* Register<Category>(this, AppMessages.MSG_CATEGORY_CHANGED, category => {
                Categories = new ObservableCollection<Category>(App.Context.Categories);
            });*/
        }

        public void Init() {
            Categories = new ObservableCollection<Category>(App.Context.Categories);
            foreach(var c in Categories)
            {
                Console.WriteLine("categories: " + c.Name);
            }
            Category = category;
           // IsNew = isNew;
            RaisePropertyChanged();
        }

        private void SaveAction()
        {
            if (isNew &&  Category.Name != null)
            {
                QuestionCateg.Category = QuestionCateg.Category;
                QuestionCateg.Question = QuestionCateg.Question;

                App.Context.Categories.Add(Category);
                App.Context.SaveChanges();
                isNew = false;
            }

            Context.SaveChanges();
            //NotifyColleagues(AppMessages.MSG_QUESTION_CHANGED);
        }

        private void CancelAction()
        {
            if (IsNew && Category != null)
            {
                Context.Reload(Category);
                RaisePropertyChanged();
            }

        }

        private bool CanCancelActionNewCourse()
        {
            return Category != null && (IsNew || Context?.Entry(Category)?.State == EntityState.Modified);
        }


        private void DeleteAction()
        {
            if (Category != null)
            {
                CancelAction();
                Category.Delete();
                RaisePropertyChanged();
            }

        }
        protected override void OnRefreshData() {
        }

 




    }
}
