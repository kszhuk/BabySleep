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
                    return Color.LawnGreen;
                }

                if (sliderValue < 4)
                {
                    return Color.Red;
                }

                return Color.Yellow;
            }

            return Color.AliceBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
