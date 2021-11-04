using BabySleep.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BabySleep.EfData.Models
{
    [Table("Sleep")]
    public class Sleep
    {
        [Key]
        [Column("SleepGuid")]
        public Guid SleepGuid { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime StartTime { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndTime { get; set; }

        public Guid ChildGuid { get; set; }

        public short SleepPlace { get; set; }

        public short FeedingCount { get; set; }

        public short Quality { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        public short FallAsleepTime { get; set; }//minutes to fall asleep at bedtime

        public short AwakeningCount { get; set; }
    }
}
