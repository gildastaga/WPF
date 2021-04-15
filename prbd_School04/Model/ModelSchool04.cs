using Microsoft.EntityFrameworkCore;
using PRBD_Framework;
using System;
using Microsoft.Extensions.Logging;
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
        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Mail)
                .IsUnique();

            modelBuilder.Entity<QuestionQuizz>()
                .HasKey(qq => new { qq.QuizzId, qq.QuestionId });

            modelBuilder.Entity<Registration>()
                .HasKey(r => new { r.CourseId, r.StudentId });

            modelBuilder.Entity<Teacher>()
                // avec, du côté many, la propriété CourseGiven ...
                .HasMany(t => t.CourseGiven)
                // avec, du côté one, la propriété TeacherCourse ...
                .WithOne(c => c.TeacherCourse)
                // et pour laquelle on désactive le delete en cascade
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.TeacherCourse)
                .WithMany(t => t.CourseGiven);


            modelBuilder.Entity<QuestionQuizz>()
                .HasOne(qq => qq.Quizz)
                .WithMany(q => q.QuestionsQuizz)
                .OnDelete(DeleteBehavior.ClientCascade);

            // l'entité Question participe à une relation one-to-many ...
            modelBuilder.Entity<Question>()
                // avec, du côté many, la propriété Propositions ...
                .HasMany(q => q.Propositions)
                // avec, du côté one, la propriété Question ...
                .WithOne(p => p.Question)
                // et pour laquelle on désactive le delete en cascade
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        public void SeedData() {
            Database.BeginTransaction();

            var Katia = new Student("Mijares", "Katia", "abc@def", "abcdef");
            var Corentin = new Student("Heinix", "Corentin", "ghj@def", "abcdef");
            var Benoit = new Teacher("Penelle", "Benoit", "benoit@penelle", "Penelle1");
            var Boris = new Teacher("Verhagen", "Boris", "boris@verhagen", "Boris1");
            Students.AddRange(Katia, Corentin);
            Teachers.AddRange(Benoit, Boris);
            var prwb = new Course("prwb", "cours de prwb", Benoit);
            var web = new Course("WEB", "cours de web", Benoit);
            var SGBD = new Course("SGBD", "cours de sgbd", Boris);
            Courses.AddRange(prwb, web, SGBD);
            Benoit.CourseGiven.Add(SGBD);
            Benoit.CourseGiven.Add(web);
            Boris.CourseGiven.Add(prwb);
            var enreg1 = new Registration(Corentin, prwb);
            var enreg2 = new Registration(Katia, web);
            Registrations.AddRange(enreg1, enreg2);
            Katia.CoursesStudent.Add(enreg2);
            Corentin.CoursesStudent.Add(enreg1);
            web.StudentsCourse.Add(enreg2);
            prwb.StudentsCourse.Add(enreg1);
            var quest1 = new Question {
                Enonce = "Quelle est le langage utilisé ?",
                IsUpdate = false,
                IsDelete = false,
                Course = web
            };
            var quest2 = new Question("Qu'est-ce que le SQL?", false, false, SGBD);
            Questions.AddRange(quest1, quest2);

            var quest3 = new Question {
                Enonce = "Choisir la réponse avec un (V) :",
                Course = web
            };
            var quest4 = new Question {
                Enonce = "Choisir la ou les réponses avec un (V) :",
                Course = web,
                typeQuestion = TypeQuestion.ManyAnswer
            };
            Questions.AddRange(quest3, quest4);

            var prop1 = new Proposition ("Proposition 1 (X)", Type.False, quest1);
            var prop2 = new Proposition ("Proposition 2 (V)", Type.True, quest1);
            var prop3 = new Proposition ("Proposition 3 (X)", Type.False, quest2);
            var prop4 = new Proposition("Proposition 4 (V)", Type.True, quest2) ;
            var prop5 = new Proposition ("Proposition 5 (V)", Type.True, quest3);
            Propositions.AddRange(prop1, prop2, prop3, prop4, prop5);

            var quiz1 = new Quizz {
                Course = prwb,
                Title = "Premier quiz"
            };
            Quizz.AddRange(quiz1);

            var questQuizz1 = new QuestionQuizz {
                Quizz = quiz1,
                Question = quest1,
                NbPoint = 10
            };
            var questQuizz2 = new QuestionQuizz {
                Quizz = quiz1,
                Question = quest2,
                NbPoint = 25
            };
            QuestionQuizzs.AddRange(questQuizz1, questQuizz2);

            quiz1.QuestionsQuizz.Add(questQuizz1);
            quiz1.QuestionsQuizz.Add(questQuizz2);

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
        public DbSet<QuestionProposition> QuestionPropositions { get; set; }
    }
}
