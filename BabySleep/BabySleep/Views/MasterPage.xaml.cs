using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BabySleep.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : FlyoutPage
    {
        public MasterPage()
        {
            InitializeComponent();
            this.Flyout = new MenuPage();
            this.Detail = new NavigationPage(new MainPage());
            App.MasterDetail = this;
        }

        public MasterPage(Page detail)
        {
            InitializeComponent();
            this.Flyout = new MenuPage();
            this.Detail = new NavigationPage(detail);
            App.MasterDetail = this;
        }
    }
}