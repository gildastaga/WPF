using System;
using System.Linq;
using System.Windows;
using PRBD_Framework;

namespace prbd_msn_tuto {
    public partial class App : ApplicationBase {
        public static Model Context { get => Context<Model>(); }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            Context.SeedData();

            // affichage du nombre d'instances de l'entité 'Member'
            Console.WriteLine(Context.Members.Count());

            // affichage du pseudo de tous les membres
            foreach (var m in Context.Members) {
                Console.WriteLine(m.Pseudo);
            }
        }

        protected override void OnRefreshData() {
            // pour plus tard
        }
    }
}
