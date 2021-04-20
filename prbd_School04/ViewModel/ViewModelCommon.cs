using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    public abstract class ViewModelCommon : ViewModelBase<ModelSchool04> {
        public ViewModelCommon() {
        }

        public static bool IsTeacher {
            get => App.IsLoggedIn && App.CurrentUser.IsTeacher();
        }

        public static bool IsStudent {
            get => !IsTeacher;
        }

        public static User CurrentUser {
            get => App.CurrentUser;
        }

        public static void Login(User user) {
            App.Login(user);
        }

        public static void Logout() {
            App.Logout();
        }

        protected override void OnRefreshData() {
            throw new NotImplementedException();
        }

        public static bool IsLoggedIn { get => App.IsLoggedIn; }
    }
}
