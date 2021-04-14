﻿using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class QuestionQuizz : EntityBase<ModelSchool04> {
        //public int QuestionQuizzId {
        //    get; set; 
        //}
        public int NbPoint {
            get; set;
        }
        public int QuestionId {
            get; set;
        }
        [Required]
        public virtual Question Question {
            get; set;
        }
        public int QuizzId {
            get; set;
        }
        [Required]
        public virtual Quizz Quizz {
            get; set;
        }
        /*public virtual ICollection<Answer> Answers {
            get; set;
        } = new HashSet<Answer>();*/

        public QuestionQuizz() { }
        public QuestionQuizz( int nbPoint ) {
            NbPoint = nbPoint;
        }

    }
}
