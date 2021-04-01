using System;
using System.Collections.Generic;
using System.Globalization;
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

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class ExtendedDatePickerRenderer : DatePickerRenderer
    {
        public ExtendedDatePickerRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
            var element = Element as ExtendedDatePicker;

            if (Control != null && e.NewElement != null)
            {
                Control.SetBackground(ExtendedControlBase.CreateGradientDrawable());
            }

            if (element.IsNewChild)
            {
                Control.Text = element.Placeholder;
                this.Control.SetTextColor(Android.Graphics.Color.Gray);
            }

            this.Control.TextChanged += (sender, arg) => {
                var selectedDate = arg.Text.ToString();
                if (selectedDate != element.Placeholder)
                {
                    this.Control.SetTextColor(Android.Graphics.Color.Black);
                }
            };

            var locale = new Locale(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            Control.TextLocale = locale;
            Resources.Configuration.SetLocale(locale);
            Resources.Configuration.SetLayoutDirection(locale);
        }
    }
}