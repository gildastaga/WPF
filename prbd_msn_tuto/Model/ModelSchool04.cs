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

            var Katia = new Student("Mijares", "Katia", "abc@def", "abcdef");
            var Corentin = new Student("Heinix", "Corentin", "abc@def", "abcdef");
            var Benoit = new Teacher("Penelle", "Benoit", "benoit@penelle", "Penelle");
            var prwb = new Course {
                titleOfCourse = "PRBD",
                TeacherCourse = Benoit,
            };
            var web = new Course {
                titleOfCourse = "WEB",
                TeacherCourse = Benoit,
            };
            //var quest = new Question("Quelle est ta couleur pref ?", false, false);
            //var quest2 = new Question("Quelle est ta marque pref ?", false, false);
            Students.AddRange(Katia);
            Teachers.AddRange(Benoit);
            Courses.AddRange(prwb, web);
            prwb.CourseStrudent.Add(Katia);
            web.CourseStrudent.Add(Corentin);
            //Questions.AddRange(quest, quest2);

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
