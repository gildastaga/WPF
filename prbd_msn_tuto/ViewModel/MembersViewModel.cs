using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PRBD_Framework;
using System.Collections.Generic;
using Msn.Model;

namespace Msn.ViewModel {
    public class MembersViewModel : ViewModelCommon {

        private ObservableCollection<Member> members;
        public ObservableCollection<Member> Members {
            get => members;
            set => SetProperty<ObservableCollection<Member>>(ref members, value);
        }

        private string filter;
        public string Filter {
            get => filter;
            set => SetProperty<string>(ref filter, value, OnRefreshData);
        }

        private bool followeesSelected;
        public bool FolloweesSelected {
            get => followeesSelected;
            set => SetProperty<bool>(ref followeesSelected, value, OnRefreshData);
        }

        private bool followersSelected;
        public bool FollowersSelected {
            get => followersSelected;
            set => SetProperty<bool>(ref followersSelected, value, OnRefreshData);
        }

        private bool allSelected;
        public bool AllSelected {
            get => allSelected;
            set => SetProperty<bool>(ref allSelected, value, OnRefreshData);
        }


        public ICommand ClearFilter { get; set; }
        public ICommand NewMember { get; set; }
        public ICommand DisplayMemberDetails { get; set; }

        public MembersViewModel() : base() {
            Members = new ObservableCollection<Member>(App.Context.Members);

            ClearFilter = new RelayCommand(() => Filter = "");
            
            NewMember = new RelayCommand(() => { NotifyColleagues(AppMessages.MSG_NEW_MEMBER); });

            Register<Member>(this, AppMessages.MSG_MEMBER_CHANGED, member => {
                OnRefreshData();
            });

            DisplayMemberDetails = new RelayCommand<Member>(member => {
                NotifyColleagues(AppMessages.MSG_DISPLAY_MEMBER, member);
            });
            AllSelected = true;
        }

        protected override void OnRefreshData() {
            /* OnRefreshData est appelée deux fois quand on clique sur un radiobutton : une fois
             * pour mettre celui qui est sélectionné à true et une autre fois pour mettre celui qui
             * était sélectionné à false. Du coup, pour éviter de faire deux fois la requête, on retourne
             * sans rien faire quand deux flags sont vrais en même temps.
             */
            if (followeesSelected && followersSelected || 
                followeesSelected && allSelected || 
                followersSelected && allSelected)
                return;
            IQueryable<Member> members = string.IsNullOrEmpty(Filter) ? Member.GetAll() : Member.GetFiltered(Filter);
            var filteredMembers = from m in members
                where (
                    // on veut les followees de l'utilisateur courant => on prend tous ceux qui ont 
                    // le pseudo courant dans leurs followers 
                    FolloweesSelected && m.Followers.Any(f => CurrentUser != null && f.Pseudo == CurrentUser.Pseudo) ||
                    // on veut les followers de l'utilisateur courant => on prend tous ceux qui ont 
                    // le pseudo courant dans leurs followees 
                    FollowersSelected && m.Followees.Any(f => CurrentUser != null && f.Pseudo == CurrentUser.Pseudo) ||
                    // on veut tous les membres
                    AllSelected)
                select m;
            Members = new ObservableCollection<Member>(filteredMembers);
        }
    }
}
