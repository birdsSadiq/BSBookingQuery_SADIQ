using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    [Table("HotelComment")]
    public class HotelComment
    {
        [Key]
        public int Id { get; set; }

        #region HotelId fk
        public int? HotelId { get; set; }

        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; } = new();
        #endregion

        #region Name
        [Required]
        [StringLength(150)]
        [Display(Name = "Comment")]
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

        public virtual ICollection<HotelCommentReply> HotelCommentReplies { get; set; }
    }
}
