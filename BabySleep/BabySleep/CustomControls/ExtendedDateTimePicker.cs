using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.CustomControls
{
    /// <summary>
    /// Creates DateTimePicker
    /// </summary>
    public class ExtendedDateTimePicker : ContentView//, INotifyPropertyChanged
    {
        public ExtendedEntry Entry { get; private set; } = new ExtendedEntry();
        public ExtendedDatePicker DatePicker { get; private set; } = new ExtendedDatePicker() { IsVisible = false };
        public TimePicker TimePicker { get; private set; } = new TimePicker() { IsVisible = false };

        public string StringFormat
        {
            get => "MM/dd/yyyy HH:mm";
        }

        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            set
            {
                SetValue(DateTimeProperty, value);
                //OnPropertyChanged("DateTime");
            }
        }
        public static readonly BindableProperty DateTimeProperty =
            BindableProperty.Create("DateTime", typeof(DateTime), typeof(ExtendedDateTimePicker), DateTime.Now, 
            BindingMode.TwoWay, propertyChanged: DateTimePropertyChanged);

        private TimeSpan Time
        {
            get => TimeSpan.FromTicks(DateTime.Ticks);
            set => DateTime = new DateTime(DateTime.Date.Ticks).AddTicks(value.Ticks);
        }

        private DateTime Date
        {
            get => DateTime.Date;
            set => DateTime = new DateTime(DateTime.TimeOfDay.Ticks).AddTicks(value.Ticks);
        }

        public DateTime MaximumDate
        {
            get
            {
                return (DateTime)GetValue(MaximumDateProperty);
            }
            set
            {
                SetValue(MaximumDateProperty, value);
            }
        }
        public static readonly BindableProperty MaximumDateProperty =
            BindableProperty.Create(nameof(MaximumDate), typeof(DateTime),
            typeof(ExtendedDateTimePicker));

        public DateTime MinimumDate
        {
            get
            {
                return (DateTime)GetValue(MinimumDateProperty);
            }
            set
            {
                SetValue(MinimumDateProperty, value);
            }
        }
        public static readonly BindableProperty MinimumDateProperty =
            BindableProperty.Create(nameof(MinimumDate), typeof(DateTime),
            typeof(ExtendedDateTimePicker));

        public ExtendedDateTimePicker()
        {
            Content = new StackLayout()
            {
                Children =
                {
                    DatePicker,
                    TimePicker,
                    Entry
                }
            };

            DatePicker.SetBinding(ExtendedDatePicker.DateProperty, nameof(Date)); 
            TimePicker.SetBinding(TimePicker.TimeProperty, nameof(Time));
            TimePicker.Unfocused += (sender, args) => Time = TimePicker.Time;
            DatePicker.Focused += (s, a) => UpdateEntryText();

            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() => DatePicker.Focus())
            });
            Entry.Focused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() => DatePicker.Focus());
            };
            DatePicker.Unfocused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    TimePicker.Focus();
                    Date = DatePicker.Date;
                    UpdateEntryText();
                });
            };

            Entry.InputTransparent = true;
        }

        private void UpdateEntryText()
        {
            Entry.Text = DateTime.ToString(StringFormat);
        }

        static void DateTimePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var timePicker = (bindable as ExtendedDateTimePicker);
            timePicker.UpdateEntryText();
        }
    }
}
