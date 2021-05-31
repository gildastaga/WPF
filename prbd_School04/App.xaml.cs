using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using School04.Model;
using School04.Properties;
using PRBD_Framework;

namespace School04 {
    public partial class App : ApplicationBase {
        public static ModelSchool04 Context { get => Context<ModelSchool04>(); }
        public static object ModelSchool04 { get; internal set; }
        public static User CurrentUser { get; private set; }
        public static void Login( User user ) {
            CurrentUser = user;
        }
        public static void Logout() {
            CurrentUser = null;
        }
        public static bool IsLoggedIn { get => CurrentUser != null; }
        public App() {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Culture);
        }
        //surcharge la classe 
        protected override void OnStartup( StartupEventArgs e ) {
            base.OnStartup(e);
            // Définit l'intervalle de temps (en secondes) pour le rafraîchissement des données
            RefreshDelay = Settings.Default.RefreshDelay;

            Console.WriteLine("Chargement de BDD en cours");

            Context.Database.EnsureDeleted();
           Context.Database.EnsureCreated();
           Context.SeedData();
            /*Console.WriteLine("Liste de tous les teachers");
            foreach (var t in Context.Teachers.ToList()) {
                Console.Write($"{t.FirstName} {t.Name}");
                foreach (var course in t.CourseGiven) {
                    Console.Write($"{ course.Title} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Liste de tous les étiants avec les cours auxquels ils sont enregistés");
            foreach (var t in Context.Students.ToList()) {
                Console.Write($"{t.FirstName} {t.Name}");
                foreach (var registration in t.CoursesStudent) {
                    Console.Write($"{ registration.Course.Title} ");
                }
                Console.WriteLine();
            }

            foreach (var t in Context.Students.ToList()) {
                foreach (var regi in t.CoursesStudent.ToList()) {
                    Console.WriteLine(regi.Course.Title);
                }

            }

            Console.WriteLine("Chargement de BDD finie");*/
        }
        protected override void OnRefreshData() {
            if (CurrentUser?.UserId != null)
                CurrentUser = User.GetByUserId(CurrentUser.UserId);
        }
    }
}
