using System.Windows.Controls;
using PRBD_Framework;

namespace prbd_msn_tuto {
    public partial class MainView : WindowBase {
        public MainView() {
            InitializeComponent();
        }

        private void Vm_DisplayMember(Member member, bool isNew) {
            if (member != null) {
                var tab = tabControl.FindByTag(member.Pseudo);
                if (tab == null)
                    tabControl.Add(
                        new MemberDetailView(member, isNew),
                        isNew ? "<new member>" : member.Pseudo, member.Pseudo
                    );
                else
                    tabControl.SetFocus(tab);
            }
        }

        private void Vm_RenameTab(Member member, string header) {
            var tab = tabControl.SelectedItem as TabItem;
            if (tab != null) {
                tab.Header = tab.Tag = header = string.IsNullOrEmpty(header) ? "<new member>" : header;
            }
        }

        private void Vm_CloseTab(Member member) {
            var tab = tabControl.FindByTag(member.Pseudo);
            tabControl.Items.Remove(tab);
        }
    }
}