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
    class CoursesDetailsViewModel : ViewModelCommon {
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
        public ICommand SaveCourse { //commande du binding sur le bouton pour sauvegarder un cours
            get; set;
        }
        public ICommand CancelCourse { //commande du binding sur le bouton pour annuler les changements d'un cours
            get; set;
        }
        public ICommand DeleteCourse { //commande du binding sur le bouton pour delete un cours
            get; set;
        }
        public void makeList() {
        }
        public bool IsExisting {
            get => !isNew;
        }
        public CoursesDetailsViewModel() : base() {
            makeList();

            //SaveCourse = new RelayCommand(() => { NotifyColleagues(AppMessages.MSG_SAVE_COURSE); });
            //CancelCourse = new RelayCommand(() => { NotifyColleagues(AppMessages.MSG_CANCEL_COURSE); });
            //DeleteCourse = new RelayCommand(() => { NotifyColleagues(AppMessages.MSG_DELETE_COURSE);});
            //premier parametre est une action. Deuxieme paramètre va determiner si le bouton peut etre actif ou pas
            SaveCourse = new RelayCommand(SaveActionCourse, CanSaveOrCancelActionCourse); 
            //Cancel = new RelayCommand(CancelAction, CanCancelAction);
            //Delete = new RelayCommand(DeleteAction, () => !IsNew);
        }

        public void Init(Course course, bool isNew) {
            // Bind properties of child ViewModel
            //this.BindOneWay(nameof(Course), MemberMessages, nameof(MemberMessages.Member));

            // Il faut recharger ce membre dans le contexte courant pour pouvoir le modifier
            Course = isNew ? course : Course.GetById(course.CourseId);
            IsNew = isNew;

            RaisePropertyChanged();
        }
        protected override void OnRefreshData() {
            if (IsNew || Course == null)
                return;
            Course = Course.GetById(Course.CourseId);
            RaisePropertyChanged();
        }
        private void SaveActionCourse() {
            //On verifie si le course est nouveau
            if (IsNew) {
                // il faut ajouter l'entité dans la collection des entités gérées par EF
                Context.Add(Course);
                IsNew = false;
            }
            Context.SaveChanges();
            OnRefreshData();
            NotifyColleagues(AppMessages.MSG_COURSE_CHANGED, Course);
        }

        // determine si le bouton peut etre actif ou pas
        private bool CanSaveOrCancelActionCourse() {
            if (IsNew)
                return !string.IsNullOrEmpty(Title);
            return Course != null && (Context?.Entry(Course)?.State == EntityState.Modified);
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
