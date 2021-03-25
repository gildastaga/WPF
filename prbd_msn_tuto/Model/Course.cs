using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class Course : EntityBase<ModelSchool04> {
        public int CourseId {
            get; set;
        }
        public string titleOfCourse {
            get; set;
        }
    }
}
