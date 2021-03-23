using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_msn_tuto {
    class User {

        public User( string name, string firstName, string mail, string password ) {
        }

        private string name; 
        public string Name {
            get; set; 
        }

        private string firstname;
        public string FirstName {
            get; set;
        }

        private string mail;
        public string Mail {
            get; set; 
        }

        private string password;
        public string Password {
            get; set;
        }
    }
}
