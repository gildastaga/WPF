using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    class RegistrationsViewModel : ViewModelCommon {
        private Course course;
        public Course Course { get => course; set => SetProperty(ref course, value); }
        public RegistrationsViewModel() : base() {
            
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

        public void Init(Course course) {
            // Il faut recharger ce membre dans le contexte courant pour pouvoir le modifier
            Course = Course.GetById(course.CourseId);
            CurrentRegistrations = new ObservableCollectionFast<Registration>(Registration.GetCurrentRegistrationsFromCourse(Course));
            NoRegistrations = new ObservableCollectionFast<User>(Registration.GetNoRegistrationsFromCourse(Course));

            RaisePropertyChanged();
        }

        
    }
}
