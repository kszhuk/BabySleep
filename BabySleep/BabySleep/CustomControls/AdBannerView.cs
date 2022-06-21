using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.CustomControls
{
    public class AdBannerView : View
    {
        public enum Sizes { Standardbanner, LargeBanner, MediumRectangle, FullBanner, Leaderboard, SmartBannerPortrait }
        public Sizes Size { get; set; }
        public AdBannerView()
        {
            this.BackgroundColor = Color.Accent;
        }
    }
}
