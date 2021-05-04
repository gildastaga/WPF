using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class Category : EntityBase<ModelSchool04> {

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual Question Question { get; set; }

        public Category() {}

        public Category(string Name) {
            this.Name = Name;
            this.Question = Question;
        }

        public void Delete() {
            // Supprime la catégorie elle-même
            Context.Categories.Remove(this);
            Context.SaveChanges();
        }
    }
}
