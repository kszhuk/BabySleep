using BabySleep.Application.DTO;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTOAssemblers
{
    public class ChildDtoAssembler : IChildDtoAssembler
    {
        public ChildDto WriteChildDto(Child child)
        {
            if(child == null)
            {
                child = new Child();
            }

            var childDto = new ChildDto()
            {
                ChildGuid = child.ChildGuid,
                Name = child.Name,
                BirthDate = child.BirthDate,
                Picture = child.Picture,
                BirthWeek = child.BirthWeek,
                Age = child.Age
            };

            return childDto;
        }

        public IList<ChildDto> WriteChildrenDto(IList<Child> children)
        {
            var childrenDto = new List<ChildDto>();
            foreach(var child in children)
            {
                childrenDto.Add(WriteChildDto(child));
            }

            return childrenDto;
        }
    }
}
