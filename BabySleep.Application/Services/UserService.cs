using BabySleep.Application.Interfaces;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public string GetUserGuid(string email)
        {
            return userRepository.GetUserGuid(email);
        }
    }
}
