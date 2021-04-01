using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySleep.BL.Interfaces
{
    /// <summary>
    /// Custom Interface for getting the OS specific folder for the DB file
    /// </summary>
    public interface IEnvironmentService
    {
        string GetDbFolder();
    }
}
