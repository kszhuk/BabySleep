using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.Convertors
{
    public class NullableShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nullable = value as short?;
            var result = string.Empty;

            if (nullable.HasValue)
            {
                result = nullable.Value.ToString();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            short shortValue;
            short? result = null;

            if (short.TryParse(stringValue, out shortValue))
            {
                result = new Nullable<short>(shortValue);
            }

            return result;
        }
    }
}
