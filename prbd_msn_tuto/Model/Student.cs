using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class Student : User {
        public Student( string name, string firstName, string mail, string password ) : base(name, firstName, mail, password) {
            Name = name;
            FirstName = firstName;
            Mail = mail;
            Password = password;
        }

    }
}
