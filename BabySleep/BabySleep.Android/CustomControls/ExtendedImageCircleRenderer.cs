using System;
using Android.Content;
using Android.Graphics;
using Android.Views;
using BabySleep.CustomControls;
using BabySleep.Droid.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(ExtendedImageCircle), typeof(ExtendedImageCircleRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class ExtendedImageCircleRenderer : ImageRenderer
    {
        public ExtendedImageCircleRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                if ((int)Android.OS.Build.VERSION.SdkInt < 18)
                    SetLayerType(LayerType.Software, null);
            }
        }
        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            try
            {
                var element = Element as ExtendedImageCircle;

                var radius = Math.Min(Width, Height) / 2;
                var strokeWidth = 10;
                radius -= strokeWidth / 2;
                Path path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);
                canvas.DrawColor(Android.Graphics.Color.AliceBlue);
                
                var result = true;
                if (element.IsEmptyPicture)
                {
                    string firstLetter = element.Name[0].ToString().ToUpper();

                    var textPaint = new Paint
                    {
                        Color = Android.Graphics.Color.ParseColor("#007AFF"),
                        TextSize = 80,
                        TextAlign = Paint.Align.Center
                    };

                    var textBounds = new Rect();
                    textPaint.GetTextBounds(firstLetter, 0, 1, textBounds);

                    var yPos = (float)((canvas.Height / 2) - ((textPaint.Descent() + textPaint.Ascent()) / 2));

                    canvas.DrawText(firstLetter, Width / 2,
                        //-textBounds.Top, 
                        yPos,
                        textPaint);
                }
                else
                {
                    result = base.DrawChild(canvas, child, drawingTime);
                }
                canvas.Restore();
                path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
                var paint = new Paint();
                paint.AntiAlias = true;
                paint.StrokeWidth = 5;
                paint.SetStyle(Paint.Style.Stroke);
                paint.Color = global::Android.Graphics.Color.AliceBlue;
                canvas.DrawPath(path, paint);
                paint.Dispose();
                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return base.DrawChild(canvas, child, drawingTime);
        }
    }
}