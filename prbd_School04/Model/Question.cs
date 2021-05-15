using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {

    public enum TypeQuestion {
        OneAnswer,
        ManyAnswer
    }

    public class Question : EntityBase<ModelSchool04> {
        public int QuestionId { get; set; }
        public string Enonce { get; set; }
        public Boolean IsUpdate { get; set; }
        public Boolean IsDelete { get; set; }
        public TypeQuestion typeQuestion { get; set; } = TypeQuestion.OneAnswer;
        public virtual ICollection<Category> Categories{get; set;} = new HashSet<Category>();
        public virtual ICollection<Question> QuestionQuizz {get; set;} = new HashSet<Question>();

       public virtual ICollection<Answer> Answers{get; set;} = new HashSet<Answer>();

        public virtual ICollection<Proposition> Propositions { get; set; } = new HashSet<Proposition>();

        [Required]
        public virtual Course Course {get; set;}

        public Question() {}
        public Question(string enonce, Boolean isUpdate, Boolean isDelete, Course course) {
            Enonce = enonce;
            IsUpdate = isUpdate;
            IsDelete = isDelete;
            Course = course;
        }

    }
}
