using BabySleep.Interfaces;
using BabySleep.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace BabySleep.Views
{
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        public event EventHandler UpdateIcons;
        Page currentPage;

        public MainPage()
        {
            InitializeComponent();
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            this.SelectedTabColor = Color.FromHex("#007AFF");
            this.UnselectedTabColor = Color.Black;

            currentPage = Children[0];
            this.CurrentPageChanged += Handle_CurrentPageChanged;
        }

        private void Handle_CurrentPageChanged(object sender, EventArgs e)
        {
            var currentBinding = currentPage.BindingContext as ITabPage;
            if (currentBinding != null)
                currentBinding.IsSelected = false;

            currentPage = CurrentPage;
            currentBinding = currentPage.BindingContext as ITabPage;
            if (currentBinding != null)
                currentBinding.IsSelected = true;

            UpdateIcons?.Invoke(this, EventArgs.Empty);
        }
    }
}
