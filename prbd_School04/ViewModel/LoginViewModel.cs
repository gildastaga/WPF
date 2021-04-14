using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PRBD_Framework;
using School04.Model;

namespace School04.ViewModel {
    public class LoginViewModel : ViewModelCommon {

        private string mail;
        public string Mail { get => mail; set => SetProperty(ref mail, value, () => Validate()); }

        private string password;
        public string Password { get => password; set => SetProperty(ref password, value, () => Validate()); }

        public event Action OnLoginSuccess;

        public ICommand LoginCommand { get; set; }

        public override bool Validate() {
            ClearErrors();
            var member = User.GetByMail(Mail);
            if (string.IsNullOrEmpty(Mail)) {
                AddError(nameof(Mail), "Required");
            } else {
                if (Mail.Length < 3) {
                    AddError(nameof(Mail), "Mail must be at least 3 chars");
                } else {
                    if (member == null) {
                        AddError(nameof(Mail), "This mail don't exist");
                    } else {
                        if(member.Password != password) {
                            AddError(nameof(Password), "The password is incorrect");
                        }
                    }
                }
            }
            RaiseErrors();
            return !HasErrors;
        }

        public LoginViewModel() {
            LoginCommand = new RelayCommand(
                LoginAction,
                () => { return mail != null && password != null && !HasErrors; }
            );
        }

        private void LoginAction() {
            if (Validate()) {
                var user = User.GetByMail(mail);
                Login(user);
                OnLoginSuccess?.Invoke();
            }
        }

        protected override void OnRefreshData() {
            throw new NotImplementedException();
        }
    }
}
