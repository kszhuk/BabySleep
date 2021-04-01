using Xamarin.Forms;

namespace BabySleep.CustomControls
{
    /// <summary>
    /// Displays image as a circle
    /// If no image is selected, displays first letter of child name
    /// </summary>
    public class ExtendedImageCircle : Image
    {
        public string Name
        {
            get
            {
                return (string)GetValue(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string),
            typeof(ExtendedImageCircle));

        public bool IsEmptyPicture
        {
            get
            {
                return (bool)GetValue(IsEmptyPictureProperty);
            }
            set
            {
                SetValue(IsEmptyPictureProperty, value);
            }
        }
        public static readonly BindableProperty IsEmptyPictureProperty =
            BindableProperty.Create(nameof(IsEmptyPicture), typeof(bool),
            typeof(ExtendedImageCircle));
    }
}
