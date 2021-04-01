using BabySleep.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Interfaces
{
    public interface IChildService
    {
        ChildDto GetChild(Guid childGuid);
        ChildDto GetFirstChild();
        IList<ChildDto> GetChildren();
        int GetChildrenCount();
        void SaveChild(ChildDto child);
        void DeleteChild(Guid childGuid);
    }
}
