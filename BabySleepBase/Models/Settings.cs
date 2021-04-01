using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySleepBase.Models
{
    public class Setting
    {
        [PrimaryKey]
        public int RowId { get; set; }
        public string Language { get; set; }
    }
}
