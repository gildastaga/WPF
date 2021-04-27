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

        public Question Question { get; set; }

        private ObservableCollection<CheckProposition> propositions;  // toute liste qu'on doit affiché ds la vue doit etre observable et donc on doit pouvoir remplir cette liste avec la methode LoadTagsChecked()
        public ObservableCollection<CheckProposition> Propositions {
            get => propositions;
            set {
                propositions = value;
                RaisePropertyChanged(nameof(Propositions));
            }
        }

        public QuestionViewModel(Question question) : base() {
            Question = question;
            LoadPropositionsChecked();
        }

        private void LoadPropositionsChecked() {
            Propositions = new ObservableCollection<CheckProposition>();   // Pour le moment ma liste est vide,
            foreach (var proposition in App.Context.Propositions)               
            {
                var p = new CheckProposition()                   
                {
                    Name = proposition.Body,                     
                    Checked = Question.Propositions.Contains(proposition)
                };
                Propositions.Add(p);                              
            }
        }
    }
}
