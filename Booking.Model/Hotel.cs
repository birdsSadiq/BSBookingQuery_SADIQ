using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    [Table("Hotel")]
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        #region Name
        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; } = "";
        #endregion
        #region Rating
        [Required]
        [Range(0, 10)]
        [Display(Name = "Rating")]
        public int Rating { get; set; }
        #endregion
        #region City
        [Required]
        [StringLength(50)]
        [Display(Name = "City")]
        public string City { get; set; } = "";
        #endregion
        #region Country
        [Required]
        [StringLength(50)]
        [Display(Name = "Country")]
        public string Country { get; set; } = "";
        #endregion
        #region Address
        [Required]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; } = "";
        #endregion
        #region LogoUrl
        public string LogoUrl { get; set; } = "";
        #endregion
        #region UserName
        [Display(Name = "User Name")]
        public string UserName { get; set; } = "";
        #endregion
        #region DateTime
        public DateTime EntryDateTime { get; set; }
        #endregion

        #region Code
        public string GetCode()
        {
            return Id.ToString().PadLeft(6, '0');
        }
        #endregion

        public virtual ICollection<HotelComment> HotelComments { get; set; }= new HashSet<HotelComment>();
    }
}
