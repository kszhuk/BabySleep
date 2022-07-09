using Android.Gms.Extensions;
using BabySleep.Common.Exceptions.Authentication;
using BabySleep.Droid.Services;
using BabySleep.Services;
using Firebase.Auth;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthenticationService))]
namespace BabySleep.Droid.Services
{
    public class FirebaseAuthenticationService : IFirebaseAuthenticationService
    {
        public async Task<bool> CreateUser(string email, string password)
        {
            try
            {
                var authResult = await FirebaseAuth.Instance
                    .CreateUserWithEmailAndPasswordAsync(email, password);

                return await Task.FromResult(true);
            }
            catch (FirebaseAuthUserCollisionException)
            {
                throw new AuthUserCollisionException();
            }
            catch (FirebaseAuthException ex)
            {
                throw ex;
            }
        }

        public bool IsSignIn()
            => FirebaseAuth.Instance.CurrentUser != null;

        public async Task ResetPassword(string email)
        {
            try
            {
                await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email);
            }
            catch (FirebaseAuthInvalidUserException)
            {
                throw new AuthInvalidUserException();
            }
            catch (FirebaseAuthException ex)
            {
                throw ex;
            }
        }

        public async Task<string> SignIn(string email, string password)
        {
            try
            {
                var authResult = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await authResult.User.GetIdToken(false).AsAsync<GetTokenResult>();
                return token.Token;
            }
            catch(FirebaseAuthInvalidUserException)
            {
                throw new AuthInvalidUserException();
            }
            catch(FirebaseAuthException ex)
            {
                throw ex;
            }
        }

        public void SignOut()
            => FirebaseAuth.Instance.SignOut();
    }
}