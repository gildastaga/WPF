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
    class ProfileViewModel : ViewModelCommon {
        private User person;

        public User Person {
            get => CurrentUser;
            set => SetProperty(ref person, value);
        }

        public ProfileViewModel() : base(){
            makeList();
            person = CurrentUser;
            Name = Person.Name;
            FirstName = Person.FirstName;
            Profile = Person.Profile;

            SaveUser = new RelayCommand(SaveActionUser, CanSaveActionUser);
            CancelUser = new RelayCommand(CancelActionUser);
        }
        public ICommand CancelUser {
            get; set;
        }
        public ICommand SaveUser {
            get; set;
        }
        private void SaveActionUser() {
            if (Validate()) {
                
                Context.SaveChanges();
            }
                
            OnRefreshData();
            NotifyColleagues(AppMessages.MSG_UPDATE_PROFILE);
            NotifyColleagues(AppMessages.MSG_CLOSE_TAB_PROFILE);
        }
        // determine si le bouton peut etre actif ou pas
        private bool CanSaveActionUser() {
            return person != null && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(FirstName) && (Context?.Entry(Person)?.State == EntityState.Modified);
        }
        private void CancelActionUser() {
            NotifyColleagues(AppMessages.MSG_CLOSE_TAB_PROFILE);
        }
        public void makeList() {
        }
        public string Name {
            get {
                return Person?.Name;
            }
            set {
                Person.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
        public string FirstName {
            get {
                return Person?.FirstName;
            }
            set {
                Person.FirstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }
        public string Profile {
            get {
                return Person?.Profile;
            }
            set {
                Person.Profile = value;
                RaisePropertyChanged(nameof(Profile));
            }
        }
        protected override void OnRefreshData() {
            if (Person == null)
                return;
            Person = User.GetByUserId(Person.UserId); 
            RaisePropertyChanged();
        }
    }
}
