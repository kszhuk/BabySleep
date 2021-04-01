using BabySleep.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.BL.Helpers
{
    public static class Environment
    {
        public static string GetDbFolder()
        {
            return DependencyService.Get<IEnvironmentService>().GetDbFolder();
        }
    }
}
