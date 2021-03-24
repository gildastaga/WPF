using System;
using System.Windows.Input;
using Msn.Model;
using Msn.Properties;
using PRBD_Framework;

namespace Msn.ViewModel {
    public class LoginViewModel : ViewModelCommon {
        public event Action OnLoginSuccess;

        public ICommand LoginCommand { get; set; }

        private string pseudo;
        public string Pseudo { get => pseudo; set => SetProperty<string>(ref pseudo, value, () => Validate()); }

        private string password;
        public string Password { get => password; set => SetProperty<string>(ref password, value, () => Validate()); }

        public LoginViewModel() : base() {
            LoginCommand = new RelayCommand(
                LoginAction,
                () => { return pseudo != null && password != null && !HasErrors; }
            );
        }

        private void LoginAction() {
            if (Validate()) {
                var member = Context.Members.Find(Pseudo);
                Login(member);
                OnLoginSuccess?.Invoke();
            }
        }

        public override bool Validate() {
            ClearErrors();

            var member = Context.Members.Find(Pseudo);

            if (string.IsNullOrEmpty(Pseudo))
                AddError(nameof(Pseudo), Resources.Error_Required);
            else if (Pseudo.Length < 3)
                AddError(nameof(Pseudo), Resources.Error_LengthGreaterEqual3);
            else if (member == null)
                AddError(nameof(Pseudo), Resources.Error_DoesNotExist);
            else {
                if (string.IsNullOrEmpty(Password))
                    AddError(nameof(Password), Resources.Error_Required);
                else if (Password.Length < 3)
                    AddError(nameof(Password), Resources.Error_LengthGreaterEqual3);
                else if (member != null && member.Password != Password)
                    AddError(nameof(Password), Resources.Error_WrongPassword);
            }

            RaiseErrors();
            return !HasErrors;
        }

        protected override void OnRefreshData() {
        }
    }
}