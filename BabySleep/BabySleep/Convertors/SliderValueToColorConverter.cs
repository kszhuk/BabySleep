using System;
using System.Globalization;
using Xamarin.Forms;

namespace BabySleep.Convertors
{
    public class SliderValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int sliderValue;
            if(Int32.TryParse(value.ToString(), out sliderValue))
            {
                if (sliderValue > 6)
                {
                    return Color.FromHex("#4fc3f7");
                }

                if (sliderValue < 4)
                {
                    return Color.FromHex("#b3e5fc");
                }

                return Color.FromHex("#81d4fa");
            }

            return Color.AliceBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
