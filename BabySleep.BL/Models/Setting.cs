using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.BL.Models
{
    public class Setting
    {
        public int RowId { get; set; }

        public string Language { get; set; }

        public SettingType Type { get; set; }
    }

    public enum SettingType
    {
        Language = 0
    }
}
