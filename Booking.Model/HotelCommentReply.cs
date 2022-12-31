using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    [Table("HotelCommentReply")]
    public class HotelCommentReply
    {
        [Key]
        public int Id { get; set; }

        #region HotelId
        public int? HotelId { get; set; }
        #endregion
        #region HotelCommentId fk
        public int? HotelCommentId { get; set; }

        [ForeignKey("HotelCommentId")]
        public virtual Hotel HotelComment { get; set; } = new();
        #endregion

        #region Name
        [Required]
        [StringLength(150)]
        [Display(Name = "Reply")]
        public string Name { get; set; } = "";
        #endregion
        #region Show
        public bool Show { get; set; }
        #endregion
        #region UserName
        [Display(Name = "User Name")]
        public string UserName { get; set; } = "";
        #endregion
        #region DateTime
        public DateTime EntryDateTime { get; set; }
        #endregion
    }
}
