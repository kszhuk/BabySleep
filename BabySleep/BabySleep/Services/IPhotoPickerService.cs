using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BabySleep.Services
{

    /// <summary>
    /// For uploading images on Edit child page
    /// </summary>
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
