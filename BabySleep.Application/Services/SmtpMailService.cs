using AutoMapper;
using BabySleep.Application.DTO;
using BabySleep.Application.Interfaces;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Business.Interfaces;

namespace BabySleep.Application.Services
{
    public class SmtpMailService : ISmtpMailService
    {
        private readonly ISmtpMailBusinessService mailService;

        public SmtpMailService(ISmtpMailBusinessService mailService)
        {
            this.mailService = mailService;
        }

        public void Send(EmailMessageDto message)
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<EmailMessageDto, EmailMessage>());
            var mapper = new Mapper(config);
            var messageSend = mapper.Map<EmailMessage>(message);

            mailService.Send(messageSend);
        }
    }
}
