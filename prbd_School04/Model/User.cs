using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public abstract class User : EntityBase<ModelSchool04> {
        public int UserId {
            get; set;
        }
        public string Name {
            get; set;
        }
        public string FirstName {
            get; set;
        }
        public string Mail {
            get; set;
        }

        public string Password {
            get; set;
        }

        public string Discriminator { get; }
        public User() {
        }
        public User(string name, string firstName, string mail, string password ) {
            Name = name;
            FirstName = firstName;
            Mail = mail;
            Password = password;
        }

        public bool IsTeacher() {
            return Discriminator == "Teacher";
        }
        public bool IsStudent() {
            return Discriminator == "Student";
        }

        public static User GetByMail(string mail) {
            return Context.Users.SingleOrDefault(m => m.Mail == mail);
        }

    }
}
