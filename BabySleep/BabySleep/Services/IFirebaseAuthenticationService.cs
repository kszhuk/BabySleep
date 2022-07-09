using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BabySleep.Services
{
    public interface IFirebaseAuthenticationService
    {
        Task<bool> CreateUser(string email, string password);
        Task<string> SignIn(string email, string password);
        bool IsSignIn();
        void SignOut();
        Task ResetPassword(string email);
    }
}
