using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Msn.Model;
using Msn.Properties;
using PRBD_Framework;

namespace Msn {
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

            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            Context.SeedData();
        }

        protected override void OnRefreshData() {
            // pour plus tard
        }
    }
}
