using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class Student : User {
        public Student() {
        }
        public Student( string name, string firstName, string mail, string password ) : base(name, firstName, mail, password) {
            Name = name;
            FirstName = firstName;
            Mail = mail;
            Password = password;
        }
        public virtual ICollection<Answer> AnswerList {
            get; set;
        } = new HashSet<Answer>();
        public virtual ICollection<Registration> CoursesStudent {
            get; set;
        } = new HashSet<Registration>();

    }
}
