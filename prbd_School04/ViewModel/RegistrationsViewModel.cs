using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    class RegistrationsViewModel : ViewModelCommon {
        private Course course;
        public Course Course { get => course; set => SetProperty(ref course, value); }

        private string filter;
        public string Filter {
            get => filter;
            set => SetProperty<string>(ref filter, value, OnRefreshData);
        }
        public RegistrationsViewModel() : base() {
            ClearFilter = new RelayCommand(() => Filter = "");

            UnsubscribeOne = new RelayCommand(() => DeleteRegistrationsAction(selectedItemsRegistrations), () => {
                return !Context.ChangeTracker.HasChanges() && selectedItemsRegistrations?.Count > 0;
            });

            UnsubscribeAll = new RelayCommand(DeleteAllRegistrationsAction, () => {
                return !Context.ChangeTracker.HasChanges() && currentRegistrations?.Count > 0;
            });

            SubscribeOne = new RelayCommand(() => AddRegistrationsAction(selectedItemsUnregistered), () => {
                return !Context.ChangeTracker.HasChanges() && selectedItemsUnregistered?.Count > 0
                    && currentRegistrations?.Count + selectedItemsUnregistered?.Count <= course.MaxStudent;
            });

            SubscribeAll = new RelayCommand(AddAllRegistrationsAction, () => {
                return !Context.ChangeTracker.HasChanges() && noRegistrations?.Count > 0
                    && currentRegistrations?.Count + noRegistrations?.Count <= course.MaxStudent;
            });

            ChangeRegistrationState = new RelayCommand<Registration>(ChangeRegistrationStateAction);
        }
        private ObservableCollectionFast<Registration> currentRegistrations = new ObservableCollectionFast<Registration>();
        public ObservableCollectionFast<Registration> CurrentRegistrations {
            get { return currentRegistrations; }
            set {
                currentRegistrations = value;
                RaisePropertyChanged(nameof(CurrentRegistrations), nameof(Registrations));
            }
        }
        public ICollectionView Registrations => CurrentRegistrations.GetCollectionView(nameof(Registration.StudentName), ListSortDirection.Ascending);

        private ObservableCollectionFast<User> noRegistrations = new ObservableCollectionFast<User>();
        public ObservableCollectionFast<User> NoRegistrations {
            get { return noRegistrations; }
            set {
                noRegistrations = value;
                RaisePropertyChanged(nameof(NoRegistrations), nameof(NotRegistered));
            }
        }
        public ICollectionView NotRegistered => NoRegistrations.GetCollectionView(nameof(User.FullName), ListSortDirection.Ascending);
        public ICommand ClearFilter { get; set; }
        public ICommand UnsubscribeAll { get; set; }
        public ICommand UnsubscribeOne { get; set; }
        public ICommand SubscribeOne { get; set; }
        public ICommand SubscribeAll { get; set; }
        public ICommand ChangeRegistrationState { get; set; }

        private IList selectedItemsRegistrations = new ArrayList();
        public IList SelectedItemsRegistrations {
            get => selectedItemsRegistrations;
            set => SetProperty(ref selectedItemsRegistrations, value);
        }

        private IList selectedItemsUnregistered = new ArrayList();
        public IList SelectedItemsUnregistered {
            get => selectedItemsUnregistered;
            set => SetProperty(ref selectedItemsUnregistered, value);
        }
        public void Init(Course course) {
            // Il faut recharger ce membre dans le contexte courant pour pouvoir le modifier
            Course = Course.GetById(course.CourseId);
            CurrentRegistrations = new ObservableCollectionFast<Registration>(Registration.GetCurrentRegistrationsFromCourse(Course));
            NoRegistrations = new ObservableCollectionFast<User>(Registration.GetNoRegistrationsFromCourse(Course));

            RaisePropertyChanged();
        }

        private void DeleteRegistrationsAction(IList registrations) {
            // demande au modèle de supprimer les messages au nom de l'utilisateur courant
            var deleted = Registration.DeleteRegistrations(registrations.Cast<Registration>().ToArray());
            Context.Registrations.RemoveRange(deleted);
            Context.SaveChanges();
            OnRefreshData();
            // notifie le reste de l'application que les messages de ce membre ont été modifiés
            //NotifyColleagues(AppMessages.MSG_REFRESH_REGISTRATIONS, null);
        }

        private void DeleteAllRegistrationsAction() {
            // demande au modèle de supprimer les messages au nom de l'utilisateur courant
            var deleted = Registration.DeleteRegistrations(currentRegistrations.ToArray());
            Context.Registrations.RemoveRange(deleted);
            Context.SaveChanges();
            OnRefreshData();
            // notifie le reste de l'application que les messages de ce membre ont été modifiés
            //NotifyColleagues(AppMessages.MSG_REFRESH_REGISTRATIONS, null);
        }

        private void AddRegistrationsAction(IList unregistered) {
            // demande au modèle de supprimer les messages au nom de l'utilisateur courant
            var added = Registration.AddRegistrations(Course, unregistered.Cast<User>().ToArray());
            Context.Registrations.AddRange(added);
            Context.SaveChanges();
            OnRefreshData();
            // notifie le reste de l'application que les messages de ce membre ont été modifiés
            //NotifyColleagues(AppMessages.MSG_REFRESH_REGISTRATIONS, null);
        }

        private void AddAllRegistrationsAction() {
            // demande au modèle de supprimer les messages au nom de l'utilisateur courant
            var added = Registration.AddRegistrations(Course, NoRegistrations.ToArray());
            Context.Registrations.AddRange(added);
            Context.SaveChanges();
            OnRefreshData();
            // notifie le reste de l'application que les messages de ce membre ont été modifiés
            //NotifyColleagues(AppMessages.MSG_REFRESH_REGISTRATIONS, null);
        }

        private void ChangeRegistrationStateAction(Registration registration) {
            if (registration == null) return;
            switch (registration.RegistrationState.GetHashCode()) {
                case 0:
                    registration.RegistrationState = State.Inactive;
                    break;
                case 1:
                    registration.RegistrationState = State.Active;
                    break;
                case 2:
                    registration.RegistrationState = State.Active;
                    break;
                default:
                    break;
            }
            Context.SaveChanges();
            OnRefreshData();
            // notifie le reste de l'application que les messages de ce membre ont été modifiés
            //NotifyColleagues(AppMessages.MSG_REFRESH_REGISTRATIONS, null);
        }

        protected override void OnRefreshData() {
            if (Course == null) return;
            Course = Course.GetById(Course.CourseId);
            SelectedItemsRegistrations.Clear();
            SelectedItemsUnregistered.Clear();
            CurrentRegistrations.Reset(Registration.GetCurrentRegistrationsFromCourse(Course));
            NoRegistrations.Reset(string.IsNullOrEmpty(Filter) ? Registration.GetNoRegistrationsFromCourse(Course) : Registration.GetFiltredNoRegistrationsFromCourse(Course, Filter));
        }


    }
}
