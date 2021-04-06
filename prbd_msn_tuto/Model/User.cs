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
        public User() {
        }
        public User(string name, string firstName, string mail, string password ) {
            Name = name;
            FirstName = firstName;
            Mail = mail;
            Password = password;
        }

    }
}
