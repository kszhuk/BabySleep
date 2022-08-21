using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BabySleep.CustomControls;
using BabySleep.Droid.CustomControls;
using BabySleep.Helpers;
using BabySleep.Resources.Resx;
using Java.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRenderer))]
namespace BabySleep.Droid.CustomControls
{
    public class ExtendedPickerRenderer : PickerRenderer
    {
        private Context context;
        AlertDialog listDialog;
        string[] items;

        public ExtendedPickerRenderer(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                Control.SetBackground(ExtendedControlBase.CreateGradientDrawable());

                var locale = new Locale(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
                Control.TextLocale = locale;
                Resources.Configuration.SetLocale(locale);
                Resources.Configuration.SetLayoutDirection(locale);

                Control.Click += Control_Click;
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            Picker model = Element;
            items = model.Items.ToArray(); 
            AlertDialog.Builder builder = new AlertDialog.Builder(context);

            var titleView = LayoutInflater.From(context).Inflate(Resource.Layout.custom_title, null);
            var textView = (TextView)titleView.FindViewById(Resource.Id.customTitleText);
            var text = model.Title ?? "";
            textView.SetText(text.ToCharArray(), 0, text.Length);
            builder.SetCustomTitle(titleView);

            builder.SetNegativeButton(GeneralResources.Cancel, (s, a) =>
            {
                Control?.ClearFocus();
                builder = null;
            });

            Android.Views.View view = LayoutInflater.From(context).Inflate(Resource.Layout.listview, null);
            view.TextAlignment = Android.Views.TextAlignment.Center;

            Android.Widget.ListView listView = view.FindViewById<Android.Widget.ListView>(Resource.Id.listView1);
            listView.TextAlignment = Android.Views.TextAlignment.Center;

            MyAdapter myAdapter = new MyAdapter(items, Element.SelectedIndex);
            listView.Adapter = myAdapter;
            listView.ItemClick += ListView_ItemClick;
            builder.SetView(view);
            listDialog = builder.Create();
            listDialog.Window.DecorView.SetBackgroundColor(Android.Graphics.Color.ParseColor(Constants.COLOR_BACKGROUND)); // set the dialog background color
            listDialog.Show();
            Android.Widget.Button button = listDialog.GetButton((int)DialogButtonType.Negative);
            button.SetTextColor(Android.Graphics.Color.ParseColor(Constants.COLOR_BUTTON)); // set the button bottom color
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Control.Text = items[e.Position];
            Element.SelectedIndex = e.Position;
            listDialog.Dismiss();
            listDialog = null;
        }

        class MyAdapter : BaseAdapter
        {
            private string[] items;
            private int selectedIndex;

            public MyAdapter(string[] items)
            {
                this.items = items;
            }

            public MyAdapter(string[] items, int selectedIndex) : this(items)
            {
                this.selectedIndex = selectedIndex;
            }

            public override int Count => items.Length;

            public override Java.Lang.Object GetItem(int position)
            {
                return items[position];
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
            {
                if (convertView == null)
                {
                    convertView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.listview_item, null);
                }
                TextView textView = convertView.FindViewById<TextView>(Resource.Id.textView1);
                textView.Text = items[position];
                return convertView;
            }
        }
    }
}