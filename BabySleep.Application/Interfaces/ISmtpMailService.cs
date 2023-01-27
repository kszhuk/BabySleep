using BabySleep.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Interfaces
{
    public interface ISmtpMailService
    {
        void Send(EmailMessageDto message);
    }
}
