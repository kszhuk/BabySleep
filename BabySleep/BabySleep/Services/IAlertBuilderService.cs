using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BabySleep.Services
{
    public interface IAlertBuilderService
    {
        Task ShowExceptionAsync(string title, string message, string positiveButton);
        Task<bool> ShowQuestionAsync(string title, string message, string positiveButton, string negativeButton);
    }
}
