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

        /*public static Member CurrentUser { get; private set; }

        public static void Login(Member member) {
            CurrentUser = member;
        }

        public static void Logout() {
            CurrentUser = null;
        }

        public static bool IsLoggedIn { get => CurrentUser != null; }*/

        public App() {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Culture);
        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            Console.WriteLine("Chargement de BDD en cours");

            ProgramSchool04.Context.Database.EnsureDeleted();
            ProgramSchool04.Context.Database.EnsureCreated();
            ProgramSchool04.SeedData();
            Console.WriteLine("Liste de tous les teachers");
            foreach (var t in Context.Teachers) {
                Console.Write($"{t.FirstName} {t.Name}");
                foreach(var course in t.CourseGiven){
                    Console.Write($"{course.titleOfCourse} ");
                }
                Console.WriteLine();
                
            }

            Console.WriteLine("Chargement de BDD finie");
        }

        protected override void OnRefreshData() {
            // pour plus tard
        }
    }
}
