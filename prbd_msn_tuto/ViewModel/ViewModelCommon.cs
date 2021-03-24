using PRBD_Framework;
using Msn.Model;

namespace Msn.ViewModel {
    public abstract class ViewModelCommon : ViewModelBase<MsnContext> {

        public ViewModelCommon() : base() {
        }

        public static bool IsAdmin {
            get => App.IsLoggedIn && App.CurrentUser.IsAdmin;
        }

        public static bool IsNotAdmin {
            get => !IsAdmin;
        }

        public static Member CurrentUser {
            get => App.CurrentUser;
        }

        public static void Login(Member member) {
            App.Login(member);
        }

        public static void Logout() {
            App.Logout();
        }

        public static bool IsLoggedIn { get => App.IsLoggedIn; }
    }
}