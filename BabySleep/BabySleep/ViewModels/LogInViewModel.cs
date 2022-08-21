using BabySleep.Common.Exceptions.Authentication;
using BabySleep.Common.Helpers;
using BabySleep.Resources.Resx;
using BabySleep.Services;
using BabySleep.Validations;
using BabySleep.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Log in page
    /// </summary>
    public class LogInViewModel : INotifyPropertyChanged
    {
        public LogInViewModel()
        {
            authService = DependencyService.Resolve<IFirebaseAuthenticationService>();

            LoginCommand = new Command(Login);
            ResetPasswordCommand = new Command(ResetPassword);
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
        #endregion

        #region Commands
        public Command LoginCommand { get; }
        public Command ResetPasswordCommand { get; }
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
            }
        }

        private bool AreFieldsValid(bool validatePassword = true)
        {
            bool isEmailValid = Email.Validate();
            bool isPasswordValid = validatePassword ? Password.Validate() : true;

            if(!validatePassword)
            {
                Password.IsValid = true;
            }

            return isEmailValid && isPasswordValid;
        }

        private async void Login()
        {
            if (!AreFieldsValid())
            {
                return;
            }

            try
            {
                await authService.SignIn(Email.Value, Password.Value);
                App.NavigateFromMenu(new MainPage());
            }
            catch
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.Login, LoginResources.LoginFailed);
            }
        }

        private async void ResetPassword()
        {
            if (!AreFieldsValid(false))
            {
                return;
            }

            var result = await ((App)Xamarin.Forms.Application.Current).ShowQuestion(LoginResources.Login, LoginResources.ResetPasswordQuestion);
            if(!result)
            {
                return;
            }

            try
            {
                await authService.ResetPassword(Email.Value);
                await ((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.Login, LoginResources.PasswordRecoverySent);
            }
            catch(AuthInvalidUserException)
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.Login, LoginResources.ResetPasswordInvalidEmail);
            }
            catch
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.Login, LoginResources.ResetPasswordException);
            }
        }

        private void SignUp()
        {
            App.NavigateFromMenu(new SignUpPage());
        }
        #endregion
    }
}
