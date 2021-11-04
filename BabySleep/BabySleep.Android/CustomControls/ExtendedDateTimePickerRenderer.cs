using System;
using System.Collections.Generic;
using System.ComponentModel;
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

[assembly: ExportRenderer(typeof(ExtendedDateTimePicker), typeof(ExtendedDateTimePickerRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class ExtendedDateTimePickerRenderer : ViewRenderer
    {
        public ExtendedDateTimePickerRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var element = Element as ExtendedDateTimePicker;
            element.DatePicker.IsEnabled = element.IsEnabled;
            element.Entry.IsEnabled = element.IsEnabled;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            var element = Element as ExtendedDateTimePicker;
            element.DatePicker.MaximumDate = element.MaximumDate;
            element.DatePicker.MinimumDate = element.MinimumDate;
            element.Entry.Text = element.DateTime.ToString(element.StringFormat);

            //var locale = new Locale(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            //element.DatePicker.Date.TextLocale = locale;
            //Resources.Configuration.SetLocale(locale);
            //Resources.Configuration.SetLayoutDirection(locale);
        }
    }
}