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
        public event Action<Course, bool> DisplayCourse;
        public event Action<Quizz, bool> DisplayQuizz;
        public event Action OnLogout;
        public ICommand LogoutCommand {
            get; set;
        }
        public ICommand ReloadDataCommand {
            get; set;
        }
        public MainViewModel() : base() {
            LogoutCommand = new RelayCommand(LogoutAction);
            ReloadDataCommand = new RelayCommand(() => {
                NotifyColleagues(ApplicationBaseMessages.MSG_REFRESH_DATA);
            });
            Register<Course>(this, AppMessages.MSG_DISPLAY_COURSE, course => {
                Console.WriteLine("Test");
                DisplayCourse?.Invoke(course, false);
            });

            Register<Quizz>(this, AppMessages.MSG_DISPLAY_QUIZZ, quizz => {
                Console.WriteLine("Test");
                DisplayQuizz?.Invoke(quizz, false);
            });

            Register<Quizz>(this, AppMessages.MSG_NEW_QUIZZ, quizz => {
                Console.WriteLine("Test");
                DisplayQuizz?.Invoke(quizz, true);
            });

            Register(this, AppMessages.MSG_NEW_COURSE, () => {
                ///créer une nouvelle instance pour un nouveau cours "vide"
                //si le user connecté est un teacher 
                //sinon ne rien afficher
                if (IsTeacher) {
                    var course = new Course(null,"","",null,(Teacher)CurrentUser);
                //demande à la vue de créer dynamiquement un nouvel onglet avec le titre "new course"
                DisplayCourse?.Invoke(course, true);
                }
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
