using BabySleep.Application.DTO;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTOAssemblers
{
    public interface IChildDtoAssembler
    {
        ChildDto WriteChildDto(Child child);
        IList<ChildDto> WriteChildrenDto(IList<Child> children);
    }
}
