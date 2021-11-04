using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.Convertors
{
    public class ImageSleepTypeConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //return ImageSource.FromResource("BabySleep.Resources.add_photo_icon.jpg", typeof(ByteToImageFieldConverter).GetTypeInfo().Assembly);
            return (bool)value ? ImageSource.FromResource("BabySleep.Resources.sun-icon.png", typeof(ImageSleepTypeConverter).GetTypeInfo().Assembly)
                : ImageSource.FromResource("BabySleep.Resources.moon-icon.png", typeof(ImageSleepTypeConverter).GetTypeInfo().Assembly);
            //"BabySleep.Resources.sun-icon.png" : "BabySleep.Resources.moon-icon.png";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false; // not needed
        }
    }
}
