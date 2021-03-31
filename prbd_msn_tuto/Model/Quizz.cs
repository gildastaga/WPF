using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace School04.Model {
    public class Quizz : EntityBase<ModelSchool04> {
        public Quizz(string title) {
            Title = title; 
        }

        public int QuizzId { get; set; }

        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExaminationDate { get; set; }

        /*[Required]
        public virtual Course Course { get; set; }*/

        public virtual ICollection<QuestionQuizz> Questions { get; set; } = new HashSet<QuestionQuizz>();
    }
}
