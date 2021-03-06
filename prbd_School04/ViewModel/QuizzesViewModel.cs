using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    public class QuizzesViewModel : ViewModelCommon {
        private Course course;
        public Course Course { get => course; set => SetProperty(ref course, value); }
        public QuizzesViewModel() : base() {
            DisplayQuizz = new RelayCommand<Quizz>(quizz => {
                //Console.WriteLine(quizz.Title);
                NotifyColleagues(AppMessages.MSG_DISPLAY_QUIZZ, quizz);
            });

            CreateQuizz = new RelayCommand(() => { NotifyColleagues(AppMessages.MSG_NEW_QUIZZ, new Quizz("", null, null, null, Course)); });

            Register<Quizz>(this, AppMessages.MSG_QUIZZ_CHANGED, quizz => {
                OnRefreshData();
            });
        }
        private ObservableCollectionFast<Quizz> quizzes = new ObservableCollectionFast<Quizz>();
        public ObservableCollectionFast<Quizz> Quizzes {
            get { return quizzes; }
            set {
                quizzes = value;
                RaisePropertyChanged(nameof(Quizzes), nameof(QuizzesView));
            }
        }
        public ICollectionView QuizzesView => Quizzes.GetCollectionView(nameof(Quizz.Title), ListSortDirection.Ascending);

        private IList selectedItems = new ArrayList();
        public IList SelectedItems {
            get => selectedItems;
            set => SetProperty(ref selectedItems, value);
        }

        public void Init(Course course) {
            // Il faut recharger ce membre dans le contexte courant pour pouvoir le modifier
            Course = Course.GetById(course.CourseId);
            Quizzes = new ObservableCollectionFast<Quizz>(Quizz.GetQuizzesFromCourse(Course));

            RaisePropertyChanged();
        }

        public ICommand DisplayQuizz { get; set; }

        public ICommand CreateQuizz { get; set; }

        protected override void OnRefreshData() { 
            Quizzes.Reset(Quizz.GetQuizzesFromCourse(Course));
        }
    }
}
