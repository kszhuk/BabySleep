using Xamarin.Forms;

namespace BabySleep.CustomControls
{
    public class ExtendedTitleDatePicker : DatePicker
    {
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == DatePicker.DateProperty.PropertyName)
            {
                this.InvalidateMeasure();
            }
        }
    }
}
