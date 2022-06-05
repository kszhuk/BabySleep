using Android.App;
using Android.Content;
using BabySleep.Droid.Services;
using BabySleep.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertBuilderService))]
namespace BabySleep.Droid.Services
{
    public class AlertBuilderService : IAlertBuilderService
    {
        public Task ShowExceptionAsync(string title, string message, string positiveButton)
        {
            return ShowAlertAsync(title, message, positiveButton, string.Empty);
        }

        public Task<bool> ShowQuestionAsync(string title, string message, string positiveButton, string negativeButton)
        {
            return ShowAlertAsync(title, message, positiveButton, negativeButton);
        }

        private Task<bool> ShowAlertAsync(string title, string message, string positiveButton, string negativeButton)
        {
            var tcs = new TaskCompletionSource<bool>();

            using (var db = new AlertDialog.Builder(MainActivity.Instance, Resource.Style.AppCompatAlertDialogStyle))
            {
                db.SetTitle(title);
                db.SetMessage(message);
                db.SetPositiveButton(positiveButton, (sender, args) => { tcs.TrySetResult(true); });

                if (!string.IsNullOrEmpty(negativeButton))
                {
                    db.SetNegativeButton(negativeButton, (sender, args) => { tcs.TrySetResult(false); });
                }
                db.Show();
            }

            return tcs.Task;
        }
    }
}