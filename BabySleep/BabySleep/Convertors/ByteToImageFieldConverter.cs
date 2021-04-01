using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace BabySleep.Convertors
{
    /// <summary>
    /// Is used for ExtendedImageCircle: converts picture from db to ImageSource
    /// In case of empty picture displays 'add photo' icon
    /// </summary>
    public class ByteToImageFieldConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null; 
            if (value is null)
            {
                retSource = ImageSource.FromResource("BabySleep.Resources.add_photo_icon.jpg", typeof(ByteToImageFieldConverter).GetTypeInfo().Assembly);
            }
            else
            {
                byte[] imageAsBytes = (byte[])value;
                retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }
            return retSource;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
