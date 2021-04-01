using BabySleep.ViewModels;
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
    public partial class ChildEntryPage : ContentPage
    {
        public ChildEntryPage()
        {
            InitializeComponent();
        }

        public ChildEntryPage(ChildEntryPageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}