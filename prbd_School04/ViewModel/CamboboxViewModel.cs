using School04.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.ViewModel
{
    class CamboboxViewModel : ViewModelCommon
    {

        public Course Course { get; private set; }

        private ObservableCollection<Student> studentRegister;
        public ObservableCollection<Student> StudentRegister
        {
            get => studentRegister;
            set => SetProperty<ObservableCollection<Student>>(ref studentRegister, value);
        }

        public void Init()
        {
            StudentRegister = new ObservableCollection<Student>();
            RaisePropertyChanged();
        }
    }
}
