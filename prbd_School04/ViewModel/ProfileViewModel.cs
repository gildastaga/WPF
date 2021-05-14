using PRBD_Framework;
using School04.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.ViewModel {
    class ProfileViewModel : ViewModelCommon {
        private User person;

        public User Person {
            get => CurrentUser;
        }

        public ProfileViewModel() {
            person = CurrentUser;
            Name = Person.Name;
            FirstName = Person.FirstName;
            Profile = Person.Profile; 
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
            // Pour plus tard
        }
    }
}
