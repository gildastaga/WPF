using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School04.Model;

namespace School04.ViewModel {
    public class MainViewModel : ViewModelCommon {
        public event Action<Course, bool> DisplayCourse;
        public MainViewModel() : base() {
            Register<Course>(this, AppMessages.MSG_DISPLAY_COURSE, course => {
                Console.WriteLine("Test");
                DisplayCourse?.Invoke(course, false);
            });

            Register(this, AppMessages.MSG_NEW_COURSE, () => {
                //créer une nouvelle instance pour un nouveau cours "vide"
                //var course = new Course(null,"","",null,(Teacher)CurrentUser);
                //demande à la vue de créer dynamiquement un nouvel onglet avec le titre "new course"
                //DisplayCourse?.Invoke(course, true);
            });
        }

        protected override void OnRefreshData() {
            //pour plus tard
        }
    }
}
