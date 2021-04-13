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
        public ObservableCollection<Student> Students {
            get; set;
        }
        public MainTeacherViewModel() : base() {
            Students = new ObservableCollection<Student>(Context.Students);
        }

        protected override void OnRefreshData() {
            // Pour plus tard
        }
    }
}
