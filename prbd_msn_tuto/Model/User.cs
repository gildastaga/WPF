using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msn.Model {
    public abstract class User : EntityBase<ModelSchool04> {
        public int UserId {
            get; set;
        }
        public string name {
            get; set;
        }
        public string firstName {
            get; set;
        }
        public string mail {
            get; set;
        }
        public string Password {
            get; set;
        }
    }
}
