using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class QuestionQuizz : EntityBase<ModelSchool04> {
        public int QuestionQuizzId {
            get; set; 
        }
        public int NbPoint { get; set; }

        public QuestionQuizz(int nbPoint) {
            NbPoint = nbPoint;
        }

    }
}
