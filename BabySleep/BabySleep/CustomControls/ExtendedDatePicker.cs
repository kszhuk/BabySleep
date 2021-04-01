using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.CustomControls
{
    /// <summary>
    /// Displays placeholder for new child
    /// Applies common style for editor elements
    /// </summary>
    public class ExtendedDatePicker : DatePicker
    {
        public string Placeholder { get; set; }
        public bool IsNewChild
        {
            get
            {
                return (bool)GetValue(IsNewChildProperty);
            }
            set
            {
                SetValue(IsNewChildProperty, value);
            }
        }
        public static readonly BindableProperty IsNewChildProperty =
            BindableProperty.Create(nameof(IsNewChild), typeof(bool),
            typeof(ExtendedDatePicker));
    }
}
