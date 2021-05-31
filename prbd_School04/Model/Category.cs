using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class Category : EntityBase<ModelSchool04> {

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual Question Question { get; set; }

        public virtual ICollection<QuestionCateg> QuestionsCateg { get; set; } = new HashSet<QuestionCateg>();
        [NotMapped]
        public IEnumerable<Question> Questions { get => QuestionsCateg.Select(qt => qt.Question); }


        public Category() {}

        public Category(string Name, Question Question) {

            this.Name = Name;
            this.Question = Question;
        }

        public void Delete() {
            // Supprime la catégorie elle-même
            Context.Categories.Remove(this);
            Context.SaveChanges();
        }

        [NotMapped]
        public bool IsChecked { get; set; } = true;

        [NotMapped]
        public int NbQuestions { get => Questions.Count(); }

    }
}
