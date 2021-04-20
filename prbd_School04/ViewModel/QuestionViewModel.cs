using PRBD_Framework;
using School04.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School04.ViewModel {
    public class QuestionViewModel : ViewModelBase<ModelSchool04> {
        protected override void OnRefreshData() {
            throw new NotImplementedException();
        }

        private ObservableCollection<CheckProposition> propositions;  // toute liste qu'on doit affiché ds la vue doit etre observable et donc on doit pouvoir remplir cette liste avec la methode LoadTagsChecked()
        public ObservableCollection<CheckProposition> Propositions {
            get => propositions;
            set {
                propositions = value;
                RaisePropertyChanged(nameof(Propositions));
            }
        }

        public QuestionViewModel() : base() {


        }

        /*private void LoadPropositionsChecked() {
            Propositions = new ObservableCollection<CheckProposition>();   // Pour le moment ma liste est vide,
            foreach (var proposition in App.Context.Propositions)               //on parcours la DbSet de la BD et pour chaque TagName, je creer une new instance sur lui puis je me dde si ma liste de tag contient dejà ce tag; ma méthode contains renvoit true ou false
            {
                var p = new CheckProposition()                    // on creer une new instance de TagChecked (cette new classe me permet de savoir si un tag a été coché ou pas d'où cette new classe avec un TagName et un boolean pour savoir si oui ou non le tag a été coché ou pas
                {
                    Name = proposition.Body,                     // je prends le TagName sur lequel je suis
                    Checked = Question.Context.Propositions.Contains(proposition)
                };
                Propositions.Add(p);                              // Enfin j'ajoute ce tag ds la liste.
            }
        }*/
    }
}
