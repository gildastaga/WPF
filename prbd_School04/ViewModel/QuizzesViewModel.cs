using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    public class QuizzesViewModel : ViewModelCommon {
        private Course course;
        public Course Course { get => course; set => SetProperty(ref course, value); }
        public QuizzesViewModel() : base() {
            
        }
        private ObservableCollectionFast<Quizz> quizzes = new ObservableCollectionFast<Quizz>();
        public ObservableCollectionFast<Quizz> Quizzes {
            get { return quizzes; }
            set {
                quizzes = value;
                RaisePropertyChanged(nameof(Quizzes), nameof(QuizzesView));
            }
        }
        public ICollectionView QuizzesView => Quizzes.GetCollectionView(nameof(DateTime), ListSortDirection.Descending);

        public void Init(Course course) {
            // Il faut recharger ce membre dans le contexte courant pour pouvoir le modifier
            Course = Course.GetById(course.CourseId);
            Quizzes = new ObservableCollectionFast<Quizz>(Quizz.GetQuizzesFromCourse(Course));

            RaisePropertyChanged();
        }
    }
}
