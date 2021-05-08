using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class Course : EntityBase<ModelSchool04> {
        public static List<Course> lsCourses = new List<Course>();
        public int CourseId {
            get; set;
        }
        public int? Code {
            get; set; 
        }
        public string Title {
            get; set;
        }
        public string Description {
            get; set;
        }
        public int? MaxStudent {
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
        public Course(int? code, string title, string description, int? maxStudent, Teacher teacher) {
            Code = code;
            Title = title;
            Description = description;
            MaxStudent = maxStudent;
            TeacherCourse = teacher;
        }
        public string codeTitle() {
            return Code + " - " + Title;
        }
        public int NbElem => lsCourses.Count;

        public static void AddElem( Course c ) {
            lsCourses.Add(c);
            Context.SaveChanges();
        }
        public static bool RemoveElem( Course c ) {
            return lsCourses.Remove(c);
            Context.SaveChanges();
        }
        public static Course GetById(int courseId) {
            return Context.Courses.SingleOrDefault(c => c.CourseId == courseId);
        }
    }
}
