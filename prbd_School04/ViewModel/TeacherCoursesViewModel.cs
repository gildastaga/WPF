using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    public class TeacherCoursesViewModel : ViewModelCommon{
        private ObservableCollection<Course> courses;
        public ObservableCollection<Course> Courses {
            get => courses;
            set => SetProperty<ObservableCollection<Course>>(ref courses, value);
        }
        public void makeList() {
            if ((CurrentUser != null) && (CurrentUser.IsTeacher())) {
                    Teacher teacher = (Teacher) CurrentUser;
                    Courses = new ObservableCollection<Course>(teacher.CourseGiven);
                }
        }
        public TeacherCoursesViewModel() : base() {
            makeList(); 
        }
        protected override void OnRefreshData() {
            // Pour plus tard
        }
    }
}
