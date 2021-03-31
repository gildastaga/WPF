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
        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuestionQuizz>()
                .HasKey(qq => new { qq.QuizzId, qq.QuestionId });

            modelBuilder.Entity<Registration>()
                .HasKey(r => new { r.CourseId, r.StudentId });

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.CourseGiven)
                .WithOne(c => c.TeacherCourse)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<QuestionQuizz>()
                .HasOne(qq => qq.Quizz)
                .WithMany(q => q.Questions)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Propositions)
                .WithOne(p => p.Question)
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        public void SeedData() {
            Database.BeginTransaction();

            var Katia = new Student("Mijares", "Katia", "abc@def", "abcdef");
            var Corentin = new Student("Heinix", "Corentin", "abc@def", "abcdef");
            var Benoit = new Teacher("Penelle", "Benoit", "benoit@penelle", "Penelle");
            Students.AddRange(Katia, Corentin);
            Teachers.AddRange(Benoit);
            var prwb = new Course {
                titleOfCourse = "PRBD",
                TeacherCourse = Benoit,
            };
            var web = new Course {
                titleOfCourse = "WEB",
                TeacherCourse = Benoit,
            };
            Courses.AddRange(prwb, web);
            var enreg1 = new Registration {
                Course = prwb,
                Student = Corentin
            };
            var enreg2 = new Registration {
                Course = web,
                Student = Katia
            };
            Registrations.AddRange(enreg1, enreg2);


            //Courses.RemoveRange(Courses
            //    .Include("StudentCourse"));
            Katia.CoursesStudent.Add(enreg2);
            Corentin.CoursesStudent.Add(enreg1);

            web.StudentsCourse.Add(enreg2);
            prwb.StudentsCourse.Add(enreg1);
            //var quest = new Question("Quelle est ta couleur pref ?", false, false);
            //var quest2 = new Question("Quelle est ta marque pref ?", false, false);

            //.Include("AnswerList"));
            /*Courses.RemoveRange(Courses
                .Include("CourseStrudent"));*/
            //.Include("QuizzCourse")
            //.Include("QuestionList"));


            //Benoit.CourseGiven.Add(prwb);
            //Benoit.CourseGiven.Add(web);
            //Katia.StudentCourse.Add(prwb);
            //Corentin.StudentCourse.Add(web);*/
            //prwb.CourseStrudent.Add(Katia);
            //web.CourseStrudent.Add(Corentin);
            //Questions.AddRange(quest, quest2);*/

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
