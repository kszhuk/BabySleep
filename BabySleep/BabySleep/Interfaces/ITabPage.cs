using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Interfaces
{
    public interface ITabPage
    {
        bool IsSelected { get; set; }
        string CurrentIcon { get; }
    }
}
