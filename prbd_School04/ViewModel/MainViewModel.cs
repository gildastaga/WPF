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
        //Action pour le renommage de l'en-tete de l'onglet du course
        public event Action<Course, string> RenameTabCourseDetail;
        //Action pour l'affichage des cours
        public event Action<Course, bool> DisplayCourse;
        //Action pour l'affichage des Quizz
        public event Action<Quizz, bool> DisplayQuizz;
        //action pour fermer une fenetre (userControl) Course
        public event Action<Course> CloseTabCourse;
        public event Action CloseTabProfile;
        //action pour se déconnecter
        public event Action OnLogout;
        //Action pour le renommage de l'en-tete de l'onglet du Quizz
        public event Action<Quizz, string> RenameTabQuizz;
        public event Action<Quizz> CloseTabQuizz;
        //Commande pour se déconnecter
        public ICommand LogoutCommand {
            get; set;
        }
        //commande pour rafraichir
        public ICommand ReloadDataCommand {
            get; set;
        }
        public MainViewModel() : base() {
            //On définit une commande LogoutCommand et on lui associe la méthode LogoutAction dans laquelle 
            //on réalise de manière effective le logout, puis on invoque l'événement OnLogout afin que la vue soit 
            //notifiée et qu'elle puisse procéder à la partie visuelle du traitement du logout, 
            //à savoir la redirection vers la fenêtre de login
            LogoutCommand = new RelayCommand(LogoutAction);
            //On définit une commande ReloadDataCommand à laquelle on associe une fonction lambda 
            //qui envoie un message MSG_REFRESH_DATA à toute l'application afin que tous les modèles de vues 
            //ouverts rafraîchissent leurs données
            ReloadDataCommand = new RelayCommand(() => {
                NotifyColleagues(ApplicationBaseMessages.MSG_REFRESH_DATA);
            });

            Register<Course>(this, AppMessages.MSG_DISPLAY_COURSE, course => {
                Console.WriteLine("Test");
                DisplayCourse?.Invoke(course, false);
            });

            Register<Course>(this, AppMessages.MSG_COURSE_CHANGED, course => {
                RenameTabCourseDetail?.Invoke(course, course.Title);
            });

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

            Register<Course>(this, AppMessages.MSG_CLOSE_TAB_COURSE, course => {
                CloseTabCourse?.Invoke(course);
            });
            
            Register(this, AppMessages.MSG_CLOSE_TAB_PROFILE, () => {
                CloseTabProfile?.Invoke();
            });
        }
        private void LogoutAction() {
            Logout();
            OnLogout?.Invoke();
        }
        //on définit la propriété TitleWindow avec laquelle est bindé le titre de la fenêtre.
        //Cela permet d'y afficher entre parenthèses le pseudo de l'utilisateur actuellement connecté.
        public string TitleWindow {
            get => $"School04 ({CurrentUser.ToString()})";
        }
        protected override void OnRefreshData() {
            //pour plus tard
        }
    }
}
