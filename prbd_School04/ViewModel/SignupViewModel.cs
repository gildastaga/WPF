using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using PRBD_Framework;
using School04.Model;
using School04.View;

namespace School04.ViewModel {
    public class SignupViewModel : ViewModelCommon {

        private string firstName;
        public string FirstName { get => firstName; set => SetProperty(ref firstName, value, () => Validate()); }

        private string lastName;
        public string LastName { get => lastName; set => SetProperty(ref lastName, value, () => Validate()); }

        private string mail;
        public string Mail { get => mail; set => SetProperty(ref mail, value, () => Validate()); }

        private string password;
        public string Password { get => password; set => SetProperty(ref password, value, () => Validate()); }

        private string passwordConfirm;
        public string PasswordConfirm { get => passwordConfirm; set => SetProperty(ref passwordConfirm, value, () => Validate()); }

        public event Action OnSignupSuccess;

        public ICommand SignupCommand { get; set; }
        public ICommand LogIn { get; set; }

        public override bool Validate() {
            ClearErrors();
            var member = User.GetByMail(Mail);
            if (string.IsNullOrEmpty(Mail)) {
                AddError(nameof(Mail), "Required");
            } else {
                if (Mail.Length < 3) {
                    AddError(nameof(Mail), "Mail must be at least 3 chars");
                } else {
                    if (!new EmailAddressAttribute().IsValid(Mail)) {
                        AddError(nameof(Mail), "Mail format incorrect");
                    } else {
                        if (member != null) {
                            AddError(nameof(Mail), "This mail is already used");
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(FirstName)) {
                AddError(nameof(FirstName), "Required");
            } else {
                if (FirstName.Length < 3) {
                    AddError(nameof(FirstName), "Firstname must be at least 3 chars");
                }
                /*if (member != null) {
                    if (member.Password != password) {
                        AddError(nameof(Password), "The password is incorrect");
                    }
                }*/
            }
            if (string.IsNullOrEmpty(LastName)) {
                AddError(nameof(LastName), "Required");
            } else {
                if (LastName.Length < 3) {
                    AddError(nameof(LastName), "Lastname must be at least 3 chars");
                }
                /*if (member != null) {
                    if (member.Password != password) {
                        AddError(nameof(Password), "The password is incorrect");
                    }
                }*/
            }
            if (string.IsNullOrEmpty(Password)) {
                AddError(nameof(Password), "Required");
            } else {
                if (Password.Length < 8) {
                    AddError(nameof(Password), "Passord must be at least 8 chars");
                } else {
                    Regex regex = new Regex("[A-Z]+");
                    Regex regex2 = new Regex("[0-9]+");
                    Regex regex3 = new Regex("[a-z]+");
                    if (!(regex.IsMatch(Password) && regex2.IsMatch(Password) && regex3.IsMatch(Password))) {
                        AddError(nameof(Password), "The password must contain at least one lowercase letter,\n one uppercase letter and one number");
                    } else if (string.IsNullOrEmpty(PasswordConfirm)) {
                        AddError(nameof(PasswordConfirm), "Required");
                    } else if (Password != PasswordConfirm) {
                        AddError(nameof(PasswordConfirm), "The passwords doesn't match");
                    }
                }
            }


            RaiseErrors();
            return !HasErrors;
        }

        public SignupViewModel() {
            SignupCommand = new RelayCommand(
                SignupAction,
                () => { return firstName != null && lastName != null && mail != null && password != null && passwordConfirm != null && !HasErrors; }
            );
            LogIn = new RelayCommand(LoginAction); ;
        }

        private void LoginAction() {
            App.NavigateTo<LoginView>();
        }

        private void SignupAction() {
            if (Validate()) {
                var newStudent = new Student(LastName, FirstName, Mail, Password);
                Context.Students.Add(newStudent);
                Context.SaveChanges();
                Login(newStudent);
                OnSignupSuccess?.Invoke();
                NotifyColleagues(AppMessages.NEW_STUDENT_ADDED);
            }
        }

        protected override void OnRefreshData() {
            throw new NotImplementedException();
        }
    }
}
