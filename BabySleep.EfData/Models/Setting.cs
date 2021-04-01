using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabySleep.EfData.Models
{
    [Table("Settings")]
    public class Setting
    {
        [Key]
        [Column("RowID")]
        public int RowId { get; set; }

        [StringLength(5)]
        public string Language { get; set; }

        public short Type { get; set; }
    }
}
