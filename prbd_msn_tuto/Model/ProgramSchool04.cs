using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    class ProgramSchool04 {
        public static ModelSchool04 Context { get; private set; } = new ModelSchool04();
        public static void SeedData() {
            var context = ProgramSchool04.Context;

            Context.Database.BeginTransaction();

            var Katia = new Student("Mijares", "Katia", "abc@def", "abcdef");
            var Corentin = new Student("Heinix", "Corentin", "ghj@def", "abcdef");
            var Benoit = new Teacher("Penelle", "Benoit", "benoit@penelle", "Penelle1");
            var Boris = new Teacher("Verhagen", "Boris", "boris@verhagen", "Boris1");
            Context.Students.AddRange(Katia, Corentin);
            Context.Teachers.AddRange(Benoit,Boris);
            var prwb = new Course("prwb", Benoit);
            var web = new Course("WEB", Benoit);
            var SGBD = new Course("SGBD", Boris);
            Context.Courses.AddRange( prwb, web, SGBD);
            Benoit.CourseGiven.Add(SGBD);
            Benoit.CourseGiven.Add(web);
            Boris.CourseGiven.Add(prwb);
            var enreg1 = new Registration(Corentin, prwb);
            var enreg2 = new Registration(Katia, web);
            Context.Registrations.AddRange(enreg1, enreg2);
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
            Context.Questions.AddRange(quest1, quest2);
            
            Context.SaveChanges();

            Context.Database.CommitTransaction();
        }
    }
}
