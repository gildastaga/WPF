using Microsoft.EntityFrameworkCore;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public class ModelSchool04 : DbContextBase {
        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=school04");
        }

        public void SeedData() {
            Database.BeginTransaction();
            
            var Katia = new Student() { firstName = "Katia", name = "Mijares", mail = "c.mijareskatia@outlook.com", Password = "Password1," };
            var Penelle = new Teacher() { firstName = "Benoit", name = "Penelle" };
            var Corentin = new Student() { firstName = "Corentin", name = "Heinix" };
            Students.AddRange(Katia);
            Teachers.Add(Penelle);
            Users.Add(Corentin);
            SaveChanges();

            Database.CommitTransaction();
        }
        public DbSet<User> Users {
            get; set;
        }
        public DbSet<Student> Students {
            get; set;
        }
        public DbSet<Teacher> Teachers {
            get; set;
        }
        public DbSet<Course> Courses {
            get; set;
        }
        public DbSet<Answer> Answers {
            get; set;
        }
        public DbSet<Category> Categories {
            get; set;
        }
        public DbSet<Proposition> Propositions {
            get; set;
        }
        public DbSet<Question> Questions {
            get; set;
        }
        public DbSet<QuestionQuizz> QuestionQuizzs {
            get; set;
        }
        public DbSet<Registration> Registrations {
            get; set;
        }

        public DbSet<Quizz> Quizz {
            get; set;
        }
    }
}
