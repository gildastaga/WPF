using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PRBD_Framework;
using System.Collections.Generic;

namespace prbd_msn_tuto {
    public class MembersViewModel : ViewModelBase<Model> {

        private ObservableCollection<Member> members;
        public ObservableCollection<Member> Members {
            get => members;
            set => SetProperty<ObservableCollection<Member>>(ref members, value);
        }

        private string filter;
        public string Filter {
            get => filter;
            set => SetProperty<string>(ref filter, value, ApplyFilterAction);
        }

        public ICommand ClearFilter { get; set; }
        public ICommand NewMember { get; set; }
        public ICommand DisplayMemberDetails { get; set; }

        public MembersViewModel() : base() {
            Members = new ObservableCollection<Member>(App.Context.Members);

            ClearFilter = new RelayCommand(() => Filter = "");
            
            NewMember = new RelayCommand(() => { NotifyColleagues(AppMessages.MSG_NEW_MEMBER); });

            Register<Member>(this, AppMessages.MSG_MEMBER_CHANGED, member => {
                ApplyFilterAction();
            });

            DisplayMemberDetails = new RelayCommand<Member>(member => {
                NotifyColleagues(AppMessages.MSG_DISPLAY_MEMBER, member);
            });
        }

        private void ApplyFilterAction() {
            IEnumerable<Member> query = Context.Members;
            if (!string.IsNullOrEmpty(Filter))
                query = from m in Context.Members
                        where m.Pseudo.Contains(Filter) || m.Profile.Contains(Filter)
                        select m;
            Members = new ObservableCollection<Member>(query);
        }

        protected override void OnRefreshData() {
            // pour plus tard
        }
    }
}