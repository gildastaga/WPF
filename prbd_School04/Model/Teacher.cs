using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class Teacher : User {
        public Teacher() {
        }
        public Teacher( string name, string firstName, string profile, string mail, string password ): base(name, firstName, profile, mail, password) {
            Name = name;
            FirstName = firstName;
            Profile = profile; 
            Mail = mail;
            Password = password;
        }
        public Teacher( string name, string firstName, string mail, string password ) : base(name, firstName, mail, password) {
            Name = name;
            FirstName = firstName;
            Mail = mail;
            Password = password;
        }
        //Ensemble de cours donnés par un professeur
        public virtual ICollection<Course> CourseGiven {
            get; set;
        } = new HashSet<Course>();

        public virtual List<Course> lsCourses { get; } = new List<Course>();
    }
}
