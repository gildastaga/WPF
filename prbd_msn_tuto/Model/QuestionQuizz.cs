using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msn.Model {
    public class QuestionQuizz : EntityBase<ModelSchool04> {
        public int NbPoint { get; set; }

        public QuestionQuizz(int nbPoint) {
            NbPoint = nbPoint;
        }

    }
}
