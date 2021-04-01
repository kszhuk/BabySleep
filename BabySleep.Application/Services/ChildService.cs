using BabySleep.Application.DTO;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Interfaces;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Services
{
    public class ChildService : IChildService
    {
        private readonly IChildRepository childRepository;
        private readonly IChildDtoAssembler childDtoAssembler;

        public ChildService(IChildRepository childRepository, IChildDtoAssembler childDtoAssembler)
        {
            this.childDtoAssembler = childDtoAssembler;
            this.childRepository = childRepository;
        }

        public void SaveChild(ChildDto childDto)
        {
            var child = new Child(
                childDto.ChildGuid, 
                childDto.BirthDate, 
                childDto.BirthWeek, 
                childDto.Name, 
                childDto.Picture);

            if(child.ChildGuid == Guid.Empty)
            {
                childRepository.Add(child);
            }
            else
            {
                if (child.Validate())
                {
                    childRepository.Update(child);
                }
            }
        }

        public IList<ChildDto> GetChildren()
        {
            return childDtoAssembler.WriteChildrenDto(childRepository.GetAll());
        }

        public int GetChildrenCount()
        {
            return childRepository.Count();
        }

        public ChildDto GetChild(Guid childGuid)
        {
            return childDtoAssembler.WriteChildDto(childRepository.Get(childGuid));
        }

        public void DeleteChild(Guid childGuid)
        {
            childRepository.Delete(childGuid);
        }

        public ChildDto GetFirstChild()
        {
            return childDtoAssembler.WriteChildDto(childRepository.GetFirst());
        }
    }
}
