using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace School04.Model {
    public class Answer : EntityBase<ModelSchool04> {
        public int AnswerId { get; set; }

        [Required]
        public virtual Student Student { get; set; }

        [Required]
        public virtual QuestionQuizz Question { get; set; }

        public virtual ICollection<Proposition> ChoosedProposition { get; set; } = new HashSet<Proposition>();
    }
}
