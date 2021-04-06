using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class Course : EntityBase<ModelSchool04> {

        public int CourseId {
            get; set;
        }
        public string titleOfCourse {
            get; set;
        }
        //Un cours est donné par tel professeur
        [Required]
        public virtual Teacher TeacherCourse {
            get; set;
        }
        public virtual ICollection<Question> QuestionList {
            get; set;
        } = new HashSet<Question>();
        public virtual ICollection<Quizz> QuizzCourse {
            get; set;
        } = new HashSet<Quizz>();
        public virtual ICollection<Registration> StudentsCourse {
            get; set;
        } = new HashSet<Registration>();
        public Course() {
        }
        public Course( string title, Teacher teacher) {
            titleOfCourse = title;
            TeacherCourse = teacher;
        }
    }
}
