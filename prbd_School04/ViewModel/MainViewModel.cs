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
        }
    }
}
