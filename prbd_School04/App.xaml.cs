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
            Console.WriteLine("Liste de tous les teachers");
            foreach (var t in Context.Teachers.ToList()) {
                Console.Write($"{t.FirstName} {t.Name}");
                foreach (var course in t.CourseGiven) {
                    Console.Write($"{ course.Title} ");
                }
                Console.WriteLine();

            }

            Console.WriteLine("Chargement de BDD finie");
        }
        /*private void TestMessage() {
            //instancie le service de message
            var message = new Messenger();

            //enregistre deux handles différents pour le même messages "coucou"
            message.Register(message.COUCOU, () => {
                Console.WriteLine("coucou received by handler 1");
            });
            message.Register(AppMessages.COUCOU, () => {
                Console.WriteLine("coucou received by handler 2");
            });

            //enregistre un hndler pour le message "brol" qui prend en paramète un entier
            message.Register<int>(AppMessages.BROL, (i) => {
                Console.WriteLine("brol received with parameter " + i );
            });

            //notifie les "observers" (ici appelés les collègues") 
            //enregistrés avec le message "coucou" puis "brol"
            message.NotifyColleagues(AppMessages.COUCOU);
            message.NotifyColleagues(AppMessages.BROL, 123);
        }*/

        protected override void OnRefreshData() {
            if (CurrentUser?.Mail != null)
                CurrentUser = User.GetByMail(CurrentUser.Mail);
        }
    }
}
