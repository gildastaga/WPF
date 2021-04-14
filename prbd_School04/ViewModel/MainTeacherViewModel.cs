using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    public class MainTeacherViewModel : ViewModelBase<ModelSchool04> {
        public ObservableCollection<Course> Courses { get; set; }
        /*public ObservableCollection<Course> Courses {
            get => courses;
            set => SetProperty<ObservableCollection<Course>>(ref courses, value);
        }*/
        public MainTeacherViewModel() : base() {
            Courses = new ObservableCollection<Course>(Context.Courses);
        }

        protected override void OnRefreshData() {
            // Pour plus tard
        }
    }
}
