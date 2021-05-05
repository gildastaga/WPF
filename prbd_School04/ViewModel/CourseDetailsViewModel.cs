using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    class CourseDetailsViewModel : ViewModelCommon {
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
        public void Init(Course course, bool isNew) {
            // Bind properties of child ViewModel
            //this.BindOneWay(nameof(Course), MemberMessages, nameof(MemberMessages.Member));

            // Il faut recharger ce membre dans le contexte courant pour pouvoir le modifier
            Course = isNew ? course : Course.GetById(course.CourseId);
            IsNew = isNew;

            RaisePropertyChanged();
        }

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
