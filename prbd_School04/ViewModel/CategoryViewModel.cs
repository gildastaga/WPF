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


        public string Name {
            get { return Category?.Name; }
            set {
                Category.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public Question Question {
            get { return Category?.Question; }
            set {
                Category.Question = value;
                RaisePropertyChanged(nameof(Question));
            }
        }

        private Category categorySelected;
        public Category CategorySelected
        {
            get { return categorySelected; }
            set
            {
                categorySelected = value;
                
                if(categorySelected != null && categorySelected.Name != null)
                {
                    
                    IsNew = false;
                    Console.WriteLine("Categ...: " + categorySelected.Name);
                } else
                {
                    IsNew = true;
                }
                RaisePropertyChanged(nameof(CategorySelected));
                RaisePropertyChanged();
            }
        }

        public CategoryViewModel() : base() {
            Save = new RelayCommand(SaveAction);
            Cancel = new RelayCommand(CancelAction, CanCancelAction);
            Delete = new RelayCommand(DeleteAction, () => !IsNew);

           /* Register<Category>(this, AppMessages.MSG_CATEGORY_CHANGED, category => {
                Categories = new ObservableCollection<Category>(App.Context.Categories);
            });*/
        }

        public void Init() {
            Categories = new ObservableCollection<Category>(App.Context.Categories);
            Category = category;
            IsNew = false; ;
            RaisePropertyChanged();
        }

        private void SaveAction()
        {
            Console.WriteLine(IsNew);
            if (IsNew)
            {
                var categ = Categories.LastOrDefault();
                App.Context.Categories.Add(categ);
                App.Context.SaveChanges();
                isNew = false;
            }

            Context.SaveChanges();
        }

        private void CancelAction()
        {
            if (IsNew && Category != null)
            {
                Context.Reload(Category);
                RaisePropertyChanged();
            }

        }

        private bool CanCancelAction()
        {
            return Category != null && (IsNew || Context?.Entry(Category)?.State == EntityState.Modified);
        }


        private void DeleteAction()
        {
            if (!IsNew)
            {
                var categ = Categories.LastOrDefault();
                App.Context.Categories.Remove(categ);
                Categories.Remove(categ);
                App.Context.SaveChanges();
                isNew = false;
            }

            Context.SaveChanges();

        }
        
        protected override void OnRefreshData() {
        }

 




    }
}
