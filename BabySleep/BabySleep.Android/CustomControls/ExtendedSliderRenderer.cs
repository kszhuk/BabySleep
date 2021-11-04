using Android.Content;
using Android.Graphics.Drawables;
using BabySleep.CustomControls;
using BabySleep.Droid.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedSlider), typeof(ExtendedSliderRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class ExtendedSliderRenderer : SliderRenderer
    {
        public ExtendedSliderRenderer(Context context) : base(context) 
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                // Set custom drawable resource
                Control.SetProgressDrawableTiled(Resources.GetDrawable(Resource.Drawable.custom_progressbar_style, (this.Context).Theme));

                // Hide thumb to make it look cool lol
                Control.SetThumb(new ColorDrawable(Color.Transparent.ToAndroid()));
            }
        }
    }
}