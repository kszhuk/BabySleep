using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.Convertors
{
    public class StringNullOrEmptyBoolConverter : IValueConverter
    {
        /// <summary>
        /// Returns false if string is null or empty
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var s = value as string;
            return !string.IsNullOrWhiteSpace(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
