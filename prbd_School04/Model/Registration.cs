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
        public static List<Registration> lsRegistration = new List<Registration>();
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
        //return une collection de registration inscrit à un cours spécifique
        public static IQueryable<Registration> GetCurrentRegistrationsFromCourse(Course course) {
            return Context.Registrations.Where(r => r.Course == course);
        }
        //return une collection de registration qui ne sont pas inscrit à un cours spécifique
        public static IQueryable<Student> GetNoRegistrationsFromCourse(Course course) {
            var noRegistrations = from s in Context.Students
                           where s.CoursesStudent.All(s => s.Course != course)
                           orderby s.FirstName
                           select s;
            return noRegistrations;
        }

        public static IQueryable<User> GetFiltredNoRegistrationsFromCourse(Course course, string Filter) {
            var filtered = GetNoRegistrationsFromCourse(course).Where(s => s.FirstName.Contains(Filter) || s.Name.Contains(Filter));
            return filtered;
            /*var filtered = from s in Context.Students
                           where s.CoursesStudent.All(s => s.Course != course) && (s.FirstName.Contains(Filter) || s.Name.Contains(Filter))
                           orderby s.FirstName
                           select s;
            return filtered;*/
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

        public static Registration[] DeleteRegistrations(params Registration[] registrations) {
            var deleted = new List<Registration>();
            foreach (var r in registrations) {
                Context.Registrations.Remove(r);
                deleted.Add(r);
            }
            //Context.SaveChanges();
            return deleted.ToArray();
        }
        public static Registration[] AddRegistrations(Course course, params User[] students) {
            var added = new List<Registration>();
            foreach (Student s in students) {
                var r = new Registration(s, course);
                r.RegistrationState = State.Active;
                Context.Registrations.Add(r);
                added.Add(r);
            }
            //Context.SaveChanges();
            return added.ToArray();
        }
    }
}
