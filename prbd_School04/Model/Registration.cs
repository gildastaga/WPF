using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace School04.Model {
    public enum State { Active, Valide, Inactive }
    public class Registration : EntityBase<ModelSchool04> {

        public State RegistrationState {
            get; set;
        } = State.Inactive;
        public int StudentId {
            get; set;
        }
        public int CourseId {
            get; set;
        }

        [Required]
        public virtual Student Student { get; set; }

        [Required]
        public virtual Course Course { get; set; }

        public Registration() {
        }
        public Registration( Student student, Course course ) {
            Student = student;
            Course = course;
        }
        public static IQueryable<Registration> GetCurrentRegistrationsFromCourse(Course course) {
            return Context.Registrations.Where(r => r.Course == course);
        }
        public static IQueryable<User> GetNoRegistrationsFromCourse(Course course) {
            var filtered = from s in Context.Students
                           where s.CoursesStudent.All(s => s.Course != course)
                           orderby s.FirstName
                           select s;
            return filtered;
        }

        public string SwitchLabel {
            get {
                switch (((State)RegistrationState).GetHashCode()) {
                    case 0:
                        return "Deactivate";
                    case 1:
                        return "Validate";
                    case 2:
                        return "Activate";
                    default:
                        return "Deactivate";
                }
            }
        }

        public string StudentName => Student.ToString();
    }
}
