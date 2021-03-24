using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04 {
    class Program {
        static void Main( string[] args ) {
            using var model = new Model();
            model.Database.EnsureCreated();

            var Katia = new Student() { firstName = "Katia", name = "Mijares", mail = "c.mijareskatia@outlook.com", Password = "Password1," };
            model.Students.Add(Katia);
            model.SaveChanges();

        }
    }
}
