using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    public class MainViewModel : ViewModelCommon {
        //Action pour le renommage de l'en-tete de l'onglet
        public event Action<Course, string> RenameTabCourseDetail;
        public event Action<Course, bool> DisplayCourse;
        public event Action<Quizz, bool> DisplayQuizz;
        public event Action<Course> CloseTab;
        public event Action OnLogout;
        public ICommand LogoutCommand {
            get; set;
        }
        public ICommand ReloadDataCommand {
            get; set;
        }
        public event Action<Quizz, string> RenameTabQuizz;
        public event Action<Quizz> CloseTabQuizz;
        public MainViewModel() : base() {
            LogoutCommand = new RelayCommand(LogoutAction);

            Register<Course>(this, AppMessages.MSG_DISPLAY_COURSE, course => {
                Console.WriteLine("Test");
                DisplayCourse?.Invoke(course, false);
            });

            Register<Course>(this, AppMessages.MSG_COURSE_CHANGED, course => {
                RenameTabCourseDetail?.Invoke(course, course.Title);
            });

            /*Register<Course>(this, AppMessages.MSG_SAVE_COURSE, course => {
                School04.Model.Course.AddElem(course);

            });

            Register<Course>(this, AppMessages.MSG_CANCEL_COURSE, course => {
                DisplayCourse?.Invoke(course, false);
            });

            Register<Course>(this, AppMessages.MSG_DELETE_COURSE, course => {
                Course.RemoveElem(course);
                DisplayCourse?.Invoke(course, false);
            });*/

            Register<Quizz>(this, AppMessages.MSG_DISPLAY_QUIZZ, quizz => {
                Console.WriteLine("Test");
                DisplayQuizz?.Invoke(quizz, false);
            });

            Register<Quizz>(this, AppMessages.MSG_NEW_QUIZZ, quizz => {
                Console.WriteLine("Test");
                DisplayQuizz?.Invoke(quizz, true);
            });

            Register<Quizz>(this, AppMessages.MSG_TITLE_QUIZZ_CHANGED, quizz => {
                RenameTabQuizz?.Invoke(quizz, quizz.Title);
            });

            Register<Quizz>(this, AppMessages.MSG_CLOSE_QUIZZ_TAB, quizz => {
                CloseTabQuizz?.Invoke(quizz);
            });

            Register(this, AppMessages.MSG_NEW_COURSE, () => {
                ///créer une nouvelle instance pour un nouveau course "vide"
                //si le user connecté est un teacher 
                //sinon ne rien afficher
                if (IsTeacher) {
                    var course = new Course(null,"","",null,(Teacher)CurrentUser);
                    //demande à la vue de créer dynamiquement un nouvel onglet avec le titre "new course"
                    DisplayCourse?.Invoke(course, true);
                }
            });

            Register<Course>(this, AppMessages.MSG_CLOSE_TAB, course => {
                CloseTab?.Invoke(course);
            });
        }
        private void LogoutAction() {
            Logout();
            OnLogout?.Invoke();
        }
        public string TitleWindow {
            get => $"School04 ({CurrentUser.ToString()})";
        }

        protected override void OnRefreshData() {
            //pour plus tard
        }
    }
}
