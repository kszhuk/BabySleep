using Android.Content;
using BabySleep.CustomControls;
using BabySleep.Droid.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class ExtendedEditorRenderer : EditorRenderer
    {
        public ExtendedEditorRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                Control.SetBackground(ExtendedControlBase.CreateGradientDrawable());
            }
        }
    }
}