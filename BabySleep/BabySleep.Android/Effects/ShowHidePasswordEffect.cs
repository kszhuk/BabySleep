using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using BabySleep.Droid.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(ShowHidePasswordEffect), "ShowHidePasswordEffect")]
namespace BabySleep.Droid.Effects
{
    public class ShowHidePasswordEffect : PlatformEffect
    {
		protected override void OnAttached()
		{
			ConfigureControl();
		}

		protected override void OnDetached()
		{
		}

		private void ConfigureControl()
		{
			EditText editText = ((EditText)Control);
			editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, Resource.Drawable.hide_pass, 0);
			editText.SetOnTouchListener(new OnDrawableTouchListener());

		}
	}

	public class OnDrawableTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
	{
		public bool OnTouch(Android.Views.View v, MotionEvent e)
		{
			if (v is EditText && e.Action == MotionEventActions.Up)
			{
				EditText editText = (EditText)v;
				if (e.RawX >= (editText.Right - editText.GetCompoundDrawables()[2].Bounds.Width()))
				{
					if (editText.TransformationMethod == null)
					{
						editText.TransformationMethod = PasswordTransformationMethod.Instance;
						editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, Resource.Drawable.hide_pass, 0);
						editText.SetSelection(editText.Length());
					}
					else
					{
						editText.TransformationMethod = null;
						editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, Resource.Drawable.show_pass, 0);
						editText.SetSelection(editText.Length());
					}

					return true;
				}
			}

			return false;
		}
	}
}