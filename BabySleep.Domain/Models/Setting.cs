using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Domain.Models
{
    public class Setting
    {
        public int RowId { get; set; }

        public string Language { get; set; }

        public SettingType Type { get; set; }
    }

    public enum SettingType : short
    {
        Language = 0
    }
}
