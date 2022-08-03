using Amazon.Lambda;
using Autofac;
using BabySleep.Infrastructure.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Sync page
    /// </summary>
    public class SyncViewModel : INotifyPropertyChanged
    {
        public SyncViewModel()
        {
            syncService = App.Container.Resolve<ISyncAWSService>();
            SyncCommand = new Command(Sync);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        private readonly ISyncAWSService syncService;
        #endregion

        #region Commands
        public Command SyncCommand { get; }
        #endregion

        #region Private Methods
        private async void Sync()
        {
            syncService.Sync();
        }
        #endregion
    }
}
