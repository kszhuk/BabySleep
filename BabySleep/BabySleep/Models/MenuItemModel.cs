using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.Models
{
    public enum MenuItemType
    {
        Main,
        EditLanguage,
        EditAccountInfo,
        Sync
    }

    /// <summary>
    /// Items of application menu
    /// </summary>
    public class MenuItemModel
    {
        public MenuItemType Id { get; set; }
        public string Title { get; set; }
        public ImageSource Icon { get; set; }
    }
}
