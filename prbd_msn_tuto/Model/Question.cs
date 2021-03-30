using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual ICollection<Category> Categories{
            get; set;
        } = new HashSet<Category>();
        public virtual ICollection<QuestionQuizz> QuestionQuizz {
            get; set;
        } = new HashSet<QuestionQuizz>();
        public virtual ICollection<Proposition> Propositions {
            get; set;
        } = new HashSet<Proposition>();

        [Required]
        public virtual Course Course {
            get; set;
        }
        public Question(string enonce, Boolean isUpdate, Boolean isDelete) {
            Enonce = enonce;
            IsUpdate = isUpdate;
            IsDelete = isDelete;
        }
    }
}
