using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msn.Model {

    public enum TypeQuestion {
        OneAnswer,
        ManyAnswer
    }

    public class Question {
        public int QuestionId { get; set; }
        public string Enonce { get; set; }
        public Boolean IsUpdate { get; set; }
        public Boolean IsDelete { get; set; }
        public TypeQuestion typeQuestion { get; set; } = TypeQuestion.OneAnswer;
        public virtual Category category { get; set; }

        public Question(string enonce, Boolean isUpdate, Boolean isDelete) {
            Enonce = enonce;
            IsUpdate = isUpdate;
            IsDelete = isDelete;
        }
    }
}
