using BabySleep.Common.Exceptions.Authentication;
using BabySleep.Resources.Resx;
using BabySleep.Services;
using BabySleep.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Page for regestering new users
    /// </summary>
    public class SignUpViewModel : INotifyPropertyChanged
    {
        public SignUpViewModel()
        {
            authService = DependencyService.Resolve<IFirebaseAuthenticationService>();

            SignUpCommand = new Command(SignUp);

            AddValidations();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        private readonly IFirebaseAuthenticationService authService;

        ValidatableObject<string> email = new ValidatableObject<string>();
        public ValidatableObject<string> Email
        {
            get => email;
            set
            {
                email = value;
                AddValidations();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }

        ValidatableObject<string> password = new ValidatableObject<string>();
        public ValidatableObject<string> Password
        {
            get => password;
            set
            {
                password = value;
                AddValidations();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        ValidatableObject<string> passwordConfirm = new ValidatableObject<string>();
        public ValidatableObject<string> PasswordConfirm
        {
            get => passwordConfirm;
            set
            {
                passwordConfirm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PasswordConfirm)));
            }
        }
        #endregion

        #region Commands
        public Command SignUpCommand { get; }
        #endregion

        #region Private Methods
        private void AddValidations()
        {
            if (!email.Validations.Any())
            {
                Email.Validations.Add(new EmailRule<string> { ValidationMessage = LoginResources.InvalidEmail });
            }

            if (!password.Validations.Any())
            {
                Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = LoginResources.EmptyPassword });
                Password.Validations.Add(new PasswordRule<string> { ValidationMessage = LoginResources.InvalidPassword });
            }
        }

        private bool AreFieldsValid()
        {
            bool isEmailValid = Email.Validate();
            bool isPasswordValid = Password.Validate();

            PasswordConfirm.IsValid = (PasswordConfirm.Value ?? "") == (Password.Value ?? "");

            return isEmailValid && isPasswordValid && PasswordConfirm.IsValid;
        }

        private async void SignUp()
        {
            if (!AreFieldsValid())
            {
                return;
            }           

            try
            {
                await authService.CreateUser(Email.Value, Password.Value);
                await((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.SignUp, LoginResources.SignUpSuccessful);
            }
            catch (AuthUserCollisionException)
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.SignUp, LoginResources.AuthUserCollisionException);
            }
            catch
            {
                await((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.SignUp, LoginResources.SignUpException);
            }
        }
        #endregion
    }
}
