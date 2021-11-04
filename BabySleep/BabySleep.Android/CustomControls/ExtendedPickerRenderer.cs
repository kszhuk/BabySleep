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

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class ExtendedPickerRenderer : PickerRenderer
    {
        public ExtendedPickerRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                Control.SetBackground(ExtendedControlBase.CreateGradientDrawable());

                var locale = new Locale(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
                Control.TextLocale = locale;
                Resources.Configuration.SetLocale(locale);
                Resources.Configuration.SetLayoutDirection(locale);
            }
        }
    }
}