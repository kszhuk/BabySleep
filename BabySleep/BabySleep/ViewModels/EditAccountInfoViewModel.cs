using BabySleep.Common.Exceptions.Authentication;
using BabySleep.Resx;
using BabySleep.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Edit Account Info page
    /// </summary>
    public class EditAccountInfoViewModel : INotifyPropertyChanged
    {
        public EditAccountInfoViewModel()
        {
            authService = DependencyService.Resolve<IFirebaseAuthenticationService>();

            if (authService.IsSignIn())
            {
                Email = authService.GetEmail();
            }
            else
            {
                IsEnabled = false;
                ((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.Login, LoginResources.UserNotLogin);
            }

            ChangePasswordCommand = new Command(ChangePassword);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        private readonly IFirebaseAuthenticationService authService;

        string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }

        bool isEnabled = true;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
            }
        }
        #endregion

        #region Commands
        public Command ChangePasswordCommand { get; }
        #endregion

        #region Private Methods
        private async void ChangePassword()
        {
            var result = await ((App)Xamarin.Forms.Application.Current).ShowQuestion(LoginResources.Login, LoginResources.ResetPasswordQuestion);
            if (!result)
            {
                return;
            }

            try
            {
                await authService.ResetPassword(Email);
                await ((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.Login, LoginResources.PasswordRecoverySent);
            }
            catch (AuthInvalidUserException)
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.Login, LoginResources.ResetPasswordInvalidEmail);
            }
            catch
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(LoginResources.Login, LoginResources.ResetPasswordException);
            }
        }
        #endregion
    }
}
