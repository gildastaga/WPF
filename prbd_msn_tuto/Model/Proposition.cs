using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {

    public enum Type {
        True,
        False
    }
    public class Proposition : EntityBase<ModelSchool04> {
        public int PropositionId { get; set; }
        public string Body { get; set; }
        public Type Type { get; set; } = Type.True;
        [Required]
        public virtual Question Question {
            get; set;
        }
        /*public virtual ICollection<Answer> Answers {
            get; set;
        } = new HashSet<Answer>();*/

        public Proposition(string body, Type type) {
            Body = body;
            Type = type;
        }
    }
}
