using Microsoft.Win32;
using Msn.Model;
using PRBD_Framework;

namespace Msn.View {
    public partial class MemberDetailView : UserControlBase {
        public MemberDetailView(Member member, bool isNew) {
            InitializeComponent();
            vm.Init(member, isNew);
        }

        private string Vm_OnLoadImage() {
            var fd = new OpenFileDialog();
            if (fd.ShowDialog() == true) {
                return fd.FileName;
            }
            return null;
        }
    }
}