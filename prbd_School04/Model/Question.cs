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
       // public int QuestionName { get; set; }
        public string Enonce { get; set; }
        public Boolean IsUpdate { get; set; }
        public Boolean IsDelete { get; set; }
        public TypeQuestion typeQuestion { get; set; } = TypeQuestion.OneAnswer;
        public virtual ICollection<Category> Categories{get; set;} = new HashSet<Category>();
        public virtual ICollection<QuestionQuizz> QuestionQuizz {get; set;} = new HashSet<QuestionQuizz>();

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

        public Question(string enonce, Course course)
        {
            Enonce = enonce;
            Course = course;
        }

        public static IQueryable<Question> GetAvailableQuestionsForQuizz(Quizz quizz) {
            return Context.Questions.Where(q => q.Course.CourseId == quizz.Course.CourseId && !q.QuestionQuizz.Any(qq => qq.Question.QuestionId == q.QuestionId && qq.Quizz.QuizzId == quizz.QuizzId));
            /*var availableQuestions = from qq in Context.QuestionQuizzs
                where qq.Question.Course == quizz.Course
                orderby qq.Question.Enonce
                select qq.Question;
            return availableQuestions;*/
        }
        public static Question GetById(int id) {
            return Context.Questions.SingleOrDefault(q => q.QuestionId == id);
        }

        public int GoodPropositionsCount => Propositions.Where(p => p.Type == Type.True).Count();

        public void Delete() { //TODO: Remove the linked QuestionsQuizz
            Context.Questions.Remove(this);
            Context.SaveChanges();
        }

        public void RemoveElem(Answer a)
        {
            this.RemoveElem(a);
            Context.SaveChanges();
        }

    }
}
