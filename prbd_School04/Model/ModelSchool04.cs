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
            enreg2.RegistrationState = State.Active;
            enreg3.RegistrationState = State.Active;
            enreg4.RegistrationState = State.Pending;
            enreg6.RegistrationState = State.Active;
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

            var quest11 = new Question {
                Enonce = "-Quelle est le langage utilisé ?",
                IsUpdate = false,
                IsDelete = false,
                Course = tgpr,
                typeQuestion = TypeQuestion.ManyAnswer

            };

            var quest111 = new Question {
                Enonce = "-Quelle est le langage utilisé ?",
                IsUpdate = false,
                IsDelete = false,
                Course = prm2,
                typeQuestion = TypeQuestion.ManyAnswer

            };

            var quest2 = new Question {
                Enonce = "-Qu'est-ce que le SQL Server?",
                IsUpdate = false,
                IsDelete = false,
                Course = prwb,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest22 = new Question {
                Enonce = "-Qu'est-ce que le SQL Server?",
                IsUpdate = false,
                IsDelete = false,
                Course = prwb,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest222 = new Question {
                Enonce = "-Qu'est-ce que le SQL Server?",
                IsUpdate = false,
                IsDelete = false,
                Course = prm2,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest3 = new Question {
                Enonce = "-Choisir la réponse avec un (V) :",
                IsUpdate = false,
                IsDelete = false,
                Course = prm2,
                typeQuestion = TypeQuestion.ManyAnswer
            };


            var quest33 = new Question {
                Enonce = "-Choisir la réponse avec un (V) :",
                IsUpdate = false,
                IsDelete = false,
                Course = prwb,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest333 = new Question {
                Enonce = "-Choisir la réponse avec un (V) :",
                IsUpdate = false,
                IsDelete = false,
                Course = tgpr,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            Questions.AddRange(quest1, quest11, quest111, quest2, quest22, quest222, quest3, quest33, quest333);

            var quest4 = new Question {
                Enonce = "-Qu'est ce que le PHP:",
                Course = anc3,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest44 = new Question {
                Enonce = "-Qu'est ce que le PHP:",
                Course = map4,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest444 = new Question {
                Enonce = "-Qu'est ce que le PHP:",
                Course = prbd,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest5 = new Question {
                Enonce = "-Choisir les nombres pairs",
                Course = map4,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest55 = new Question {
                Enonce = "-Choisir les nombres pairs",
                Course = anc3,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest555 = new Question {
                Enonce = "-Choisir les nombres pairs",
                Course = prbd,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest6 = new Question {
                Enonce = "-Qu'est ce que l'informatique",
                Course = prbd,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest66 = new Question {
                Enonce = "-Qu'est ce que l'informatique",
                Course = map4,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest665 = new Question {
                Enonce = "-Qu'est ce que l'informatique",
                Course = anc3,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest7 = new Question {
                Enonce = "-Que signifie TGPR",
                Course = tgpr,
                typeQuestion = TypeQuestion.OneAnswer
            };
            var quest77 = new Question {
                Enonce = "-Que signifie TGPR",
                Course = bnet,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest777 = new Question {
                Enonce = "-Que signifie TGPR",
                Course = snet,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest8 = new Question {
                Enonce = "-Qu'est ce que la géométrie",
                Course = bnet,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest88 = new Question {
                Enonce = "-Qu'est ce que la géométrie",
                Course = snet,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest888 = new Question {
                Enonce = "-Qu'est ce que la géométrie",
                Course = tgpr,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest9 = new Question {
                Enonce = "-Qu'est ce que le réseau",
                Course = snet,
                typeQuestion = TypeQuestion.OneAnswer
            };


            var quest99 = new Question {
                Enonce = "-Qu'est ce que le réseau",
                Course = bnet,
                typeQuestion = TypeQuestion.OneAnswer
            };


            var quest999 = new Question {
                Enonce = "-Qu'est ce que le réseau",
                Course = tgpr,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest10 = new Question {
                Enonce = "-Qu'entend-t-on par automatisation",
                Course = phys,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest100 = new Question {
                Enonce = "Qu'est ce que l'homogénéité",
                Course = chimie,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest1000 = new Question {
                Enonce = "Qu'est ce que l'homogénéité",
                Course = algo,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest12 = new Question {
                Enonce = "Que signifie hétérogène",
                Course = chimie,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest122 = new Question {
                Enonce = "Que signifie hétérogène",
                Course = phys,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest1222 = new Question {
                Enonce = "Que signifie hétérogène",
                Course = algo,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest13 = new Question {
                Enonce = "what is the push of Archimedes",
                Course = ang,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest133 = new Question {
                Enonce = "what is the push of Archimedes",
                Course = phys,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest1333 = new Question {
                Enonce = "what is the push of Archimedes",
                Course = algo,
                typeQuestion = TypeQuestion.OneAnswer
            };

            var quest14 = new Question {
                Enonce = "what is parity",
                Course = ang,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest144 = new Question {
                Enonce = "what is parity",
                Course = chimie,
                typeQuestion = TypeQuestion.ManyAnswer
            };

            var quest1444 = new Question {
                Enonce = "what is management",
                Course = ang,
                typeQuestion = TypeQuestion.ManyAnswer
            };
            Questions.AddRange(quest4, quest44, quest444, quest5, quest55, quest555, quest6, quest66, quest665, 
                quest7, quest77, quest777, quest8, quest88, quest888, quest9, quest99, quest999, 
                quest10, quest100, quest1000, quest12, quest122, quest1222, quest13, quest133, quest1333,
                quest14, quest144, quest1444);  

            var prop1 = new Proposition ("C#", Type.True);
            var prop2 = new Proposition("WPF", Type.True);
            var prop3 = new Proposition("javaFX", Type.False);
            var prop4 = new Proposition("wpf", Type.False);
            var prop5 = new Proposition ("server mobile", Type.False);
            var prop6 = new Proposition("système de gestion de BD", Type.True) ;
            var prop7 = new Proposition ("acess", Type.False);
            var prop8 = new Proposition("programmation", Type.True);
            var prop9 = new Proposition("informatique", Type.True);
            var prop10 = new Proposition("langage", Type.False);
            var prop11 = new Proposition("langage informatique", Type.True);
            var prop12 = new Proposition("6", Type.True);
            var prop13 = new Proposition("1", Type.False);
            var prop14 = new Proposition("8", Type.True);
            var prop15 = new Proposition("Angular", Type.True);
            var prop16 = new Proposition("Data Acess", Type.True);
            var prop17 = new Proposition("Information", Type.True);
            var prop18 = new Proposition("10", Type.True);
            var prop19 = new Proposition("Réseaux", Type.True);
            var prop20 = new Proposition("Routage", Type.True);
            var prop21 = new Proposition("Machine Virtuelle", Type.True);
            var prop22 = new Proposition("20", Type.True);
            var prop23 = new Proposition("Php", Type.True);
            var prop24 = new Proposition("JavaScript", Type.True);
            var prop25 = new Proposition("React", Type.True);
            Propositions.AddRange(prop1, prop2, prop3, prop4, prop5, prop3, prop6, prop7, prop8, prop9,
                prop10, prop11, prop12, prop13, prop14, prop15, prop16, prop17, prop18, prop19, prop20, prop21,
                prop22, prop23, prop24, prop25);
            
            //Propositions de PRWB
            quest1.Propositions.Add(prop1);
            quest1.Propositions.Add(prop2);
            quest1.Propositions.Add(prop3); 
            quest2.Propositions.Add(prop4);
            quest2.Propositions.Add(prop5);
            quest2.Propositions.Add(prop6);
            
            //Propositions de TGPR
            quest11.Propositions.Add(prop7);
            quest11.Propositions.Add(prop8);
            quest11.Propositions.Add(prop9);
            quest333.Propositions.Add(prop10);
            quest333.Propositions.Add(prop11);
            quest333.Propositions.Add(prop12);

            //Propositions de ANC3
            quest4.Propositions.Add(prop13);
            quest4.Propositions.Add(prop14);
            quest55.Propositions.Add(prop15);
            quest55.Propositions.Add(prop16);
            quest55.Propositions.Add(prop17);
            quest665.Propositions.Add(prop18);
            quest665.Propositions.Add(prop19);
            quest665.Propositions.Add(prop20);

            //Propositions de MAP4
            quest66.Propositions.Add(prop21);
            quest66.Propositions.Add(prop22);
            quest66.Propositions.Add(prop23);
            quest44.Propositions.Add(prop24);
            quest44.Propositions.Add(prop25);

            var quiz1 = new Quizz {
                Course = prbd,
                Title = "Premier quiz",
                ExaminationStartDate = new DateTime(2019, 1, 1)
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




            var QuestionCateg1 = new QuestionCateg(quest1, cat1);
            var QuestionCateg2 = new QuestionCateg(quest1, cat4);
            var QuestionCateg3 = new QuestionCateg(quest11, cat2);
            var QuestionCateg4 = new QuestionCateg(quest11, cat3);
            var QuestionCateg5 = new QuestionCateg(quest665, cat5);
            var QuestionCateg6 = new QuestionCateg(quest665, cat4);
            var QuestionCateg7 = new QuestionCateg(quest665, cat1);

            var QuestionCateg8 = new QuestionCateg(quest2, cat3);
            var QuestionCateg9 = new QuestionCateg(quest2, cat4);
            var QuestionCateg10 = new QuestionCateg(quest22, cat5);
            var QuestionCateg11 = new QuestionCateg(quest222, cat1);
            var QuestionCateg12 = new QuestionCateg(quest222, cat3);
            var QuestionCateg13 = new QuestionCateg(quest3, cat2);

            
            var QuestionCateg17 = new QuestionCateg(quest333, cat4);
            var QuestionCateg18 = new QuestionCateg(quest333, cat1);
            var QuestionCateg19 = new QuestionCateg(quest333, cat5);
            var QuestionCateg14 = new QuestionCateg(quest55, cat1);
            var QuestionCateg15 = new QuestionCateg(quest55, cat2);
            var QuestionCateg16 = new QuestionCateg(quest55, cat5);

            var QuestionCateg20 = new QuestionCateg(quest4, cat3);
            var QuestionCateg21 = new QuestionCateg(quest4, cat4);

            QuestionCategs.AddRange(QuestionCateg1, QuestionCateg2, QuestionCateg3, QuestionCateg4, QuestionCateg5,
                QuestionCateg6, QuestionCateg7, QuestionCateg8, QuestionCateg9, QuestionCateg10, QuestionCateg11,
                QuestionCateg12, QuestionCateg13, QuestionCateg14, QuestionCateg15, QuestionCateg16, QuestionCateg17,
                QuestionCateg18, QuestionCateg19, QuestionCateg20, QuestionCateg21);
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
        public DbSet<QuestionCateg> QuestionCategs
        {
            get; set;
        }
    }
}
