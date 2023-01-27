using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Business.Interfaces
{
    public interface ISmtpMailBusinessService
    {
        void Send(EmailMessage message);
    }
}
