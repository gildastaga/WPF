using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class QuestionProposition : EntityBase<ModelSchool04> {

        [Key]
        public int Id { get; set; }
        public virtual Question Question { get; set; }
        public virtual Proposition Proposition { get; set; }
    }
    
}
