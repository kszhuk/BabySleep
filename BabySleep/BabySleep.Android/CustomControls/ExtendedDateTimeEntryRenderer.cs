using Android.Content;
using Android.Graphics.Drawables;
using BabySleep.CustomControls;
using BabySleep.Droid.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedDateTimeEntry), typeof(ExtendedDateTimeEntryRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class ExtendedDateTimeEntryRenderer : EntryRenderer
    {
        public ExtendedDateTimeEntryRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                //var shape = new GradientDrawable();
                //shape.SetStroke(1, Android.Graphics.Color.ParseColor("#5AC8FF"));
                ////shape.SetColor(Android.Graphics.Color.AliceBlue);
                ////shape.SetCornerRadius(10);

                //Control.SetBackground(shape);

                Control.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#5AC8FF"));
            }
        }
    }
}