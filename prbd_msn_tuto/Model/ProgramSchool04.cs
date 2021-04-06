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
            var Corentin = new Student("Heinix", "Corentin", "abc@def", "abcdef");
            var Benoit = new Teacher("Penelle", "Benoit", "benoit@penelle", "Penelle1");
            var Boris = new Teacher("Verhagen", "Boris", "boris@verhagen", "Boris1");
            Context.Students.AddRange(Katia, Corentin);
            Context.Teachers.AddRange(Benoit);
            var prwb = new Course {
                titleOfCourse = "PRBD",
                TeacherCourse = Benoit,
            };
            var web = new Course {
                titleOfCourse = "WEB",
                TeacherCourse = Benoit,
            };
            var SGBD = new Course {
                titleOfCourse = "SGBD",
                TeacherCourse = Boris,
            };
            Context.Courses.AddRange(prwb, web, SGBD);
            var enreg1 = new Registration {
                Course = prwb,
                Student = Corentin
            };
            var enreg2 = new Registration {
                Course = web,
                Student = Katia
            };
            Context.Registrations.AddRange(enreg1, enreg2);
            Katia.CoursesStudent.Add(enreg2);
            Corentin.CoursesStudent.Add(enreg1);

            web.StudentsCourse.Add(enreg2);
            prwb.StudentsCourse.Add(enreg1);
            var quest1 = new Question("Quelle est ta couleur pref ?", false, false);
            var quest2 = new Question("Quelle est ta marque pref ?", false, false);
            //Benoit.CourseGiven.Add(prwb);
            //Benoit.CourseGiven.Add(web);
            //Katia.StudentCourse.Add(prwb);
            //Corentin.StudentCourse.Add(web);*/
            //prwb.CourseStrudent.Add(Katia);
            //web.CourseStrudent.Add(Corentin);
            //Questions.AddRange(quest, quest2);*/

            

            Context.SaveChanges();

            Context.Database.CommitTransaction();
        }
    }
}
