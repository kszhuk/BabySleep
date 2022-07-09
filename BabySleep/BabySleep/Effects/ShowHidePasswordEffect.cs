using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.Effects
{
	public class ShowHidePasswordEffect : RoutingEffect
	{
		public string EntryText { get; set; }
		public ShowHidePasswordEffect() : base("Xamarin.ShowHidePasswordEffect") { }
	}
}
