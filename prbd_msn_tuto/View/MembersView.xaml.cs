using PRBD_Framework;

namespace Msn.View {
    public partial class MembersView : UserControlBase {
        public MembersView() {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            vm.DisplayMemberDetails.Execute(listView.SelectedItem);
        }
    }
}
