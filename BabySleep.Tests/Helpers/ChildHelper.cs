using BabySleep.Application.DTO;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Tests.Helpers
{
    public static class ChildHelper
    {
        public static Child FillChild(Guid guid, string name, byte[] picture, short? birthWeek, DateTime birthDate)
        {
            return new Child()
            {
                ChildGuid = guid,
                Name = name,
                Picture = picture,
                BirthWeek = birthWeek,
                BirthDate = birthDate
            };
        }

        public static ChildDto FillChildDto(Guid guid, string name, byte[] picture, short? birthWeek, DateTime birthDate, string age)
        {
            return new ChildDto()
            {
                ChildGuid = guid,
                Name = name,
                Picture = picture,
                BirthWeek = birthWeek,
                BirthDate = birthDate,
                Age = age
            };
        }
    }
}
