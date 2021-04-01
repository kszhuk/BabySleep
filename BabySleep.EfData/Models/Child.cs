using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabySleep.EfData.Models
{
    [Table("Children")]
    public class Child
    {
        [Key]
        [Column("ChildGUID")]
        public Guid ChildGuid { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        public byte[] Picture { get; set; }

        public short? BirthWeek { get; set; }
    }
}
