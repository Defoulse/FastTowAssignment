using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FastTowAssignment.Models
{
    public class Order
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual IdentityUser Client { get; set; }
        public long DriverId { get; set; }
        [ForeignKey("DriverId")]
        public virtual IdentityUser Driver { get; set; }
        [Required]
        public long DepartureCityId { get; set; }
        [Required]
        public long DestinationCityId { get; set; }
        [ForeignKey("DepartureCityId")]
        public virtual City DepartureCity { get; set; }
        [ForeignKey("DestinationCityId")]
        public virtual City DestinationCity { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public long CurrentStatusId { get; set; }
        [ForeignKey("CurrentStatusId")]
        public virtual Status CurrentStatus { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
    }
}
