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
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=school04").UseLazyLoadingProxies(true); //Charge les liens entre les objets sql de façon automatique
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
            var Marceline = new Student("Momosoh", "Marceline", "marceline@epfc.eu", "Marceline1.");
            var Jean = new Student("Sérien", "Jean", "jean@mail.be", "JeanPass1.");
            var Pierre = new Student("Kiroule", "Pierre", "pierre@epfc.eu", "Pierre1.");
            var Paul = new Student("Itesse", "Paul", "paul@gmail.com", "PaulPass1.");
            var Benoit = new Teacher("Penelle", "Benoit", "benoit@penelle", "Penelle1");
            var Boris = new Teacher("Verhagen", "Boris", "boris@verhagen", "Boris1");
            Students.AddRange(Katia, Corentin, Marceline, Jean, Pierre, Paul);
            Teachers.AddRange(Benoit, Boris);
            var anc3 = new Course(1868, "ANC3", "Projet d'analyse et de conception", 10, Benoit);
            var map4 = new Course(1914, "MAP4", "Mathématique appliquées à l'informatique", 12, Benoit);
            var prbd = new Course(1975, "PRBD", "Projet de développement SGBD", 15, Benoit);
            var sgbd = new Course(1976, "PRBB", "Projet de développement SGBD", 13, Boris);
            var prm2 = new Course(1900, "PRM2", "Principes algorithmiques et programmation", 5, Boris);
            var prwb = new Course(1930, "PRWB", "Projet de développement Web", 2, Benoit);
            var tgpr = new Course(1963, "TGPR", "Technique de gestion de projets", 8, Benoit);
            var bnet = new Course(1958, "BNET", "réseaux et sécurité", 8, Benoit);
            var snet = new Course(1964, "SNET", "réseaux et sécurité", 8, Benoit);
            var algo = new Course(1854, "ALGO", "cours d'algo", 8, Benoit);
            var chimie = new Course(1987, "CHM", "cours de chimie", 8, Benoit);
            var phys = new Course(1985, "PHY", "cours de physique", 8, Benoit);
            var ang = new Course(1966, "ANG", "cours d'anglais", 8, Benoit);
            Courses.AddRange(anc3, map4, prbd, sgbd, prm2, prwb, tgpr, bnet, snet, algo, chimie, phys, ang);
            Benoit.CourseGiven.Add(sgbd);
            Boris.CourseGiven.Add(prbd);
            var enreg1 = new Registration(Corentin, prbd);
            var enreg2 = new Registration(Katia, map4);
            var enreg3 = new Registration(Katia, prwb);
            var enreg4 = new Registration(Katia, tgpr);
            var enreg5 = new Registration(Corentin, tgpr);
            var enreg6 = new Registration(Katia, prbd);
            var enreg7 = new Registration(Marceline, prbd);
            var enreg8 = new Registration(Pierre, prbd);
            var enreg9 = new Registration(Paul, prbd);
            var enreg10 = new Registration(Jean, prbd);
            enreg5.RegistrationState = State.Active;
            Registrations.AddRange(enreg1, enreg2, enreg3, enreg4, enreg5, enreg6, enreg7, enreg8, enreg9, enreg10);
            Katia.CoursesStudent.Add(enreg2);
            Corentin.CoursesStudent.Add(enreg1);
            anc3.StudentsCourse.Add(enreg2);
            prbd.StudentsCourse.Add(enreg1);
            prbd.StudentsCourse.Add(enreg6);
            prbd.StudentsCourse.Add(enreg7);
            prbd.StudentsCourse.Add(enreg8);
            prbd.StudentsCourse.Add(enreg9);
            prbd.StudentsCourse.Add(enreg10);

            var quest1 = new Question {
                Enonce = "-Quelle est le langage utilisé ?",
                IsUpdate = false,
                IsDelete = false,
                Course = prwb,
                typeQuestion = TypeQuestion.ManyAnswer

            };

            var quest2 = new Question {
                Enonce = "-Qu'est-ce que le SQL Server?",
                IsUpdate = false,
                IsDelete = false,
                Course = sgbd,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest3 = new Question {
                Enonce = "-Choisir la réponse avec un (V) :",
                IsUpdate = false,
                IsDelete = false,
                Course = prm2,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            Questions.AddRange(quest1, quest2, quest3);

            var quest4 = new Question {
                Enonce = "-Qu'est ce que le PHP:",
                Course = anc3,
                typeQuestion = TypeQuestion.OneAnswer
            };
            var quest5 = new Question {
                Enonce = "-Choisir les nombres pairs",
                Course = map4,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest6 = new Question {
                Enonce = "-Que signifie TGPR",
                Course = tgpr,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest7 = new Question {
                Enonce = "-Qu'est ce que la géométrie",
                Course = map4,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest8 = new Question {
                Enonce = "-Qu'est ce que l'informatique",
                Course = prbd,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest9 = new Question {
                Enonce = "-Qu'entend-t-on par automatisation",
                Course = phys,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest10 = new Question {
                Enonce = "Qu'est ce que l'homogénéité",
                Course = chimie,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest11 = new Question {
                Enonce = "Que signifie hétérogène",
                Course = chimie,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest12 = new Question {
                Enonce = "what is the push of Archimedes",
                Course = ang,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest13 = new Question {
                Enonce = "what is parity",
                Course = ang,
                typeQuestion = TypeQuestion.ManyAnswer
            };
            Questions.AddRange(quest4, quest5, quest6, quest7, quest8, quest9, quest10, quest11, quest12, quest13);

            var prop1 = new Proposition ("C#", Type.True);
            var prop11 = new Proposition("javaFX", Type.False);
            var prop111 = new Proposition("wpf", Type.True);
            var prop2 = new Proposition ("server mobile", Type.False);
            var prop22 = new Proposition("système de gestion de BD", Type.True) ;
            var prop3 = new Proposition ("acess", Type.False);
            var prop33 = new Proposition("programmation", Type.True);
            var prop333 = new Proposition("informatique", Type.True);
            var prop4 = new Proposition("langage", Type.False);
            var prop44 = new Proposition("langage informatique", Type.True);
            var prop5 = new Proposition("6", Type.True);
            var prop55 = new Proposition("1", Type.False);
            var prop555 = new Proposition("8", Type.True);
            Propositions.AddRange(prop1, prop11, prop111, prop2, prop22, prop3, prop33, prop333, prop4, prop44,
                prop5, prop55, prop555);
            
            quest1.Propositions.Add(prop1);
            quest1.Propositions.Add(prop11);
            quest1.Propositions.Add(prop111);
            quest2.Propositions.Add(prop2);
            quest2.Propositions.Add(prop22);
            quest3.Propositions.Add(prop3);
            quest3.Propositions.Add(prop33);
            quest3.Propositions.Add(prop333);
            quest4.Propositions.Add(prop4);
            quest4.Propositions.Add(prop44);
            quest5.Propositions.Add(prop5);
            quest5.Propositions.Add(prop55);
            quest5.Propositions.Add(prop555);

            var quiz1 = new Quizz {
                Course = prbd,
                Title = "Premier quiz"
            };
            
            var quiz2 = new Quizz {
                Course = tgpr,
                Title = "Deuxième quiz"
            };
            Quizz.AddRange(quiz1, quiz2);

            var questQuizz1 = new QuestionQuizz {
                Quizz = quiz1,
                Question = quest1,
                NbPoint = 10,
                PosQuestionInQuizz = 1
            };
            var questQuizz2 = new QuestionQuizz {
                Quizz = quiz1,
                Question = quest2,
                NbPoint = 25,
                PosQuestionInQuizz = 2
            };
            QuestionQuizzs.AddRange(questQuizz1, questQuizz2);

            quiz1.QuestionsQuizz.Add(questQuizz1);
            quiz1.QuestionsQuizz.Add(questQuizz2);

            var cat1 = new Category("Arithmétique", quest1);
            var cat2 = new Category("Géométrie", quest1);
            var cat3 = new Category("Logique", quest2);
            var cat4 = new Category("Mathématiques", quest3);
            var cat5 = new Category("Test", quest4);
            Categories.AddRange(cat1, cat2, cat3, cat4, cat5);

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
