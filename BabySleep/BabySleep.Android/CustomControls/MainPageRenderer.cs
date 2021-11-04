using BabySleep.Views;
using System;
using System.Linq;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using BabySleep.Interfaces;
using Android.Content;
using BabySleep.Droid.CustomControls;
using Android.Views;
using Google.Android.Material.BottomNavigation;

[assembly: ExportRenderer(typeof(MainPage), typeof(MainPageRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class MainPageRenderer : TabbedPageRenderer
    {
        IMenu menu;
        public MainPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            if (Element != null)
            {
                ((MainPage)Element).UpdateIcons += Handle_UpdateIcons;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);


            if (menu == null && e.PropertyName == "Renderer")
            {
                for (int i = 0; i <= this.ViewGroup.ChildCount - 1; i++)
                {
                    var childView = this.ViewGroup.GetChildAt(i);
                    if (childView is ViewGroup viewGroup)
                    {
                        for (int j = 0; j <= viewGroup.ChildCount - 1; j++)
                        {
                            var childRelativeLayoutView = viewGroup.GetChildAt(j);
                            if (childRelativeLayoutView is BottomNavigationView bView)
                            {
                                ((BottomNavigationView)childRelativeLayoutView).ItemIconTintList = null;
                                menu = bView.Menu;
                            }
                        }
                    }
                }
            }
        }

        void Handle_UpdateIcons(object sender, EventArgs e)
        {
            IMenu menuTabs = menu;

            if (menuTabs == null)
                return;

            for (var i = 0; i < Element.Children.Count; i++)
            {
                var child = Element.Children[i].BindingContext as ITabPage;
                var icon = child.CurrentIcon;
                if (string.IsNullOrEmpty(icon))
                    continue;

                IMenuItem tab = menuTabs.GetItem(i);
                SetCurrentTabIcon(tab, icon);
            }
        }

        void SetCurrentTabIcon(IMenuItem tab, string icon)
        {
            var name = icon.Split('.')[0];
            var resourceId = (int)typeof(Resource.Drawable).GetField(name).GetValue(null);

            var iconRes = Context.GetDrawable(resourceId);
            tab.SetIcon(iconRes);
        }
    }
}