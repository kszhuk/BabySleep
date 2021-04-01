using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BabySleep.Droid.CustomControls
{
    public static class ExtendedControlBase
    {
        public static GradientDrawable CreateGradientDrawable()
        {
            var shape = new GradientDrawable();
            shape.SetStroke(1, Android.Graphics.Color.ParseColor("#5AC8FF"));
            shape.SetColor(Android.Graphics.Color.AliceBlue);
            shape.SetCornerRadius(10);

            return shape;
        }
    }
}