using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    [Table("Demo")]
    public class Demo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; } = "";

        public string LogoUrl { get; set; } = "";


        #region UserName
        public string UserName { get; set; } = "";

        #endregion
    }
}
