using System;
using Msn.Model;
using PRBD_Framework;

namespace Msn.ViewModel {
    public class MainViewModel : ViewModelCommon {
        public event Action<Member, bool> DisplayMember;
        public event Action<Member, string> RenameTab;
        public event Action<Member> CloseTab;

        public MainViewModel() {
            Register(this, AppMessages.MSG_NEW_MEMBER, () => {
                // crée une nouvelle instance pour un nouveau membre "vide"
                var member = new Member("", "");
                // demande à la vue de créer dynamiquement un nouvel onglet avec le titre "<new member>"
                DisplayMember?.Invoke(member, true);
            });

            Register<Member>(this, AppMessages.MSG_DISPLAY_MEMBER, member => {
                DisplayMember?.Invoke(member, false);
            });

            Register<Member>(this, AppMessages.MSG_PSEUDO_CHANGED, member => {
                RenameTab?.Invoke(member, member.Pseudo);
            });

            Register<Member>(this, AppMessages.MSG_CLOSE_TAB, member => {
                CloseTab?.Invoke(member);
            });
        }

        protected override void OnRefreshData() {
            // pour plus tard
        }
    }
}
