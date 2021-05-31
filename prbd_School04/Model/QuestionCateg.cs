using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model
{
    public class QuestionCateg : EntityBase<ModelSchool04>
    {
        public int QuestionCategId { get; set; }
        public virtual Question Question { get; set; }
        public virtual Category Category { get; set; }

        public QuestionCateg(Question question, Category category)
        {
            Question = question;
            Category = category;
        }

        public void Delete()
        {
            // Supprime la catégorie elle-même
            Context.QuestionCategs.Remove(this);
            Context.SaveChanges();
        }

        public QuestionCateg()
        {
        }
    }
}
