using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BabySleep.CustomControls;
using BabySleep.Droid.CustomControls;
using Java.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedTitleDatePicker), typeof(ExtendedTitleDatePickerRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class ExtendedTitleDatePickerRenderer : DatePickerRenderer
    {
        public ExtendedTitleDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                Control.SetTextColor(Android.Graphics.Color.ParseColor("#007AFF"));

                var element = Element as ExtendedTitleDatePicker;
                var textSize = Device.GetNamedSize(NamedSize.Title, element);
                Control.TextSize = (float)textSize;

                var gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.Transparent);
                Control.SetBackground(gd);
            }

            var locale = new Locale(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            Control.TextLocale = locale;
            Resources.Configuration.SetLocale(locale);
            Resources.Configuration.SetLayoutDirection(locale);
        }
    }
}