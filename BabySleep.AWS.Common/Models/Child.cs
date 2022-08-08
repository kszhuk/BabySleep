using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.AWS.Common.Models
{
    public class Child
    {
        public Guid UserGuid { get; set; }
        public Guid ChildGuid { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
