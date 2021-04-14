using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace School04.Model {
    public enum State { ACTIVE, VALIDE, INACTIVE }
    public class Registration : EntityBase<ModelSchool04> {

        public State RegistrationState {
            get; set;
        } = State.INACTIVE;
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
    }
}
