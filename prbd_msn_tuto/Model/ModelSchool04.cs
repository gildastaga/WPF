using Microsoft.EntityFrameworkCore;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msn.Model {
    public class ModelSchool04 : DbContextBase {
        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=school04");
        }

        public void SeedData() {
            Database.EnsureCreated();
            var Katia = new Student() { firstName = "Katia", name = "Mijares", mail = "c.mijareskatia@outlook.com", Password = "Password1," };
            Students.Add(Katia);
            SaveChanges();

            //Database.CommitTransaction();
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

        public DbSet<Course> Answer {
            get; set;
        }
        public DbSet<Course> Category {
            get; set;
        }
        public DbSet<Course> Proposition {
            get; set;
        }
        public DbSet<Course> Question {
            get; set;
        }
        public DbSet<Course> QuestionQuizz {
            get; set;
        }
        public DbSet<Course> Registration {
            get; set;
        }
    }
}
