using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTO
{
    public class ChildDto
    {
        public Guid ChildGuid { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public byte[] Picture { get; set; }
        public short? BirthWeek { get; set; }
        public bool IsEmptyPicture { get => Picture == null; }
        public string Age { get; set; }
    }
}
