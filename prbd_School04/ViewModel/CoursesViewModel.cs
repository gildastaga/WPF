using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    class CoursesViewModel : ViewModelCommon {

        private ObservableCollection<Course> courses;
        public ObservableCollection<Course> Courses {
            get => courses;
            set => SetProperty<ObservableCollection<Course>>(ref courses, value);
        }
        public ICommand DisplayCourseDetails {
            get; set;
        }

        public void makeList() {
            if ((CurrentUser != null) && (CurrentUser.IsTeacher())) {
                Teacher teacher = (Teacher)CurrentUser;
                Courses = new ObservableCollection<Course>(teacher.CourseGiven);
            } else if ((CurrentUser != null) && (!CurrentUser.IsTeacher())) {
                Student student = (Student)CurrentUser;
                Courses = new ObservableCollection<Course>(App.Context.Courses);
            }
        }
        public CoursesViewModel() : base() {
            makeList();

            DisplayCourseDetails = new RelayCommand<Course>(course => {
                Console.WriteLine(course.Title); 
                NotifyColleagues(AppMessages.MSG_DISPLAY_COURSE, course);
            });
        }
        protected override void OnRefreshData() {
            // Pour plus tard
        }
    }
}
