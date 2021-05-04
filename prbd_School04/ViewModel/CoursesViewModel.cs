using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    class CoursesViewModel : ViewModelCommon {

        private ObservableCollection<Course> courses;

        public ObservableCollection<Course> Courses {
            get => courses;
            //set => SetProperty<ObservableCollection<Course>>(ref courses, value);
            set {
                courses = value;
                RaisePropertyChanged(nameof(courses)); //mis à jour de l'affichage
            }
        }
        public string Filter {
            get; set;
        }
        public ICommand ApplyFilter {
            get; set;
        }
        public ICommand DisplayCourseDetails {
            get; set;
        }
        public void makeList() {
            //Affiche la bonne liste des cours : soit le user connecté est un prof 
            // et donc affiche les cours que ce prof donne
            //soit le user connecté est un etudiant et affiche tout les cours
            if ((CurrentUser != null) && (CurrentUser.IsTeacher())) {
                Teacher teacher = (Teacher)CurrentUser;
                Courses = new ObservableCollection<Course>(teacher.CourseGiven);
            } else if ((CurrentUser != null) && (!CurrentUser.IsTeacher())) {
                Student student = (Student)CurrentUser;
                Courses = new ObservableCollection<Course>(App.Context.Courses);
            }
        }
        public CoursesViewModel() : base() {
            makeList();
            //possède une valeur qui est une référence vers une nouvelle instance de la classe ApplyFilterCommand
            ApplyFilter = new ApplyFilterCommand(ApplyFilterAction);

            DisplayCourseDetails = new RelayCommand<Course>(course => {
                Console.WriteLine(course.Title);
                NotifyColleagues(AppMessages.MSG_DISPLAY_COURSE, course);
            });
        }
        private void ApplyFilterAction() {
            Console.WriteLine("Search clicked! " + Filter);
            var query = from m in Context.Courses
                        where m.Title.Contains(Filter)
                        select m;
            Courses = new ObservableCollection<Course>(query);
            Console.WriteLine($"{query.Count()} courses found");
        }

        protected override void OnRefreshData() {
            // Pour plus tard
        }
    }
    public class ApplyFilterCommand : ICommand {
        private Action action;
        public event EventHandler CanExecuteChanged;
        //On veut que cette commande interroge le modèle pour retourner une liste filtrée de cours
        // et que l'affichage soit ensuite mis-à-jour pour afficher cette liste filtrée
        public ApplyFilterCommand( Action action ) {
            this.action = action;
        }
        //retourne un booléen qui est utilisé par le composant visuel (ici le bouton) 
        //pour savoir s'il doit être actif et permettre l'exécution de la commande, ou pas
        //si on retourne false, on verra que notre bouton sera désactivé et ne déclenchera
        //aucune action si on clique dessus
        public bool CanExecute( object parameter ) {
            return true;
        }
        public void Execute( object parameter ) {
            action();
        }
    
    }
}
