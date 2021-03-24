using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msn.Model {
    class ModelSchool04 : DbContext {
        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=school04");
        }

        public void SeedData() {
            Database.BeginTransaction();
            var Katia = new Student() { firstName = "Katia", name = "Mijares", mail = "c.mijareskatia@outlook.com", Password = "Password1," };
            Students.Add(Katia);
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
    }
}
