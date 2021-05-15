using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {

    public class CategoryViewModel : ViewModelCommon {

        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }

        private Category category;
        public Category Category { get => category; set => SetProperty(ref category, value); }

        private bool isNew;
        public bool IsNew {
            get { return isNew; }
            set {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
            }
        }

        public bool IsExisting { get => !isNew; }

        public string Name {
            get { return Category?.Name; }
            set {
                Category.Name = value;
                RaisePropertyChanged(nameof(Name));
               // NotifyColleagues(AppMessages.MSG_NAME_CHANGED, Category);
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
            Cancel = new RelayCommand(CancelAction);
            Delete = new RelayCommand(DeleteAction, () => !IsNew);
        }

        public void Init(Category category, bool isNew) {
            Category = category;
            IsNew = isNew;
            RaisePropertyChanged();
        }

        private void SaveAction() {
            if (IsNew) {   
                Category.Name = Category.Name;
                Context.Add(Category);
                IsNew = false;
            }
            Context.SaveChanges();
           // NotifyColleagues(AppMessages.MSG_CATEGORY_CHANGED, Category);
        }

        private void CancelAction() {
            if (IsNew) {
               // NotifyColleagues(AppMessages.MSG_CLOSE_TAB, Category);
            } else {
                Context.Reload(Category);
                RaisePropertyChanged();
            }
        }

        private void DeleteAction() {
            CancelAction();
            Category.Delete();
            //NotifyColleagues(AppMessages.MSG_CATEGORY_CHANGED, Category);
            //NotifyColleagues(AppMessages.MSG_CLOSE_TAB, Category);
        }

    }
}
