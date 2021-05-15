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
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public DateTime? ExaminationStartDate { get; set; }

        public DateTime? ExaminationEndDate { get; set; }
        [Required]
        public virtual Course Course { get; set; }
        public virtual ICollection<QuestionQuizz> QuestionsQuizz { get; set; } = new HashSet<QuestionQuizz>();
        public Quizz() {
        }
        public Quizz( string title, DateTime? creation, DateTime? examinationStart, DateTime? examinationEnd, Course course ) {
            Title = title;
            CreationDate = creation;
            ExaminationStartDate = examinationStart;
            ExaminationEndDate = examinationEnd;
            Course = course;
        }
        public static Quizz GetById(int id) {
            return Context.Quizz.SingleOrDefault(q => q.QuizzId == id);
        }
        public static IQueryable<Quizz> GetAll() {
            return Context.Quizz.OrderByDescending(m => m.ExaminationStartDate);
        }

        public static IQueryable<Quizz> GetQuizzesFromCourse(Course course) {
            return Context.Quizz.Where(q => q.Course == course);
        }

        public void Delete() { //TODO: Remove the linked QuestionsQuizz
            Context.Quizz.Remove(this);
            Context.SaveChanges();
        }

        public int QuestionsCount => QuestionsQuizz.Count();
    }
}
