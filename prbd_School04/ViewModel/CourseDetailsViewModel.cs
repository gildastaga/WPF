using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    class CourseDetailsViewModel : ViewModelCommon {
        public event Action<Course> DisplayCourseTabsTeacher;
        public event Action<Course> DisplayCourseTabsStudent;

        private Course course;
        public Course Course { get => course; set => SetProperty(ref course, value); }

        private bool isNew;
        public bool IsNew {
            get { return isNew; }
            set {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew));
            }
        }
        //commande du binding sur le bouton pour sauvegarder un cours
        public ICommand SaveCourse { 
            get; set;
        }
        //commande du binding sur le bouton pour annuler les changements d'un cours
        public ICommand CancelCourse { 
            get; set;
        }
        //commande du binding sur le bouton pour delete un cours
        public ICommand DeleteCourse { 
            get; set;
        }
        public void makeList() {
        }
        public bool IsExisting {
            get => !isNew;
        }
        public CourseDetailsViewModel() : base() {
            makeList();
            //premier parametre est une action. Deuxieme paramètre va determiner si le bouton peut etre actif ou pas
            SaveCourse = new RelayCommand(SaveActionCourse, CanSaveActionCourse); 
            CancelCourse = new RelayCommand(CancelActionCourse, CanCancelActionCourse);
            DeleteCourse = new RelayCommand(DeleteActionCourse, () => !IsNew);

            Register(this, AppMessages.MSG_UPDATE_PROFILE, OnRefreshData);
        }

        public void Init(Course course, bool isNew) {
            // Bind properties of child ViewModel
            //this.BindOneWay(nameof(Course), MemberMessages, nameof(MemberMessages.Member));

            // Il faut recharger ce membre dans le contexte courant pour pouvoir le modifier
            Course = isNew ? course : Course.GetById(course.CourseId);
            IsNew = isNew;
            if(!isNew && CurrentUser.IsTeacher())
                DisplayCourseTabsTeacher?.Invoke(Course);
            else if(!isNew && CurrentUser.IsStudent())
                DisplayCourseTabsStudent?.Invoke(Course);

            RaisePropertyChanged();
        }
        protected override void OnRefreshData() {
            if (IsNew || Course == null)
                return;
            Course = Course.GetById(Course.CourseId);
            RaisePropertyChanged();
        }
        private void SaveActionCourse() {
            //Console.WriteLine("SaveCourse");
            //On verifie si le course est nouveau
            if (IsNew) {
                // il faut ajouter l'entité dans la collection des entités gérées par EF
                Context.Add(Course);
                IsNew = false;
                Context.SaveChanges();
                DisplayCourseTabsTeacher?.Invoke(Course);
            } else {
                Context.SaveChanges();
            }
            //OnRefreshData();
            NotifyColleagues(AppMessages.MSG_COURSE_CHANGED, Course);
        }
        // determine si le bouton peut etre actif ou pas
        private bool CanSaveActionCourse() {
            if (IsNew)
                return !string.IsNullOrEmpty(Title) && Code != null;
            return Course != null && Code != null && !string.IsNullOrEmpty(Title) && MaxCapacity >= Course.StudentsCourse.Count() && (Context?.Entry(Course)?.State == EntityState.Modified);
        }
        private void CancelActionCourse() {
            if (IsNew) {
                //ferme le tabControl
                NotifyColleagues(AppMessages.MSG_CLOSE_TAB_COURSE, course);
            } else {
                Context.Reload(Course);
                RaisePropertyChanged();
            }
        }
        private bool CanCancelActionCourse() {
            return Course != null && (IsNew || Context?.Entry(Course)?.State == EntityState.Modified);
        }
        private void DeleteActionCourse() {
            CancelActionCourse();
            Course.Delete();
            NotifyColleagues(AppMessages.MSG_COURSE_CHANGED, Course);
            //ferme le tabControl
            NotifyColleagues(AppMessages.MSG_CLOSE_TAB_COURSE, Course);
        }
        //ici, on crée les propriétés pour les différents champs qui sont bindés dans la CourseDetailsView.xaml
        public int? Code {
            get { return Course?.Code; }
            set {
                Course.Code = value;
                RaisePropertyChanged(nameof(Code));
            }
        }

        public string Title {
            get { return Course?.Title; }
            set {
                Course.Title = value;
                RaisePropertyChanged(nameof(Title));
                // Pour pouvoir mettre à jour l'en-tête de l'onglet en cas de changement de titre de course
                NotifyColleagues(AppMessages.MSG_COURSE_CHANGED, Course);
            }
        }

        public string Description {
            get { return Course?.Description; }
            set {
                Course.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public int? MaxCapacity {
            get { return Course?.MaxStudent; }
            set {
                Course.MaxStudent = value.Value;
                RaisePropertyChanged(nameof(MaxCapacity));
            }
        }

        public string Teacher {
            get { return Course?.TeacherCourse.ToString(); }
        }
    }
}
