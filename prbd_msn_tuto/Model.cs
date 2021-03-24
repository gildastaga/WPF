using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04 {
    public class Model : DbContext {
        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=school04");
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
