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
        private string filter;
        public string Filter {
            get => filter;
            set => SetProperty(ref filter, value, ApplyFilterAction);
        }
        public ICommand ClearFilter {
            get; set;
        }
        public ICommand NewCourse {
            get; set; 
        }
        public ICommand DisplayCourseDetails {
            get; set;
        }
        public void makeList() {
            //Affiche la bonne liste des cours : soit le user connecté est un prof 
            // et donc affiche les cours que ce prof donne
            //soit le user connecté est un etudiant et affiche tout les cours
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
            
            ClearFilter = new RelayCommand(() => Filter = "");

            NewCourse = new RelayCommand(() => { NotifyColleagues(AppMessages.MSG_NEW_COURSE); });
            
            Register<Course>(this, AppMessages.MSG_COURSE_CHANGED, course => { ApplyFilterAction(); });

            DisplayCourseDetails = new RelayCommand<Course>(course => {
                 NotifyColleagues(AppMessages.MSG_DISPLAY_COURSE, course);
            });
        }
        private void ApplyFilterAction() {
            Console.WriteLine("Search clicked! " + Filter);
            IEnumerable<Course> query = Context.Courses;
            if(!string.IsNullOrEmpty(Filter))
                query = from m in Context.Courses
                        where m.Title.Contains(Filter)
                        select m;
            Courses = new ObservableCollection<Course>(query);
            Console.WriteLine($"{query.Count()} courses found");
        } 
        protected override void OnRefreshData() {
            // Pour plus tard
        }
    }
}
