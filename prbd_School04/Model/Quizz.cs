using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace School04.Model {
    public class Quizz : EntityBase<ModelSchool04> {
        public int QuizzId { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime ExaminationDate { get; set; }
        [Required]
        public virtual Course Course { get; set; }
        public virtual ICollection<QuestionQuizz> QuestionsQuizz { get; set; } = new HashSet<QuestionQuizz>();
        public Quizz() {
        }
        public Quizz( string title, DateTime creation, DateTime examination, Course course ) {
            Title = title;
            CreationDate = creation;
            ExaminationDate = examination;
            Course = course;
        }
    }
}
