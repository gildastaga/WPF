using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.Model {
    public abstract class User : EntityBase<ModelSchool04> {
        public int UserId {
            get; set;
        }
        public string Name {
            get; set;
        }
        public string FirstName {
            get; set;
        }
        public string Mail {
            get; set;
        }
        public string Password {
            get; set;
        }
        public string Profile {
            get; set;
        }
        public string Discriminator { get; }
        public User() {
        }
        public User(string name, string firstName, string mail, string password, string profile = null ) {
            Name = name;
            FirstName = firstName;
            Profile = profile; 
            Mail = mail;
            Password = password;
        }
        public override bool Validate() {
            ClearErrors();
            if (Name == null)
                AddError(nameof(Name), "required");
            if (FirstName == null)
                AddError(nameof(FirstName), "required");
            return !HasErrors;
        }
        public override string ToString() {
            return FirstName + " " + Name;
        }

        public bool IsTeacher() {
            return Discriminator == "Teacher";
        }
        public bool IsStudent() {
            return Discriminator == "Student";
        }
        public static User GetByMail(string mail) {
            return Context.Users.SingleOrDefault(m => m.Mail == mail);
        }
        public static User GetByName( string name ) {
            return Context.Users.SingleOrDefault(m => m.Name == name);
        }
        public static User GetByFirstName( string firstname ) {
            return Context.Users.SingleOrDefault(m => m.FirstName == firstname);
        }
        public static User GetByUserId( int id ) {
            return Context.Users.SingleOrDefault(m => m.UserId == id);
        }

        public string FullName => this.ToString();
    }
}
